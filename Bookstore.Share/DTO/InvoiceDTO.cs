using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTO
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

    public class InvoiceResponse
    {
        public int MaHoaDon { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; } = string.Empty;
        public string TenNguoiTao { get; set; } = string.Empty;
        public int MaKhachHang { get; set; }
        public string TenKhachHang { get; set; } = string.Empty;
        public decimal TongTienTamTinh { get; set; }
        public decimal GiamGia { get; set; }
        public decimal Thue { get; set; }
        public decimal TongTien { get; set; }
        public decimal SoTienTra { get; set; }
        public decimal ConLai => TongTien - SoTienTra;
    }


}
