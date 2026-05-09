using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTOResponses
{
    public class CustomerTierResponse
    {
        public int MaLoaiKhachHang { get; set; }
        public string TenLoaiKhachHang { get; set; } = string.Empty;
    }
}
