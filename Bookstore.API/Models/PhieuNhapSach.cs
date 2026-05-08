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
        public string NguoiTao { get; set; } = string.Empty;
        public int MaNhaCungCap { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TongTien { get; set; }
        public int GetID() => MaPhieuNhapSach;

        [ForeignKey("MaNhaCungCap")]
        public virtual NhaCungCap? NhaCungCap { get; set; }

        [ForeignKey("NguoiTao")]
        public virtual NguoiDung? NguoiDung { get; set; }
        public virtual ICollection<CT_PhieuNhapSach> CT_PhieuNhapSach { get; set; } = new List<CT_PhieuNhapSach>();
    }
}
