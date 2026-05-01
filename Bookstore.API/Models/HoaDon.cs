using System.ComponentModel.DataAnnotations;

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
        public decimal TongTienTamTinh { get; set; }
        public decimal GiamGia { get; set; }
        public decimal Thue { get; set; }
        public decimal TongTien { get; set; }
        public decimal SoTienTra { get; set; }
        public int GetID() => MaHoaDon;
    }
}
