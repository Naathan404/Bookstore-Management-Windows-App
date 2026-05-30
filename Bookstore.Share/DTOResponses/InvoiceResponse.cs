using System;

namespace Bookstore.Share.DTOResponses
{
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