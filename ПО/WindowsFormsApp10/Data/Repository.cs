using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;

namespace RemontKt.Data
{
    public class Repository
    {
        #region Singleton

        private static Repository _repository;

        public static Repository RepositoryInstance => _repository ?? (_repository = new Repository());

        private Repository()
        {
        }

        #endregion

        /// <summary>
        /// Метод создания обьекта контекста
        /// </summary>
        private static RemontDBEntities CreateContext()
        {
            return new RemontDBEntities();
        }

        /// <summary>
        /// Метод проверки наличия администратора в системе
        /// </summary>        
        public static bool FirstStart()
        {
            using (var context = CreateContext())
            {
                return !context.Employees.Any();
            }
        }

        /// <summary>
        /// Текущий сотрудник
        /// </summary>
        public static Employee LoginedEmployee { get; private set; }


        /// <summary>
        /// Метод для вход сотрудника в систему
        /// </summary>
        public bool Login(string login, string password)
        {
            using (var context = CreateContext())
            {

                LoginedEmployee = context.Employees.Include(nameof(Post)).
                    FirstOrDefault(user => user.Login == login && user.Password == password);
                return LoginedEmployee != null;
            }
        }

        #region Методы реализации поиска

        /// <summary>
        /// Метод поиска выполненных работ c заменой запчастей
        /// </summary>
        public List<RequestWork> SearchRequestWorks(DateTime? dateFrom, DateTime? dateTo, TechnicType technicType, SpareType spareType, string spareName, string spareArticle)
        {
            using (var context = CreateContext())
            {
                var query = GetDbQuery<RequestWork>(context).AsQueryable()
                    .Where(work => work.Spare != null);

                if (dateFrom.HasValue && dateTo.HasValue)// если есть обе даты - ищем период
                {
                    query = query.Where(requestWork => requestWork.RequestWorkDt >= dateFrom.Value
                                                        && requestWork.RequestWorkDt <= dateTo.Value);
                }
                if (dateFrom.HasValue && !dateTo.HasValue)// ищем все даты после указанной
                {
                    query = query.Where(requestWork => requestWork.RequestWorkDt >= dateFrom.Value);
                }
                if (!dateFrom.HasValue && dateTo.HasValue)// ищем все даты до указанной
                {
                    query = query.Where(requestWork => requestWork.RequestWorkDt <= dateTo.Value);
                }
                if (technicType.TechnicType_ID > 0)
                {
                    query = query.Where(requestWork => requestWork.Spare.SpareType.TechnicType_ID == technicType.TechnicType_ID);
                }
                if (spareType.SpareType_ID > 0)
                {
                    query = query.Where(requestWork => requestWork.Spare.SpareType_ID == spareType.SpareType_ID);
                }
                if (!string.IsNullOrWhiteSpace(spareName))
                {
                    query = query.Where(requestWork => requestWork.Spare.SpareName.ToLower().Contains(spareName.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(spareArticle))
                {
                    query = query.Where(requestWork => requestWork.Spare.Article.ToLower().Contains(spareArticle.ToLower()));
                }
                return query.ToList();

            }
        }


        /// <summary>
        /// Метод поиска выполненных работ
        /// </summary>
        public List<RequestWork> SearchRequestWorks(DateTime? dateFrom, DateTime? dateTo, Work work, Employee employee, int requestId)
        {
            using (var context = CreateContext())
            {
                var query = GetDbQuery<RequestWork>(context).AsQueryable();
                if (dateFrom.HasValue && dateTo.HasValue)// если есть обе даты - ищем период
                {
                    query = query.Where(requestWorkk => requestWorkk.RequestWorkDt >= dateFrom.Value
                                                        && requestWorkk.RequestWorkDt <= dateTo.Value);
                }
                if (dateFrom.HasValue && !dateTo.HasValue)// ищем все даты после указанной
                {
                    query = query.Where(requestWorkk => requestWorkk.RequestWorkDt >= dateFrom.Value);
                }
                if (!dateFrom.HasValue && dateTo.HasValue)// ищем все даты до указанной
                {
                    query = query.Where(requestWorkk => requestWorkk.RequestWorkDt <= dateTo.Value);
                }
                if (work.Work_ID > 0)
                {
                    query = query.Where(requestWorkk => requestWorkk.Work_ID == work.Work_ID);
                }
                if (employee.Employee_ID > 0)
                {
                    query = query.Where(requestWorkk => requestWorkk.Employee_ID == employee.Employee_ID);
                }
                if (requestId > 0)
                {
                    query = query.Where(requestWorkk => requestWorkk.Request_ID == requestId);
                }
                return query.ToList();

            }
        }

        /// <summary>
        /// Метод поиска заявок по заданным параметрам
        /// </summary>
        public List<Request> SearchRequests(DateTime? dateFrom, DateTime? dateTo, string clientFName,
            FaultType faultType, int requestId, Status status, TechnicType technicType, Manufacturer manufacturer)
        {
            using (var context = CreateContext())
            {
                var query = GetDbQuery<Request>(context).AsQueryable();
                if (dateFrom.HasValue && dateTo.HasValue)// если есть обе даты - ищем период
                {
                    query = query.Where(request => request.RequestDt >= dateFrom.Value && request.RequestDt <= dateTo.Value);
                }
                if (dateFrom.HasValue && !dateTo.HasValue)// ищем все даты после указанной
                {
                    query = query.Where(request => request.RequestDt >= dateFrom.Value);
                }
                if (!dateFrom.HasValue && dateTo.HasValue)// ищем все даты до указанной
                {
                    query = query.Where(request => request.RequestDt <= dateTo.Value);
                }
                if (!string.IsNullOrWhiteSpace(clientFName))
                {
                    query = query.Where(request => request.Client.FName.ToLower().Contains(clientFName));
                }
                if (faultType.FaultType_ID > 0)
                {
                    query = query.Where(request => request.FaultType_ID == faultType.FaultType_ID);
                }
                if (requestId > 0)
                {
                    query = query.Where(request => request.Request_ID == requestId);
                }
                if (status.Status_ID > 0)
                {
                    query = query.Where(request => request.Status_ID == status.Status_ID);
                }
                if (technicType.TechnicType_ID > 0)
                {
                    query = query.Where(request => request.Technic.TechnicType_ID == technicType.TechnicType_ID);
                }
                if (manufacturer.Manufacturer_ID > 0)
                {
                    query = query.Where(request => request.Technic.Manufacturer_ID == manufacturer.Manufacturer_ID);
                }
                return query.ToList();
            }
        }

        /// <summary>
        /// Метод поиска техники по заданным параметрам
        /// </summary>
        public List<Technic> SearchTechnics(TechnicType technicType, string manufacturer, string model)
        {
            using (var context = CreateContext())
            {
                var query = GetDbQuery<Technic>(context).AsQueryable();
                if (technicType.TechnicType_ID != 0)
                {
                    query = query.Where(technic => technic.TechnicType_ID == technicType.TechnicType_ID);
                }
                if (!string.IsNullOrWhiteSpace(manufacturer))
                {
                    query = query.Where(technic => technic.Manufacturer.ManufacturerName.ToLower().Contains(manufacturer.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(model))
                {
                    query = query.Where(technic => technic.Model.ToLower().Contains(model.ToLower()));
                }
                return query.ToList();
            }
        }


        /// <summary>
        /// Метод поиска запчастей по параметрам
        /// </summary>
        public List<Spare> SearchSpares(TechnicType technicType, SpareType spareType, string spareName, string article)
        {
            using (var context = CreateContext())
            {
                var query = GetDbQuery<Spare>(context).AsQueryable();
                if (technicType.TechnicType_ID > 0)
                {
                    query = query.Where(spare => spare.SpareType.TechnicType_ID == technicType.TechnicType_ID);
                }
                if (spareType.SpareType_ID > 0)
                {
                    query = query.Where(spare => spare.SpareType_ID == spareType.SpareType_ID);
                }
                if (!string.IsNullOrEmpty(spareName))
                {
                    query = query.Where(spare => spare.SpareName.ToLower().Contains(spareName.ToLower()));
                }
                if (!string.IsNullOrEmpty(article))
                {
                    query = query.Where(spare => spare.Article.ToLower().Contains(article.ToLower()));
                }
                return query.ToList();
            }
        }
        #endregion

        #region CRUD методы

        
        /// <summary>
        /// Внутренний метод получения запроса с присоединёнными свойствами
        /// </summary>
        private DbQuery<T> GetDbQuery<T>(RemontDBEntities context) where T : class
        {
            if (typeof(T).Name == nameof(Request))
            {
                context.SpareTypes.Load();
                context.Spares.Load();
                context.Works.Load();
                context.Posts.Load();
                context.Manufacturers.Load();
                context.TechnicTypes.Load();
                context.Parameters.Load();
                context.Technics.Include("TechnicParameters").Load();
                return context.Set<T>()
                    .Include(nameof(Client))
                    .Include(nameof(Employee))
                    .Include(nameof(FaultType))
                    .Include(nameof(Status))
                    .Include(nameof(Technic))
                    .Include("RequestWorks");
            }
            if (typeof(T).Name == nameof(RequestWork))
            {
                context.TechnicTypes.Load();
                context.SpareTypes.Load();
                context.Posts.Load();
                return context.Set<T>()
                    .Include(nameof(Employee))
                    .Include(nameof(Spare))
                    .Include(nameof(Work))
                    .Include(nameof(Request));
            }
            if (typeof(T).Name == nameof(Employee))
            {
                context.Posts.Load();
                return context.Set<T>();
            }
            if (typeof(T).Name == nameof(SpareType))
            {
                context.TechnicTypes.Load();
                return context.Set<T>();
            }
            if (typeof(T).Name == nameof(Spare))
            {
                context.TechnicTypes.Load();
                context.SpareTypes.Load();
                return context.Set<T>();
            }
            if (typeof(T).Name == nameof(TechnicParameter))
            {
                context.Parameters.Load();
                return context.Set<T>();
            }
            if (typeof(T).Name == nameof(Technic))
            {
                context.TechnicTypes.Load();
                return context.Set<T>().Include(nameof(TechnicType)).Include(nameof(Manufacturer));
            }
            return context.Set<T>();
        }

        /// <summary>
        /// Метод выбора коллекции сущностей
        /// </summary>
        public List<T> GetEntityes<T>() where T : class
        {
            using (var context = CreateContext())
            {
                return GetDbQuery<T>(context).ToList();
            }
        }

        /// <summary>
        /// Метод выбора коллекции сущностей по указнному условию предиката
        /// </summary>
        public List<T> GetEntityes<T>(Func<T, bool> predicate) where T : class
        {
            using (var context = CreateContext())
            {
                return GetDbQuery<T>(context).Where(predicate).ToList();
            }
        }

        /// <summary>
        /// Метод получения коллекции сущностей T сгруппированных по ключу S
        /// </summary>
        public List<IGrouping<S, T>> GetRequestGroupBy<T, S>(Func<T, S> keySelector, DateTime? dateFrom, DateTime? dateTo) where T : Request
        {
            using (var context = CreateContext())
            {
                var query = GetDbQuery<T>(context).AsQueryable();

                if (dateFrom.HasValue && dateTo.HasValue) // если есть обе даты - ищем период
                {
                    query = query.Where(request => request.RequestDt >= dateFrom.Value && request.RequestDt <= dateTo.Value);
                }
                if (dateFrom.HasValue && !dateTo.HasValue) // ищем все даты после указанной
                {
                    query = query.Where(request => request.RequestDt >= dateFrom.Value);
                }
                if (!dateFrom.HasValue && dateTo.HasValue) // ищем все даты до указанной
                {
                    query = query.Where(request => request.RequestDt <= dateTo.Value);
                }
                return query.AsEnumerable().GroupBy(keySelector).ToList();
            }
        }

        /// <summary>
        /// Метод обновления сущности
        /// </summary>
        public bool Update<T>(T entity) where T : class
        {
            using (var context = CreateContext())
            {
                context.Set<T>().AddOrUpdate(entity);
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Метод удаления сущности
        /// </summary>
        public bool Delete<T>(T entity) where T : class
        {
            using (var context = CreateContext())
            {
                var ent = context.Set<T>().Attach(entity);
                if (ent == null) return false;
                var entry = context.Entry(ent);
                entry.State = EntityState.Deleted;
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Метод добавления сущности в БД
        /// </summary>
        public T Add<T>(T entity) where T : class
        {
            using (var context = CreateContext())
            {
                var addedEntity = context.Set<T>().Add(entity);
                context.SaveChanges();
                return addedEntity;
            }
        }

        #endregion
    }
}
