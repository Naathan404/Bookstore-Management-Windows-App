using System;

namespace Bookstore.Share.DTOs
{
    public class SachDTO
    {
        public int Id { get; set; }         
        public string ISBN { get; set; }    
        public string TenSach { get; set; }
        public string MoTa { get; set; }
        public string TheLoai { get; set; }
        public string HinhAnh { get; set; }

        public int SoLuongTonKho { get; set; }
        public int TongDaBan { get; set; }
        public decimal GiaNiemYet { get; set; }
        public decimal DonGiaBan { get; set; }

        public int NamXuatBan { get; set; }
        public string NhaXuatBan { get; set; }
        public int LanTaiBan { get; set; }
        public string HinhThucBia { get; set; }
        public List<TacGiaDTO> DanhSachTacGia { get; set; } = new List<TacGiaDTO>();

        public bool IsTacPhamMoi { get; set; }
        public int MaSachGoc { get; set; }
    }
}

public class TacGiaDTO
{
    public int Id { get; set; }
    public string TenTacGia { get; set; }
}