using System;

namespace Bookstore.Share.DTOs
{
    public class SachDTO
    {
        public int Id { get; set; }         
        public string ISBN { get; set; }    

        public string TenSach { get; set; }
        public string TacGia { get; set; }  
        public string TheLoai { get; set; }
        public string MoTa { get; set; }

        public int SoLuongTonKho { get; set; }
        public int TongDaBan { get; set; }
        public decimal GiaNiemYet { get; set; }
        public decimal DonGiaBan { get; set; }
        public string HinhAnh { get; set; }
    }
}