using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGiaVonCT_HoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GiaVon",
                table: "CT_HoaDon",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "CT_HoaDon",
                keyColumns: new[] { "ISBN", "MaHoaDon" },
                keyValues: new object[] { "978-604-1-09887-1", 1 },
                column: "GiaVon",
                value: 100000m);

            migrationBuilder.UpdateData(
                table: "CT_HoaDon",
                keyColumns: new[] { "ISBN", "MaHoaDon" },
                keyValues: new object[] { "978-0132350884", 2 },
                column: "GiaVon",
                value: 440000m);

            migrationBuilder.UpdateData(
                table: "CT_HoaDon",
                keyColumns: new[] { "ISBN", "MaHoaDon" },
                keyValues: new object[] { "978-604-2-11111-1", 3 },
                column: "GiaVon",
                value: 18000m);

            migrationBuilder.UpdateData(
                table: "CT_HoaDon",
                keyColumns: new[] { "ISBN", "MaHoaDon" },
                keyValues: new object[] { "978-604-56-7890-1", 3 },
                column: "GiaVon",
                value: 140000m);

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 22, 54, 15, 395, DateTimeKind.Local).AddTicks(8492));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 22, 54, 15, 395, DateTimeKind.Local).AddTicks(8505));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 22, 54, 15, 395, DateTimeKind.Local).AddTicks(8511));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 22, 54, 15, 395, DateTimeKind.Local).AddTicks(8507));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 22, 54, 15, 395, DateTimeKind.Local).AddTicks(8509));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiaVon",
                table: "CT_HoaDon");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 22, 16, 39, 99, DateTimeKind.Local).AddTicks(2486));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 22, 16, 39, 99, DateTimeKind.Local).AddTicks(2507));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 22, 16, 39, 99, DateTimeKind.Local).AddTicks(2513));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 22, 16, 39, 99, DateTimeKind.Local).AddTicks(2509));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 22, 16, 39, 99, DateTimeKind.Local).AddTicks(2511));
        }
    }
}
