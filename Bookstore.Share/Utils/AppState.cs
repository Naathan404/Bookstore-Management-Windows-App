using Bookstore.Share.DTO;

namespace Bookstore.WPF.Utils
{
    public static class AppState
    {
        public static UserInfo CurrentUser { get; set; }

        public static List<string> CurrentPermissions { get; set; } = new List<string>();
        public static bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

        
        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}