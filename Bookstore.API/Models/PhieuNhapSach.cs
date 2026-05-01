using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class PhieuNhapSach : IEntity<int>
        // PHIEUNHAPSACH
    {
        [Key]
        public int MaPhieuNhapSach { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiTao { get; set; }
        public int MaNhaCungCap { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TongTien { get; set; }
        public int GetID() => MaPhieuNhapSach;
    }
}
