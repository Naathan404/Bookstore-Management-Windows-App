using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
{
    public class InvoicePromoRequest
    {
        public int MaUuDai { get; set; }
        public string? ISBN { get; set; } // Nếu áp dụng trên hóa đơn thì để null, trên sách thì có ISBN
        public decimal SoTienGiam { get; set; }
    }

    public class InvoicePromoResponse
    {
        public int MaCT_HoaDon_UuDai { get; set; }
        public int MaHoaDon { get; set; }
        public int MaUuDai { get; set; }
        public string Code { get; set; } = string.Empty;
        public string TenUuDai { get; set; } = string.Empty; // Tên chương trình ưu đãi
        public string? ISBN { get; set; } // Null nếu giảm tổng bill, có trị nếu giảm riêng đầu sách
        public decimal SoTienGiam { get; set; }
    }
}
