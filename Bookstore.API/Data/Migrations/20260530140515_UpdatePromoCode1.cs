using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePromoCode1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 21, 5, 14, 257, DateTimeKind.Local).AddTicks(3596));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 21, 5, 14, 257, DateTimeKind.Local).AddTicks(3612));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 21, 5, 14, 257, DateTimeKind.Local).AddTicks(3617));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 21, 5, 14, 257, DateTimeKind.Local).AddTicks(3614));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 21, 5, 14, 257, DateTimeKind.Local).AddTicks(3615));

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 1,
                column: "SoLuongDaDung",
                value: 136);

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 3,
                column: "SoLuongDaDung",
                value: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 20, 32, 46, 289, DateTimeKind.Local).AddTicks(3288));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 20, 32, 46, 289, DateTimeKind.Local).AddTicks(3302));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 20, 32, 46, 289, DateTimeKind.Local).AddTicks(3307));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 20, 32, 46, 289, DateTimeKind.Local).AddTicks(3304));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 20, 32, 46, 289, DateTimeKind.Local).AddTicks(3306));

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 1,
                column: "SoLuongDaDung",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 3,
                column: "SoLuongDaDung",
                value: 0);
        }
    }
}
