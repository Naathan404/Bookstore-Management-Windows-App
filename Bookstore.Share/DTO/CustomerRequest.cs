using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
{
    public class CustomerRequest
    {
        public int MaLoaiKhachHang { get; set; }
        public string TenKhachHang { get; set; } = string.Empty;
        public int GioiTinh { get; set; } // 0: Nam, 1: Nu
        public DateOnly? NgaySinh { get; set;} = new DateOnly();
        public string? MaSoThue { get; set; } = string.Empty;
        public string? DiaChi { get; set; }  = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
    }
    /*
     *         public int MaKhachHang { get; set; }
        public int MaLoaiKhachHang { get; set; }
        public DateTime NgayTao { get; set; }
        public string TenKhachHang { get; set; } = string.Empty;
        public int GioiTinh { get; set; } // 0: Nam, 1: Nu
        public DateOnly NgaySinh { get; set;} = new DateOnly();
        public string MaSoThue { get; set; } = string.Empty;
        public string DiaChi { get; set; }  = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")] public decimal TongTienDaMua { get; set; }
        public int TongDonDaMua { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TienNo { get; set; }
        public int GetID() => MaKhachHang;

        [ForeignKey("MaLoaiKhachHang")]
        public virtual LoaiKhachHang? LoaiKhachHang { get; set; }
    */
}
