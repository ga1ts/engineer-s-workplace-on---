
using System.Linq;

namespace RemontKt.Data
{
  public  partial class Request
    {
        public override string ToString()
        {
            return $"{Request_ID}";
        }

        /// <summary>
        /// Стоимость ремонта = стоимость работ + стоимость запчастей
        /// </summary>
        public decimal? CostOfRepair => RequestWorks.Sum(requestWork => requestWork.TotalPrice);

        public override bool Equals(object obj)
        {
            if (obj is Request item)
            {
                return item.Request_ID == Request_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Request_ID;
        }
    }
}
