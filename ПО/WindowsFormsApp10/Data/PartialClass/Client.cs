
namespace RemontKt.Data
{
  public  partial class Client
    {
        public override string ToString()
        {
            return $"{FName} {ClientName[0]}. {LName[0]}.";
        }
       
        public override bool Equals(object obj)
        {
            if (obj is Client item)
            {
                return item.Client_ID == Client_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Client_ID;
        }
    }
}
