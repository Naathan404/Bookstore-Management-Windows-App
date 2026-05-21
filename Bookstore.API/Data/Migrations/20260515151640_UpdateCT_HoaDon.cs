using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCT_HoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CT_HoaDon_HoaDon_MaHoaDon",
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

            migrationBuilder.AddForeignKey(
                name: "FK_CT_HoaDon_HoaDon_MaHoaDon",
                table: "CT_HoaDon",
                column: "MaHoaDon",
                principalTable: "HoaDon",
                principalColumn: "MaHoaDon",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CT_HoaDon_HoaDon_MaHoaDon",
                table: "CT_HoaDon");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 10, 13, 23, 43, 696, DateTimeKind.Local).AddTicks(9361));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 10, 13, 23, 43, 696, DateTimeKind.Local).AddTicks(9374));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 10, 13, 23, 43, 696, DateTimeKind.Local).AddTicks(9379));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 10, 13, 23, 43, 696, DateTimeKind.Local).AddTicks(9375));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 10, 13, 23, 43, 696, DateTimeKind.Local).AddTicks(9378));

            migrationBuilder.AddForeignKey(
                name: "FK_CT_HoaDon_HoaDon_MaHoaDon",
                table: "CT_HoaDon",
                column: "MaHoaDon",
                principalTable: "HoaDon",
                principalColumn: "MaHoaDon",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
