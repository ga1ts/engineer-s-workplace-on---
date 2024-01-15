
namespace RemontKt.Data
{
  public  partial class Technic
    {
        public override string ToString()
        {
            return $"{TechnicType} {Manufacturer} {Model}";
        }
       
        public override bool Equals(object obj)
        {
            if (obj is Technic item)
            {
                return item.Technic_ID == Technic_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Technic_ID;
        }
    }
}
