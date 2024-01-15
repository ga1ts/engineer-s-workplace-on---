using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using RemontKt.Data;
using Word = Microsoft.Office.Interop.Word;

namespace RemontKt.Export
{
    public class ExportToWord : IDisposable
    {
        private readonly Word.Document _document;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ExportToWord()
        {
            var application = new Word.Application();
            _document = application.Documents.Add();
        }

        #region Приватные методы -------------------------------------------------------------------------

        /// <summary>
        /// Метод вызова диалога для указания пути сохранения файла
        /// </summary>
        private string ShowSaveDialog(string fileName)
        {
            var saveFile = new SaveFileDialog
            {
                Filter = "MS Word files (*.docx)|*.docx",
                FileName = fileName
            };
            return saveFile.ShowDialog() == DialogResult.OK
                ? saveFile.FileName
                : string.Empty;
        }

        /// <summary>
        /// Отображение диалогового окна с вопросом на открытие документа
        /// </summary>
        private bool ShowDocumentQuestion()
        {
            var result = MessageBox.Show("Файл успешно создан. Открыть его для просмотра?",
                "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        /// <summary>
        /// Внутренний метод построения документа по заявке (квитанция)
        /// </summary>
        private void CreateRequestTicketInternal(Request request, Word.Document document)
        {
            // ---------------------------------заголовок документа-------------------------------------
            var rng = document.Range(0, 0);
            rng.Text = $"Квитанция №{request.Request_ID} от {request.RequestDt.Date:d}";
            rng.Font.Name = "TimesNewRoman";
            rng.Font.Size = 16;
            rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            rng.Font.Bold = 1;
            rng.InsertParagraphAfter();
            rng.SetRange(rng.End, rng.End);
            // ---------------------------------данные клиента-------------------------------------------
            rng.InsertParagraphAfter();
            rng.InsertAfter($"Клиент: {request.Client} {request.Client.Phone} {request.Client.Email} {request.Client.Passport}");
            rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

            rng.InsertParagraphAfter();
            rng.InsertAfter($"Вид неисправности: {request.FaultType}");
            rng.InsertParagraphAfter();
            rng.InsertAfter($"Техника: {request.Technic} ");
            rng.InsertParagraphAfter();

            // агрегация коллекции в строку с помощью LINQ
            var str = request.Technic.TechnicParameters
                .Aggregate("Параметры техники: "
                    , (current, technicParameter) =>
                        current + $"{technicParameter.Parameter.ParameterName.ToLower()} - {technicParameter.ParameterValue}, ");
            // удаляем последний пробел и запятую, вставляем строку
            rng.InsertAfter(str.Remove(str.Length - 2));

            rng.InsertParagraphAfter();
            rng.InsertAfter("Выполненные работы");
            rng.SetRange(rng.End, rng.End);
            rng.InsertParagraphAfter();

            // добавляем таблицу в документ
            var table = _document.Tables.Add(rng, 1, 5);
            //table.Columns[1].SetWidth(80, Word.WdRulerStyle.wdAdjustFirstColumn);
            table.Range.Font.Size = 11;
            table.Borders.Enable = 1;
            table.Rows[1].Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            // --------- заполняем шапку таблицы

            table.Cell(1, 1).Range.Text = "Выполненная работа";
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //table.Cell(1, 1).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Cell(1, 2).Range.Text = "Стоимость";
            table.Cell(1, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 3).Range.Text = "Запчасть";
            table.Cell(1, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 4).Range.Text = "Стоимость";
            table.Cell(1, 4).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 5).Range.Text = "Итого";
            table.Cell(1, 5).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            // заполняем данные таблицы
            int n = 1;
            foreach (var requestWork in request.RequestWorks)
            {
                n++;
                table.Rows.Add();
                table.Cell(n, 1).Range.Text = requestWork.Work.WorkName;
                table.Cell(n, 2).Range.Text = requestWork.WorkPrice.ToString("c2");
                table.Cell(n, 3).Range.Text = requestWork.Spare != null ? requestWork.Spare.ToString() : "-";
                table.Cell(n, 4).Range.Text = requestWork.SparePrice != null ? requestWork.SparePrice.Value.ToString("c2") : "-";
                table.Cell(n, 5).Range.Text = requestWork.TotalPrice.ToString("c2");
            }

            table.Rows[1].Range.Font.Bold = 1; // делаем шрифт жирным для шапки

            rng.SetRange(table.Range.End, table.Range.End);
            rng.InsertParagraphAfter();
            rng.InsertAfter($"Итоговая стоимость: {request.CostOfRepair:c2}");

            rng.InsertParagraphAfter();
            rng.InsertAfter($"Сотрудник: {request.Employee}");


            // делаем ширину столбцов таблицы по содержимому
            table.Columns.AutoFit();
        }

        /// <summary>
        /// Внутренний метод построения докумена со списком выполненных работ
        /// </summary>
        private void CreateRequestsWorksReportInternal(List<RequestWork> requestWorks, DateTime? dateFrom, DateTime? dateTo, string fileName)
        {
            // делаем альбомную ориентацию страницы
            _document.Range().PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
            // ---------------------------------заголовок документа-------------------------------------
            var rng = _document.Range(0, 0);
            rng.Text = fileName;
            rng.Font.Name = "TimesNewRoman";
            rng.Font.Size = 16;
            rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            rng.Font.Bold = 1;
            rng.InsertParagraphAfter();
            rng.InsertParagraphAfter();
            rng.SetRange(rng.End, rng.End);

            // ---------------------------------данные c коллекции-------------------------------------------
            // добавляем таблицу в документ
            var table = _document.Tables.Add(rng, 1, 8);
            table.Range.Font.Size = 11;
            table.Borders.Enable = 1;
            table.Rows[1].Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            // --------- заполняем шапку таблицы
            table.Cell(1, 1).Range.Text = "№ заявки";
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //table.Cell(1, 1).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Cell(1, 2).Range.Text = "Дата";
            table.Cell(1, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 3).Range.Text = "Выполненная работа";
            table.Cell(1, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 4).Range.Text = "Стоимость";
            table.Cell(1, 4).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 5).Range.Text = "Сотрудник";
            table.Cell(1, 5).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 6).Range.Text = "Запчасть";
            table.Cell(1, 6).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 7).Range.Text = "Стоимость";
            table.Cell(1, 7).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 8).Range.Text = "Итого";
            table.Cell(1, 8).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            // заполняем данные таблицы
            int n = 1;

            foreach (var requestWork in requestWorks)
            {
                n++;
                table.Rows.Add();
                table.Cell(n, 1).Range.Text = requestWork.Request_ID.ToString(); //№ заявки
                table.Cell(n, 2).Range.Text = requestWork.RequestWorkDt?.ToString("d"); // дата
                table.Cell(n, 3).Range.Text = requestWork.Work.WorkName; // название работы
                table.Cell(n, 4).Range.Text = requestWork.WorkPrice.ToString("c2"); // стоимость работы
                table.Cell(n, 5).Range.Text = requestWork.Employee.ToString(); // сотрудник
                table.Cell(n, 6).Range.Text = requestWork.Spare != null ? requestWork.Spare.ToString() : "-"; // запчасть
                table.Cell(n, 7).Range.Text = requestWork.SparePrice != null ? requestWork.SparePrice.Value.ToString("c2") : "-";
                table.Cell(n, 8).Range.Text = requestWork.TotalPrice.ToString("c2");
            }
            // делаем ширину столбцов таблицы по содержимому
            table.Columns.AutoFit();
            table.Rows[1].Range.Font.Bold = 1; // делаем шрифт жирным для шапки

            rng.SetRange(table.Range.End, table.Range.End);
            rng.InsertParagraphAfter();
            rng.InsertAfter($"Итоговая стоимость: {requestWorks.Sum(work => work.TotalPrice):c2}");
            rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            rng.Font.Size = 14;
        }

        /// <summary>
        /// Внутренний метод построения докумена со списком заявок
        /// </summary>
        private void CreateRequestsReportInternal(List<Request> requests, DateTime? dateFrom, DateTime? dateTo, string fileName)
        {
            // делаем альбомную ориентацию страницы
            _document.Range().PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
            // ---------------------------------заголовок документа-------------------------------------
            var rng = _document.Range(0, 0);
            rng.Text = fileName;
            rng.Font.Name = "TimesNewRoman";
            rng.Font.Size = 16;
            rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            rng.Font.Bold = 1;
            rng.InsertParagraphAfter();
            rng.InsertParagraphAfter();
            rng.SetRange(rng.End, rng.End);

            // ---------------------------------данные c коллекции-------------------------------------------
            // добавляем таблицу в документ
            var table = _document.Tables.Add(rng, 1, 9);
            table.Range.Font.Size = 11;
            table.Borders.Enable = 1;
            table.Rows[1].Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            // --------- заполняем шапку таблицы
            table.Cell(1, 1).Range.Text = "№ заявки";
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 2).Range.Text = "Клиент";
            table.Cell(1, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 3).Range.Text = "Дата создания";
            table.Cell(1, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 4).Range.Text = "Дата выполнения";
            table.Cell(1, 4).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 5).Range.Text = "Сотрудник";
            table.Cell(1, 5).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 6).Range.Text = "Вид неисправности";
            table.Cell(1, 6).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 7).Range.Text = "Техника";
            table.Cell(1, 7).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 8).Range.Text = "Статус";
            table.Cell(1, 8).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 9).Range.Text = "Стоимость";
            table.Cell(1, 9).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            // заполняем данные таблицы
            int n = 1;

            foreach (var request in requests)
            {
                n++;
                table.Rows.Add();
                table.Cell(n, 1).Range.Text = request.Request_ID.ToString(); //№ заявки
                table.Cell(n, 2).Range.Text = request.Client.ToString(); //клиент
                table.Cell(n, 3).Range.Text = request.RequestDt.ToString("d"); // дата создания
                table.Cell(n, 4).Range.Text = request.ExecutionDt.ToString("d");// дата выполнения
                table.Cell(n, 5).Range.Text = request.Employee.ToString(); // сотрудник
                table.Cell(n, 6).Range.Text = request.FaultType.ToString(); // сотрудник
                table.Cell(n, 7).Range.Text = request.Technic.ToString();
                table.Cell(n, 8).Range.Text = request.Status.ToString();
                table.Cell(n, 9).Range.Text = request.CostOfRepair?.ToString("c2");
            }
            // делаем ширину столбцов таблицы по содержимому
            table.Columns.AutoFit();
            table.Rows[1].Range.Font.Bold = 1; // делаем шрифт жирным для шапки

            rng.SetRange(table.Range.End, table.Range.End);
            rng.InsertParagraphAfter();
            rng.InsertAfter($"Итоговая стоимость: {requests.Sum(request => request.CostOfRepair):c2}");
            rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            rng.Font.Size = 14;
        }

        /// <summary>
        /// Внутренний метод построения отчёта по расходу запчастей
        /// </summary>
        private void CreateSpareReportInternal(List<RequestWork> requestWorks, DateTime? dateFrom, DateTime? dateTo, string fileName)
        {
            // делаем альбомную ориентацию страницы
            //_document.Range().PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
            // ---------------------------------заголовок документа-------------------------------------
            var rng = _document.Range(0, 0);
            rng.Text = fileName;
            rng.Font.Name = "TimesNewRoman";
            rng.Font.Size = 16;
            rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            rng.Font.Bold = 1;
            rng.InsertParagraphAfter();
            rng.InsertParagraphAfter();
            rng.SetRange(rng.End, rng.End);

            // ---------------------------------данные c коллекции-------------------------------------------
            // добавляем таблицу в документ
            var table = _document.Tables.Add(rng, 1, 5);
            table.Range.Font.Size = 11;
            table.Borders.Enable = 1;
            table.Rows[1].Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            // --------- заполняем шапку таблицы
            table.Cell(1, 1).Range.Text = "Дата";
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 2).Range.Text = "№ заявки";
            table.Cell(1, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 3).Range.Text = "Сотрудник";
            table.Cell(1, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 4).Range.Text = "Запчасть";
            table.Cell(1, 4).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Cell(1, 5).Range.Text = "Стоимость";
            table.Cell(1, 5).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            // заполняем данные таблицы
            int n = 1;

            foreach (var requestWork in requestWorks)
            {
                n++;
                table.Rows.Add();
                table.Cell(n, 1).Range.Text = requestWork.RequestWorkDt?.ToString("d"); // дата
                table.Cell(n, 2).Range.Text = requestWork.Request_ID.ToString(); //№ заявки
                table.Cell(n, 3).Range.Text = requestWork.Employee.ToString(); // сотрудник
                table.Cell(n, 4).Range.Text = requestWork.Spare != null ? requestWork.Spare.ToString() : "-"; // запчасть
                table.Cell(n, 5).Range.Text = requestWork.SparePrice != null ? requestWork.SparePrice.Value.ToString("c2") : "-";
            }
            // делаем ширину столбцов таблицы по содержимому
            table.Columns.AutoFit();
            table.Rows[1].Range.Font.Bold = 1; // делаем шрифт жирным для шапки

            rng.SetRange(table.Range.End, table.Range.End);
            rng.InsertParagraphAfter();
            rng.InsertAfter($"Итоговая стоимость: {requestWorks.Sum(work => work.SparePrice):c2}");
            rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            rng.Font.Size = 14;
        }

        /// <summary>
        /// Внутренний метод построения статистики заявок по типам неисправностей
        /// </summary>
        private void CreateRequestsByFaultTypeReportInternal(List<RequestByFaultType> requests, DateTime? dateFrom, DateTime? dateTo, string fileName)
        {
            // ---------------------------------заголовок документа-------------------------------------
            var rng = _document.Range(0, 0);
            rng.Text = fileName;
            rng.Font.Name = "TimesNewRoman";
            rng.Font.Size = 16;
            rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            rng.Font.Bold = 1;
            rng.InsertParagraphAfter();
            rng.InsertParagraphAfter();
            rng.SetRange(rng.End, rng.End);

            // ---------------------------------данные c коллекции-------------------------------------------
            // добавляем таблицу в документ
            var table = _document.Tables.Add(rng, 1, 2);
            table.Range.Font.Size = 14;
            table.Borders.Enable = 1;
            table.Rows[1].Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            
            // --------- заполняем шапку таблицы
            table.Cell(1, 1).Range.Text = "Вид неисправности";
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            table.Cell(1, 2).Range.Text = "Количество заявок";
            table.Cell(1, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            // заполняем данные таблицы
            int n = 1;

            foreach (var request in requests)
            {
                n++;
                table.Rows.Add();
                table.Cell(n, 1).Range.Text = request.FaultType.ToString(); //Тип неисправности
                table.Cell(n, 2).Range.Text = request.Count.ToString(); //Количество заявок
            }
            // делаем ширину столбцов таблицы по содержимому
            table.Columns.AutoFit();
            table.Rows[1].Range.Font.Bold = 1; // делаем шрифт жирным для шапки

        }

        /// <summary>
        /// Внутренний метод построения статистики заявок по типу техники
        /// </summary>
        private void CreateRequestsByTechnicTypeReportInternal(List<RequestByTechnicType> requests, DateTime? dateFrom, DateTime? dateTo, string fileName)
        {
            // ---------------------------------заголовок документа-------------------------------------
            var rng = _document.Range(0, 0);
            rng.Text = fileName;
            rng.Font.Name = "TimesNewRoman";
            rng.Font.Size = 16;
            rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            rng.Font.Bold = 1;
            rng.InsertParagraphAfter();
            rng.InsertParagraphAfter();
            rng.SetRange(rng.End, rng.End);

            // ---------------------------------данные c коллекции-------------------------------------------
            // добавляем таблицу в документ
            var table = _document.Tables.Add(rng, 1, 2);
            table.Range.Font.Size = 14;
            table.Borders.Enable = 1;
            table.Rows[1].Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            
            // --------- заполняем шапку таблицы
            table.Cell(1, 1).Range.Text = "Тип техники";
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            table.Cell(1, 2).Range.Text = "Количество заявок";
            table.Cell(1, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            // заполняем данные таблицы
            int n = 1;

            foreach (var request in requests)
            {
                n++;
                table.Rows.Add();
                table.Cell(n, 1).Range.Text = request.TechnicType.ToString(); //Тип техники
                table.Cell(n, 2).Range.Text = request.Count.ToString(); //Количество заявок
            }
            // делаем ширину столбцов таблицы по содержимому
            table.Columns.AutoFit();
            table.Rows[1].Range.Font.Bold = 1; // делаем шрифт жирным для шапки

        }

        #endregion -------------------------------------------------------------------------

        #region Публичные методы -------------------------------------------------------------------------


        /// <summary>
        /// Экспорт данных в Word для создания квитанции
        /// </summary>
        public void CreateRequestTicket(Request request)
        {
            var fileName = $"Квитанция №{request.Request_ID} от {request.RequestDt.Date:d}";

            var path = ShowSaveDialog(fileName);
            if (string.IsNullOrEmpty(path)) return;
            _document.Application.Caption = fileName;
            try
            {
                CreateRequestTicketInternal(request, _document);
                _document.SaveAs(path);
                if (ShowDocumentQuestion())
                {
                    Process.Start(path);
                }
            }
            catch (Exception e)
            {
                Dispose();
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Экспорт данных по выполненным работам
        /// </summary>
        public void CreateRequestsWorksReport(List<RequestWork> requestWorks, DateTime? dateFrom, DateTime? dateTo)
        {
            var fileName = "Реестр выполненных работ";

            if (dateFrom.HasValue && dateTo.HasValue)// если есть обе даты - ищем период
            {
                fileName = $"Реестр выполненных работ с {dateFrom:d} по {dateTo:d}";
            }
            if (dateFrom.HasValue && !dateTo.HasValue)// ищем все даты после указанной
            {
                fileName = $"Реестр выполненных работ с {dateFrom:d} ";
            }
            if (!dateFrom.HasValue && dateTo.HasValue)// ищем все даты до указанной
            {
                fileName = $"Реестр выполненных работ по {dateTo:d}";
            }
            var path = ShowSaveDialog(fileName);
            if (string.IsNullOrEmpty(path)) return;
            _document.Application.Caption = fileName;
            try
            {
                CreateRequestsWorksReportInternal(requestWorks, dateFrom, dateTo, fileName);
                _document.SaveAs(path);
                if (ShowDocumentQuestion())
                {
                    Process.Start(path);
                }
            }
            catch (Exception e)
            {
                Dispose();
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Экспорт данных по заявкам
        /// </summary>
        public void CreateRequestsReport(List<Request> requests, DateTime? dateFrom, DateTime? dateTo)
        {

            var fileName = "Отчёт по заявкам";

            if (dateFrom.HasValue && dateTo.HasValue)// если есть обе даты - ищем период
            {
                fileName = $"Отчёт по заявкам с {dateFrom:d} по {dateTo:d}";
            }
            if (dateFrom.HasValue && !dateTo.HasValue)// ищем все даты после указанной
            {
                fileName = $"Отчёт по заявкам с {dateFrom:d} ";
            }
            if (!dateFrom.HasValue && dateTo.HasValue)// ищем все даты до указанной
            {
                fileName = $"Отчёт по заявкам по {dateTo:d}";
            }
            var path = ShowSaveDialog(fileName);
            if (string.IsNullOrEmpty(path)) return;
            _document.Application.Caption = fileName;
            try
            {
                CreateRequestsReportInternal(requests, dateFrom, dateTo, fileName);
                _document.SaveAs(path);
                if (ShowDocumentQuestion())
                {
                    Process.Start(path);
                }
            }
            catch (Exception e)
            {
                Dispose();
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Экспорт данных по выполненным работам
        /// </summary>
        public void CreateSpareReport(List<RequestWork> requestWorks, DateTime? dateFrom, DateTime? dateTo)
        {
            var fileName = "Отчёт по расходу запчастей";

            if (dateFrom.HasValue && dateTo.HasValue)// если есть обе даты - ищем период
            {
                fileName = $"Отчёт по расходу запчастей с {dateFrom:d} по {dateTo:d}";
            }
            if (dateFrom.HasValue && !dateTo.HasValue)// ищем все даты после указанной
            {
                fileName = $"Отчёт по расходу запчастей с {dateFrom:d} ";
            }
            if (!dateFrom.HasValue && dateTo.HasValue)// ищем все даты до указанной
            {
                fileName = $"Отчёт по расходу запчастей по {dateTo:d}";
            }
            var path = ShowSaveDialog(fileName);
            if (string.IsNullOrEmpty(path)) return;
            _document.Application.Caption = fileName;
            try
            {
                CreateSpareReportInternal(requestWorks, dateFrom, dateTo, fileName);
                _document.SaveAs(path);
                if (ShowDocumentQuestion())
                {
                    Process.Start(path);
                }
            }
            catch (Exception e)
            {
                Dispose();
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Экспорт данных по заявкам сгруппированных по типу несиправности
        /// </summary>
        public void CreateRequestsByFaultTypesReport(List<RequestByFaultType> requests, DateTime? dateFrom, DateTime? dateTo)
        {

            var fileName = "Статистика заявок по видам неисправностей";

            if (dateFrom.HasValue && dateTo.HasValue)// если есть обе даты - ищем период
            {
                fileName = $"Статистика заявок по видам неисправностей с {dateFrom:d} по {dateTo:d}";
            }
            if (dateFrom.HasValue && !dateTo.HasValue)// ищем все даты после указанной
            {
                fileName = $"Статистика заявок по видам неисправностей с {dateFrom:d} ";
            }
            if (!dateFrom.HasValue && dateTo.HasValue)// ищем все даты до указанной
            {
                fileName = $"Статистика заявок по видам неисправностей по {dateTo:d}";
            }
            var path = ShowSaveDialog(fileName);
            if (string.IsNullOrEmpty(path)) return;
            _document.Application.Caption = fileName;
            try
            {
                CreateRequestsByFaultTypeReportInternal(requests, dateFrom, dateTo, fileName);
                _document.SaveAs(path);
                if (ShowDocumentQuestion())
                {
                    Process.Start(path);
                }
            }
            catch (Exception e)
            {
                Dispose();
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Экспорт данных по заявкам сгруппированных по типу техники
        /// </summary>
        public void CreateRequestsByTechnikTypesReport(List<RequestByTechnicType> requests, DateTime? dateFrom, DateTime? dateTo)
        {

            var fileName = "Статистика заявок по типу техники";

            if (dateFrom.HasValue && dateTo.HasValue)// если есть обе даты - ищем период
            {
                fileName = $"Статистика заявок по типу техники с {dateFrom:d} по {dateTo:d}";
            }
            if (dateFrom.HasValue && !dateTo.HasValue)// ищем все даты после указанной
            {
                fileName = $"Статистика заявок по типу техники с {dateFrom:d} ";
            }
            if (!dateFrom.HasValue && dateTo.HasValue)// ищем все даты до указанной
            {
                fileName = $"Статистика заявок по типу техники по {dateTo:d}";
            }
            var path = ShowSaveDialog(fileName);
            if (string.IsNullOrEmpty(path)) return;
            _document.Application.Caption = fileName;
            try
            {
                CreateRequestsByTechnicTypeReportInternal(requests, dateFrom, dateTo, fileName);
                _document.SaveAs(path);
                if (ShowDocumentQuestion())
                {
                    Process.Start(path);
                }
            }
            catch (Exception e)
            {
                Dispose();
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion -------------------------------------------------------------------------

        /// <summary>
        /// Реализация интерфейса IDisposable
        /// </summary>
        public void Dispose()
        {
            // при реализации IDisposable можно использовать
            // using (var export=new ExportToWord()) для автоматического осовобождения ресурсов
            // - закрытие Word, чтобы не висел в процессах
            var app = _document.Application;
            _document.Close();
            app.Quit();
        }
    }
}
