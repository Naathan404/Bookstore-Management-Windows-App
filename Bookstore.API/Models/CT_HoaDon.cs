using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class CT_HoaDon
        // CT_HOADON
    {
        [Key]
        public int MaCT_HoaDon { get; set; }
        public int MaHoaDon { get; set; }
        public required string ISBN { get; set; }
        public int SoLuong { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal GiaBan { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal GiaNiemYet { get; set; }

        [ForeignKey("MaHoaDon")]
        public virtual HoaDon? HoaDon { get; set; }
        [ForeignKey("ISBN")]
        public virtual PhienBanSach? PhienBanSach { get; set; }
    }
}
