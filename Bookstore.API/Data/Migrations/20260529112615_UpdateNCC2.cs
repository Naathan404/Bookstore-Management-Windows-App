using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNCC2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NhaCungCap",
                keyColumn: "MaNhaCungCap",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 26, 14, 574, DateTimeKind.Local).AddTicks(1672));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 26, 14, 574, DateTimeKind.Local).AddTicks(1684));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 26, 14, 574, DateTimeKind.Local).AddTicks(1689));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 26, 14, 574, DateTimeKind.Local).AddTicks(1685));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 26, 14, 574, DateTimeKind.Local).AddTicks(1687));

            migrationBuilder.InsertData(
                table: "NhaCungCap",
                columns: new[] { "MaNhaCungCap", "ConGiaoGich", "DiaChi", "Email", "MaSoThue", "NganHang", "NguoiDaiDien", "SoDienThoai", "SoTaiKhoan", "TenNhaCungCap" },
                values: new object[] { 5, true, "36A Alexander, Quận 3, TP.HCM", "contact@justbooks.com", "0302221115", "Techcombank", "Tô Công Hữu Nhân", "19005551234", "1901234567777", "Nhà sách JustBooks" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NhaCungCap",
                keyColumn: "MaNhaCungCap",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 20, 44, 191, DateTimeKind.Local).AddTicks(6470));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 20, 44, 191, DateTimeKind.Local).AddTicks(6483));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 20, 44, 191, DateTimeKind.Local).AddTicks(6488));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 20, 44, 191, DateTimeKind.Local).AddTicks(6485));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 20, 44, 191, DateTimeKind.Local).AddTicks(6486));

            migrationBuilder.InsertData(
                table: "NhaCungCap",
                columns: new[] { "MaNhaCungCap", "ConGiaoGich", "DiaChi", "Email", "MaSoThue", "NganHang", "NguoiDaiDien", "SoDienThoai", "SoTaiKhoan", "TenNhaCungCap" },
                values: new object[] { 3, true, "36A Alexander, Quận 3, TP.HCM", "contact@justbooks.com", "0302221115", "Techcombank", "Tô Công Hữu Nhân", "19005551234", "1901234567777", "Nhà sách JustBooks" });
        }
    }
}
