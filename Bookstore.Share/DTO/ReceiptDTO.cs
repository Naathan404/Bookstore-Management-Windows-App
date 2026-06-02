using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
{
    public class ReceiptDTO
    {
        public int MaPhieuThuTien { get; set; }
        public DateTime NgayTao { get; set; }
        public int MaKhachHang { get; set; }
        public string TenKhachHang { get; set; } = string.Empty;
        public string NguoiTao { get; set; } = string.Empty;
        public string TenNguoiTao { get; set; } = string.Empty;
        public decimal SoTienThu { get; set; }
        public string LyDoThu { get; set; } = string.Empty;
    }
}
