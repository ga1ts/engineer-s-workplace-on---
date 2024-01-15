
namespace RemontKt.Data
{
  public  partial class RequestWork
    {
        public override string ToString()
        {
            return $"{Work.WorkName}";
        }

        /// <summary>
        /// Стоимость выполнения работы
        /// </summary>
        public decimal WorkPrice => Work.Price;

        /// <summary>
        /// Стоимость запчасти
        /// </summary>
        public decimal? SparePrice => Spare?.Price;

        /// <summary>
        /// Стоимость работы и если есть, запчасти
        /// </summary>
        public decimal TotalPrice => WorkPrice + (SparePrice ?? 0m);
        
        public override bool Equals(object obj)
        {
            if (obj is RequestWork item)
            {
                return item.RequestWork_ID == RequestWork_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return RequestWork_ID;
        }
    }
}
