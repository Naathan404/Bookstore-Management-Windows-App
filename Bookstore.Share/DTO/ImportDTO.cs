using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.Share.DTOResponses
{
    public class ImportOrderResponse
    {
        public int MaPhieuNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public int MaNhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; } = string.Empty;
        public string TenNguoiTao { get; set; } = string.Empty;
        public decimal TongTien { get; set; }
        public string GhiChu { get; set; }

        public string DisplayID => $"PN{NgayNhap:ddMMyy}{MaPhieuNhap:D3}";
    }

    public class ImportOrderDetailResponse : ImportOrderResponse
    {
        public List<ImportOrderDetailItem> ChiTietSach { get; set; } = new();
    }

    public class ImportOrderDetailItem
    {
        public int STT { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public string TacGia { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }

    public class ImportOrderRequest
    {
        [Required]
        public int MaNhaCungCap { get; set; }
        [Required]
        public string NguoiTao { get; set; } = string.Empty;
        public string GhiChu { get; set; } = string.Empty;
        public List<ImportOrderItemRequest> ChiTiet { get; set; } = new();
    }

    public class ImportOrderItemRequest
    {
        public string ISBN { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }

    public class BookSearchResponse
    {
        public string ISBN { get; set; } = string.Empty;
        public string TenSach { get; set; } = string.Empty;
        public string TacGia { get; set; } = string.Empty;
        public decimal GiaNiemYet { get; set; }
        public int TonKho { get; set; }
    }
}