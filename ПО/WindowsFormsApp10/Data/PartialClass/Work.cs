
namespace RemontKt.Data
{
  public  partial class Work
    {
        public override string ToString()
        {
            if (Work_ID==0)
            {
                return "Все виды работ";
            }
            return $"{WorkName}";
        }

       
        public override bool Equals(object obj)
        {
            if (obj is Work item)
            {
                return item.Work_ID == Work_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Work_ID;
        }
    }
}
