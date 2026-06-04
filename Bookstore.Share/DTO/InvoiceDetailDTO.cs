using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
{
    public class InvoiceDetailRequest
    {
        public string ISBN { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal GiaVon { get; set; }
    }

    public class InvoiceDetailResponse
    {
        public string ISBN { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public string HinhAnh { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; } // Giá bán thực tế lúc mua
        public decimal GiaNiemYet { get; set; } // Giá gốc để gạch ngang
        public bool IsGift => DonGia == 0; // Tự động xác định là hàng tặng nếu giá bằng 0
        public bool HasPromotion => DonGia > 0 && DonGia < GiaNiemYet;
        public bool IsNormalPrice => !IsGift && !HasPromotion;
        public decimal ThanhTien => SoLuong * DonGia;
    }
}
