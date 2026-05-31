using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUuDaiSeedData_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CTUD_Sach_Giam",
                keyColumn: "MaCT",
                keyValue: 1,
                columns: new[] { "GiamToiDa", "TiLeGiam" },
                values: new object[] { 0m, 0.0 });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2008));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2020));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2025));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2022));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2024));

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 1,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 5, 26, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2470), new DateTime(2026, 6, 30, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2471), new DateTime(2026, 5, 21, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2462) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 2,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao", "SoLuongDaDung" },
                values: new object[] { new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50 });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 3,
                columns: new[] { "CoTheSuDung", "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { false, new DateTime(2026, 5, 30, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2479), new DateTime(2026, 7, 30, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2480), new DateTime(2026, 5, 29, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2478) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 4,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 15, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2481), new DateTime(2026, 7, 15, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2482), new DateTime(2026, 5, 31, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2480) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 1,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 2,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao", "SoLuongDaDung" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 3,
                columns: new[] { "CoTheSuDung", "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 4,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
