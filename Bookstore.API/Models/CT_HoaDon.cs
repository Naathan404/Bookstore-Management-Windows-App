namespace Bookstore.API.Models
{
    public class CT_HoaDon
        // CT_HOADON
    {
        public int MaHoaDon { get; set; }
        public required string ISBN { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }
}
