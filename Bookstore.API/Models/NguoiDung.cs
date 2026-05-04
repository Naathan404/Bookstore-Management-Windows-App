using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class NguoiDung : IEntity<string>
        //NGUOI DUNG
    {
        [Key]
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public int MaNhomNguoiDung { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string GioiTinh { get; set; } = string.Empty;
        public DateOnly NgaySinh { get; set; }
        public DateOnly NgayVaoLam { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string Email { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty;

        public string? MaOTP { get; set; } = string.Empty;
        public DateTime? HanOTP { get; set; } = DateTime.Now;

        public bool DangLamViec { get; set; }
        public string GetID() => TenDangNhap;
        
        public virtual NhomNguoiDung NhomNguoiDung { get; set; }
    }
}
