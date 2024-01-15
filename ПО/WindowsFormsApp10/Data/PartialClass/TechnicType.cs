
namespace RemontKt.Data
{
  public  partial class TechnicType
    {
        public override string ToString()
        {
            if (TechnicType_ID==0)
            {
                return "Все типы техники";
            }
            return $"{TechnicTypeName}";
        }

       
        public override bool Equals(object obj)
        {
            if (obj is TechnicType item)
            {
                return item.TechnicType_ID == TechnicType_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return TechnicType_ID;
        }
    }
}
