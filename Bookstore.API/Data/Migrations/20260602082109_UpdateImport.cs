using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "PhieuNhapSach",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(1575));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(1601));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(1606));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(1603));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(1604));

            migrationBuilder.UpdateData(
                table: "PhieuNhapSach",
                keyColumn: "MaPhieuNhapSach",
                keyValue: 1,
                column: "GhiChu",
                value: "");

            migrationBuilder.UpdateData(
                table: "PhieuNhapSach",
                keyColumn: "MaPhieuNhapSach",
                keyValue: 2,
                column: "GhiChu",
                value: "");

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 1,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 5, 28, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2370), new DateTime(2026, 7, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2372), new DateTime(2026, 5, 23, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2348) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 3,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 1, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2386), new DateTime(2026, 8, 1, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2388), new DateTime(2026, 5, 31, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2383) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 4,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 17, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2391), new DateTime(2026, 7, 17, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2392), new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2389) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "PhieuNhapSach");

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
                keyValue: 3,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 5, 30, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2479), new DateTime(2026, 7, 30, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2480), new DateTime(2026, 5, 29, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2478) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 4,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 15, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2481), new DateTime(2026, 7, 15, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2482), new DateTime(2026, 5, 31, 13, 7, 48, 342, DateTimeKind.Local).AddTicks(2480) });
        }
    }
}
