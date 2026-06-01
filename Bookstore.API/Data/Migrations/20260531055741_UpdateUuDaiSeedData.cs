using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUuDaiSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CTUD_HoaDon_Giam",
                keyColumn: "MaCT",
                keyValue: 1,
                columns: new[] { "GiamToiDa", "TiLeGiam" },
                values: new object[] { 100000m, 10.0 });

            migrationBuilder.UpdateData(
                table: "CTUD_Sach_Giam",
                keyColumn: "MaCT",
                keyValue: 1,
                columns: new[] { "GiamToiDa", "TiLeGiam" },
                values: new object[] { 50000m, 25.0 });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 12, 57, 40, 538, DateTimeKind.Local).AddTicks(7801));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 12, 57, 40, 538, DateTimeKind.Local).AddTicks(7813));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 12, 57, 40, 538, DateTimeKind.Local).AddTicks(7818));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 12, 57, 40, 538, DateTimeKind.Local).AddTicks(7815));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 12, 57, 40, 538, DateTimeKind.Local).AddTicks(7816));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CTUD_HoaDon_Giam",
                keyColumn: "MaCT",
                keyValue: 1,
                columns: new[] { "GiamToiDa", "TiLeGiam" },
                values: new object[] { 100m, 0.10000000149011612 });

            migrationBuilder.UpdateData(
                table: "CTUD_Sach_Giam",
                keyColumn: "MaCT",
                keyValue: 1,
                columns: new[] { "GiamToiDa", "TiLeGiam" },
                values: new object[] { 20000m, 0.0 });

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
        }
    }
}
