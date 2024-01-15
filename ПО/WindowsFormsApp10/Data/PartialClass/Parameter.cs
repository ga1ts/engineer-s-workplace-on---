
namespace RemontKt.Data
{
  public  partial class Parameter
    {
        public override string ToString()
        {
            return $"{ParameterName}";
        }

       
        public override bool Equals(object obj)
        {
            if (obj is Parameter item)
            {
                return item.Parameter_ID == Parameter_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Parameter_ID;
        }
    }
}
