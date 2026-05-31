using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTOResponses
{
    public class ReceiptResponse
    {
        public int MaPhieuThuTien { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; } = string.Empty;
        public string TenNguoiTao { get; set; } = string.Empty;
        public int MaKhachHang { get; set; }
        public string TenKhachHang { get; set; } = string.Empty;
        public decimal SoTienThu { get; set; }
        public string LyDoThu { get; set; } = string.Empty;
    }
}
