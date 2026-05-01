using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class PhieuThuTien : IEntity
        //PHIEUTHU
    {
        [Key]
        public int MaPhieuThuTien { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiTao { get; set; }
        public int MaKhachHang { get; set; }
        public decimal SoTienThu { get; set; }
        public int GetID() => MaPhieuThuTien;
    }
}
