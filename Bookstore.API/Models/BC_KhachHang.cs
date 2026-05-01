using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class BC_KhachHang : IEntity<int>
        //BAOCAOKHACHHANG
    {
        [Key]
        public int MaBaoCaoKhachHang { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public decimal TongDoanhThu { get; set; }
        public decimal TongNo { get; set; }
        public int GetID() => MaBaoCaoKhachHang;
    }
}
