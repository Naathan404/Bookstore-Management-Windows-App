using Microsoft.AspNetCore.Http.HttpResults;
using System;

namespace Bookstore.Share.DTO
{
    public class SupplierDTO
    {
        public int MaNhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; } = "";
        public string SoDienThoai { get; set; } = "";
        public string Email { get; set; } = "";
        public string MaSoThue { get; set; } = "";
        public string SoTaiKhoan { get; set; } = "";
        public string TenNganHang { get; set; } = "";
        public string DiaChi { get; set; } = "";
        public string NguoiDaiDien { get; set; } = "";
        public bool ConHoatDong { get; set; } = true;

        public int STT { get; set; } = 1;
        public string DisplayID => "NCC" + MaNhaCungCap.ToString("D3");
    }
}