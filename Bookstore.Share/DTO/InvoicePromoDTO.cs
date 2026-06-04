using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
{
    public class InvoicePromoRequest
    {
        required public int MaUuDai { get; set; }
        required public decimal SoTienGiam { get; set; }
        //required public bool IsGift { get; set; }
    }

    public class InvoicePromoResponse
    {
        public int MaCT_HoaDon_UuDai { get; set; }
        public int MaHoaDon { get; set; }
        public int MaUuDai { get; set; }
        public string TenUuDai { get; set; } = string.Empty; // Tên chương trình ưu đãi
        public string Code { get; set; } = string.Empty;
        public decimal SoTienGiam { get; set; }
    }
}
