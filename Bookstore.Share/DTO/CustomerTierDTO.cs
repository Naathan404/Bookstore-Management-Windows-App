using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
{
    public class CustomerTierRequest
    {
        public string TenLoaiKhachHang { get; set; } = string.Empty;
        public decimal NoToiDa { get; set; }
        public double TiLeTraToiThieu { get; set; }
    }

    public class CustomerTierResponse
    {
        public int MaLoaiKhachHang { get; set; }
        public string TenLoaiKhachHang { get; set; } = string.Empty;
        public decimal NoToiDa { get; set; }
        public double TiLeTraToiThieu { get; set; }
    }
}
