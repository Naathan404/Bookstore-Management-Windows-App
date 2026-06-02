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
    }

    public class SupplierRequest
    {
        public string TenNhaCungCap { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string MaSoThue { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NganHang { get; set; } = string.Empty;
        public string SoTaiKhoan { get; set; } = string.Empty;
    }
}