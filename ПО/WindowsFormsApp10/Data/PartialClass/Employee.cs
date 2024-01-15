
namespace RemontKt.Data
{
  public  partial class Employee
    {
        public override string ToString()
        {
            if (Employee_ID==0)
            {
                // используется при поиске данных
                return "Все сотрудники";
            }
            return $"{FName} {EmpName[0]}. {LName[0]}. ({Post})";
        }

       
        public override bool Equals(object obj)
        {
            if (obj is Employee item)
            {
                return item.Employee_ID == Employee_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Employee_ID;
        }
    }
}
