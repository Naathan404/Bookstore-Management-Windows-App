namespace Bookstore.API.Models
{
    public class Account : IEntity
    {
        public int AccountID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Role { get; set; }

        public bool IsDeleted { get; set; }

        public int GetID() => AccountID;
    }
}
