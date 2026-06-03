using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTOResponses
{
    public class CreateInvoiceRequest
    {
        public string NguoiTao { get; set; } = string.Empty;
        public int MaKhachHang { get; set; }
        public decimal TongTienTamTinh { get; set; }
        public decimal GiamGia { get; set; }
        public decimal Thue { get; set; }
        public decimal TongTien { get; set; }
        public decimal SoTienTra { get; set; }

        public List<InvoiceDetailRequest> ChiTiet { get; set; } = new();
        public List<InvoicePromoRequest> UuDai { get; set; } = new();
    }

    public class InvoiceDetailRequest
    {
        public string ISBN { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal GiaVon { get; set; }
    }

    public class InvoicePromoRequest
    {
        public int MaUuDai { get; set; }
        public string? ISBN { get; set; } // Nếu áp dụng trên hóa đơn thì để null, trên sách thì có ISBN
        public decimal SoTienGiam { get; set; }
    }
}
