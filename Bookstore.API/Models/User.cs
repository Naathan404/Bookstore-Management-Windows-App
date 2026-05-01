namespace Bookstore.API.Models
{
    public class User : IEntity
        //NGUOI DUNG
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RoleID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public DateOnly JoinedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string Email { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int GetID() => UserID;

        public string? OTP { get; set;  } = String.Empty;
        public DateTime? OTPExpire { get; set; }
    }
}
