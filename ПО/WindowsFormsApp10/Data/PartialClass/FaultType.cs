
namespace RemontKt.Data
{
  public  partial class FaultType
    {
        public override string ToString()
        {
            return $"{FaultTypeName}";
        }

       
        public override bool Equals(object obj)
        {
            if (obj is FaultType item)
            {
                return item.FaultType_ID == FaultType_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return FaultType_ID;
        }
    }
}
