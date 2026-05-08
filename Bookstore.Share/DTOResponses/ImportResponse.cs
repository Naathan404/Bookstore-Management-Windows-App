using Bookstore.Share.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTOResponses
{
    public class ImportResponse
    {
        public int MaPhieu { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; } = string.Empty;
        public string TenNguoiTao { get; set; } = string.Empty;
        public string TenNhaCungCap { get; set; } = string.Empty;
        public decimal TongTien { get; set; }
    }
}
