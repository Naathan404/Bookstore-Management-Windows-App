using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class Sach : IEntity
        // SACH
    {
        [Key]
        public int MaSach { get; set; }
        public required string TenSach { get; set; }
        public int MaTheLoai { get; set; }
        public string MoTa { get; set; } = string.Empty;
        public int GetID() => MaSach;
    }
}
