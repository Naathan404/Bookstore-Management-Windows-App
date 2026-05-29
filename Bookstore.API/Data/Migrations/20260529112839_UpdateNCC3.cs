using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNCC3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "NhaCungCap",
                keyColumn: "MaNhaCungCap",
                keyValue: 5,
                column: "NguoiDaiDien",
                value: "Nguyễn Khả An");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "NhaCungCap",
                keyColumn: "MaNhaCungCap",
                keyValue: 5,
                column: "NguoiDaiDien",
                value: "Tô Công Hữu Nhân");
        }
    }
}
