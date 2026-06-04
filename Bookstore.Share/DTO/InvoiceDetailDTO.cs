using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
{
    public class InvoiceDetailRequest
    {
        required public string ISBN { get; set; } = string.Empty;
        required public int SoLuong { get; set; }
        required public decimal GiaBan { get; set; }
    }

    public class InvoiceDetailResponse
    {
        public string ISBN { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public string HinhAnh { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; } // Giá bán thực tế lúc mua
        public decimal GiaNiemYet { get; set; } // Giá gốc để gạch ngang
        public bool IsGift => GiaBan == 0; // Tự động xác định là hàng tặng nếu giá bằng 0
        public bool HasPromotion => GiaBan > 0 && GiaBan < GiaNiemYet;
        public bool IsNormalPrice => !IsGift && !HasPromotion;
        public decimal ThanhTien => SoLuong * GiaBan;
    }
}
