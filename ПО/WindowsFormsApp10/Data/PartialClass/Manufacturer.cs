
namespace RemontKt.Data
{
  public  partial class Manufacturer
    {
        public override string ToString()
        {
            return $"{ManufacturerName}";
        }

       
        public override bool Equals(object obj)
        {
            if (obj is Manufacturer item)
            {
                return item.Manufacturer_ID == Manufacturer_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Manufacturer_ID;
        }
    }
}
