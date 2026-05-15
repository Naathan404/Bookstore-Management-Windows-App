using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class PhieuThuTien : IEntity<int>
        //PHIEUTHU
    {
        [Key]
        public int MaPhieuThuTien { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; } = string.Empty;
        public int MaKhachHang { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal SoTienThu { get; set; }
        public string LyDoThu { get; set; } = "Thu tiền cho hóa đơn còn thiếu";
        public int GetID() => MaPhieuThuTien;
        [ForeignKey("MaKhachHang")]
        public virtual KhachHang? KhachHang { get; set; }
    }
}
