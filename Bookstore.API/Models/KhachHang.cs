using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class KhachHang : IEntity
        // KHACHHANG
    {
        [Key]
        public int MaKhachHang { get; set; }
        public int MaLoaiKhachHang { get; set; }
        public DateTime NgayTao { get; set; }
        public string TenKhachHang { get; set; } = string.Empty;
        public int GioiTinh { get; set; } // 0: Nam, 1: Nu
        public DateOnly NgaySinh { get; set;} = new DateOnly();
        public string MaSoThue { get; set; } = string.Empty;
        public string DiaChi { get; set; }  = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal TongTienDaMua { get; set; }
        public int TongDonDaMua { get; set; }
        public decimal TienNo { get; set; }
        public int GetID() => MaKhachHang;
    }
}
