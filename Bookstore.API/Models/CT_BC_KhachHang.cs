using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class CT_BC_KhachHang
        // CT_BC_KHACHHANG
    {
        public int MaBaoCaoKhachHang { get; set; }
        public int MaKhachHang { get; set; }

        public int SoHoaDon { get; set; }
        public decimal DoanhThu { get; set; }
        public float TiLeDoanhThu { get; set; }

        public decimal NoDau { get; set; } 
        public decimal NoPhatSinh { get; set; }

        public decimal DaTra { get; set; }

        public decimal NoCuoi { get; set; }
    }
}
