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
        public int NguoiTao { get; set; }
        public int MaKhachHang { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal SoTienThu { get; set; }
        public int GetID() => MaPhieuThuTien;
    }
}
