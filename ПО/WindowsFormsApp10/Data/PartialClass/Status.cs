
namespace RemontKt.Data
{
  public  partial class Status
    {
        public override string ToString()
        {
            return $"{StatusName}";
        }

       
        public override bool Equals(object obj)
        {
            if (obj is Status item)
            {
                return item.Status_ID == Status_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Status_ID;
        }
    }
}
