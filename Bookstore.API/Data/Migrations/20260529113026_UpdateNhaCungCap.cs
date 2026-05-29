using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNhaCungCap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                value: new DateTime(2026, 5, 29, 18, 30, 25, 697, DateTimeKind.Local).AddTicks(6364));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 30, 25, 697, DateTimeKind.Local).AddTicks(6376));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 30, 25, 697, DateTimeKind.Local).AddTicks(6381));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 30, 25, 697, DateTimeKind.Local).AddTicks(6378));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 30, 25, 697, DateTimeKind.Local).AddTicks(6379));

            migrationBuilder.InsertData(
                table: "NhaCungCap",
                columns: new[] { "MaNhaCungCap", "ConGiaoGich", "DiaChi", "Email", "MaSoThue", "NganHang", "NguoiDaiDien", "SoDienThoai", "SoTaiKhoan", "TenNhaCungCap" },
                values: new object[] { 3, true, "36A Alexander, Quận 3, TP.HCM", "contact@justbooks.com", "0302221115", "Techcombank", "Nguyễn Khả An", "19005551234", "1901234567777", "Nhà sách JustBooks" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                value: new DateTime(2026, 5, 29, 18, 28, 38, 609, DateTimeKind.Local).AddTicks(513));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 28, 38, 609, DateTimeKind.Local).AddTicks(528));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 28, 38, 609, DateTimeKind.Local).AddTicks(532));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 28, 38, 609, DateTimeKind.Local).AddTicks(529));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 18, 28, 38, 609, DateTimeKind.Local).AddTicks(531));

            migrationBuilder.InsertData(
                table: "NhaCungCap",
                columns: new[] { "MaNhaCungCap", "ConGiaoGich", "DiaChi", "Email", "MaSoThue", "NganHang", "NguoiDaiDien", "SoDienThoai", "SoTaiKhoan", "TenNhaCungCap" },
                values: new object[] { 5, true, "36A Alexander, Quận 3, TP.HCM", "contact@justbooks.com", "0302221115", "Techcombank", "Nguyễn Khả An", "19005551234", "1901234567777", "Nhà sách JustBooks" });
        }
    }
}
