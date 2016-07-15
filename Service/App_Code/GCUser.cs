namespace User
{
    public class GCUser
    {
        public static string GetUserLogOnName()
        {
            string userName = System.Web.HttpContext.Current.User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                int index = userName.IndexOf("\\", 0);
                int length = userName.Length;
                if (index > 0)
                    index++;
                return userName.Substring(index, length - index).ToLower();
            }
            else
            {
                return userName;
            }
        }
    }
}