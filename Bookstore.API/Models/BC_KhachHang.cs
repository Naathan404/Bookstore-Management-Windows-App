using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class BC_KhachHang : IEntity<int>
        //BAOCAOKHACHHANG
    {
        [Key]
        public int MaBaoCaoKhachHang { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TongDoanhThu { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal TongNo { get; set; }
        public int GetID() => MaBaoCaoKhachHang;
    }
}
