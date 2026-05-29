using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNCC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConGiaoGich",
                table: "NhaCungCap",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NguoiDaiDien",
                table: "NhaCungCap",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.UpdateData(
                table: "NhaCungCap",
                keyColumn: "MaNhaCungCap",
                keyValue: 1,
                columns: new[] { "ConGiaoGich", "NguoiDaiDien" },
                values: new object[] { true, "Lê Thành Nghĩa" });

            migrationBuilder.UpdateData(
                table: "NhaCungCap",
                keyColumn: "MaNhaCungCap",
                keyValue: 2,
                columns: new[] { "ConGiaoGich", "NguoiDaiDien" },
                values: new object[] { true, "Tô Công Hữu Nhân" });

            migrationBuilder.InsertData(
                table: "NhaCungCap",
                columns: new[] { "MaNhaCungCap", "ConGiaoGich", "DiaChi", "Email", "MaSoThue", "NganHang", "NguoiDaiDien", "SoDienThoai", "SoTaiKhoan", "TenNhaCungCap" },
                values: new object[] { 3, true, "36A Alexander, Quận 3, TP.HCM", "contact@justbooks.com", "0302221115", "Techcombank", "Tô Công Hữu Nhân", "19005551234", "1901234567777", "Nhà sách JustBooks" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NhaCungCap",
                keyColumn: "MaNhaCungCap",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "ConGiaoGich",
                table: "NhaCungCap");

            migrationBuilder.DropColumn(
                name: "NguoiDaiDien",
                table: "NhaCungCap");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 24, 15, 27, 59, 102, DateTimeKind.Local).AddTicks(7307));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 24, 15, 27, 59, 102, DateTimeKind.Local).AddTicks(7327));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 24, 15, 27, 59, 102, DateTimeKind.Local).AddTicks(7332));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 24, 15, 27, 59, 102, DateTimeKind.Local).AddTicks(7329));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 24, 15, 27, 59, 102, DateTimeKind.Local).AddTicks(7331));
        }
    }
}
