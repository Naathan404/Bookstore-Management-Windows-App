using System.ComponentModel.DataAnnotations;

namespace Bookstore.API.Models
{
    public class LoaiKhachHang : IEntity
        // LOAIKHACHHANG
    {
        [Key]
        public int MaLoaiKhachHang { get; set; }
        public required string TenLoaiKhachHang { get; set; }
        public decimal NoToiDa { get; set; }
        public double TiLeTraToiThieu { get; set; }
        public int GetID() => MaLoaiKhachHang;
    }
}
