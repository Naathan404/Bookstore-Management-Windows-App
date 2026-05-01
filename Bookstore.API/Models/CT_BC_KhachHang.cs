using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class CT_BC_KhachHang
        // CT_BC_KHACHHANG
    {
        public int MaBaoCaoKhachHang { get; set; }
        public int MaKhachHang { get; set; }

        public int SoHoaDon { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal DoanhThu { get; set; }
        public float TiLeDoanhThu { get; set; }

        [Column(TypeName = "decimal(18,2)")] public decimal NoDau { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal NoPhatSinh { get; set; }

        [Column(TypeName = "decimal(18,2)")] public decimal DaTra { get; set; }

        [Column(TypeName = "decimal(18,2)")] public decimal NoCuoi { get; set; }
    }
}
