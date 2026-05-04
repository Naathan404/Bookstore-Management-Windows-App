using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.API.Models
{
    public class LoaiKhachHang : IEntity<int>
        // LOAIKHACHHANG
    {
        [Key]
        public int MaLoaiKhachHang { get; set; }
        public required string TenLoaiKhachHang { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal NoToiDa { get; set; }
        public double TiLeTraToiThieu { get; set; }
        public int GetID() => MaLoaiKhachHang;
    }
}
