
namespace RemontKt.Data
{
  public  partial class Spare
    {
        public override string ToString()
        {
            return $"{TechnicType} {SpareType} {SpareName} ({Article})";
        }

        public TechnicType TechnicType => SpareType.TechnicType;
       
        public override bool Equals(object obj)
        {
            if (obj is Spare item)
            {
                return item.Spare_ID == Spare_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Spare_ID;
        }
    }
}
