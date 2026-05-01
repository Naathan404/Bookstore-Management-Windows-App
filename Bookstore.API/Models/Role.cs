namespace Bookstore.API.Models
{
    public class Role : IEntity
        //NHOM NGUOI DUNG
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int GetID() => RoleID;
    }
}
