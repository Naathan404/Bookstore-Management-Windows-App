using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class NhaCungCap : IEntity<int>
        // NHACUNGCAP
    {
        [Key]
        public int NhaNhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string MaSoThue { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NganHang { get; set; } = string.Empty;
        public string SoTaiKhoan { get; set; } = string.Empty;
        public int GetID() => NhaNhaCungCap;
    }
}
