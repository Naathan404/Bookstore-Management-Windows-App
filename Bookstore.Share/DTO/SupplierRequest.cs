using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
{
    public class SupplierRequest
    {
        public string TenNhaCungCap { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string MaSoThue { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NganHang { get; set; } = string.Empty;
        public string SoTaiKhoan { get; set; } = string.Empty;
    }
}
