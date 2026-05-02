using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class CT_HoaDon
        // CT_HOADON
    {
        public int MaHoaDon { get; set; }
        public required string ISBN { get; set; }
        public int SoLuong { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal DonGia { get; set; }
    }
}
