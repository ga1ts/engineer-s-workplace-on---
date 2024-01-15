
namespace RemontKt.Data
{
  public  partial class Post
    {
        public override string ToString()
        {
            return $"{PostName}";
        }

       
        public override bool Equals(object obj)
        {
            if (obj is Post item)
            {
                return item.Post_ID == Post_ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Post_ID;
        }
    }
}
