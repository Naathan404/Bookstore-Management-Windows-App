using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class PhieuNhapSach : IEntity
        // PHIEUNHAPSACH
    {
        [Key]
        public int MaPhieuNhapSach { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiTao { get; set; }
        public int MaNhaCungCap { get; set; }
        public decimal TongTien { get; set; }
        public int GetID() => MaPhieuNhapSach;
    }
}
