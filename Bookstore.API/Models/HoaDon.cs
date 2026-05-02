using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class HoaDon : IEntity<int>
        // HOADON
    {
        [Key]
        public int MaHoaDon { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiTao { get; set; }
        public int MaKhachHang { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TongTienTamTinh { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal GiamGia { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal Thue { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TongTien { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal SoTienTra { get; set; }
        public int GetID() => MaHoaDon;
    }
}
