using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
{
    public class ReceiptRequest
    {
        public string NguoiTao { get; set; } = string.Empty;
        public int MaKhachHang { get; set; }
        public decimal SoTienThu { get; set; }
        public string LyDoThu { get; set; } = "Thu tiền cho hóa đơn còn thiếu";
    }
}
