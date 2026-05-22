using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKhachHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoaiKhachHangMaLoaiKhachHang",
                table: "KhachHang",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "KhachHang",
                keyColumn: "MaKhachHang",
                keyValue: 1,
                column: "LoaiKhachHangMaLoaiKhachHang",
                value: null);

            migrationBuilder.UpdateData(
                table: "KhachHang",
                keyColumn: "MaKhachHang",
                keyValue: 2,
                column: "LoaiKhachHangMaLoaiKhachHang",
                value: null);

            migrationBuilder.UpdateData(
                table: "KhachHang",
                keyColumn: "MaKhachHang",
                keyValue: 3,
                column: "LoaiKhachHangMaLoaiKhachHang",
                value: null);

            migrationBuilder.UpdateData(
                table: "KhachHang",
                keyColumn: "MaKhachHang",
                keyValue: 4,
                column: "LoaiKhachHangMaLoaiKhachHang",
                value: null);

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 9, 2, 35, 44, 224, DateTimeKind.Local).AddTicks(5964));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 9, 2, 35, 44, 224, DateTimeKind.Local).AddTicks(5983));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 9, 2, 35, 44, 224, DateTimeKind.Local).AddTicks(5993));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 9, 2, 35, 44, 224, DateTimeKind.Local).AddTicks(5987));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 9, 2, 35, 44, 224, DateTimeKind.Local).AddTicks(5990));

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_LoaiKhachHangMaLoaiKhachHang",
                table: "KhachHang",
                column: "LoaiKhachHangMaLoaiKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_LoaiKhachHang_LoaiKhachHangMaLoaiKhachHang",
                table: "KhachHang",
                column: "LoaiKhachHangMaLoaiKhachHang",
                principalTable: "LoaiKhachHang",
                principalColumn: "MaLoaiKhachHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_LoaiKhachHang_LoaiKhachHangMaLoaiKhachHang",
                table: "KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_KhachHang_LoaiKhachHangMaLoaiKhachHang",
                table: "KhachHang");

            migrationBuilder.DropColumn(
                name: "LoaiKhachHangMaLoaiKhachHang",
                table: "KhachHang");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 6, 21, 37, 7, 89, DateTimeKind.Local).AddTicks(8721));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 6, 21, 37, 7, 89, DateTimeKind.Local).AddTicks(8733));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 6, 21, 37, 7, 89, DateTimeKind.Local).AddTicks(8738));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 6, 21, 37, 7, 89, DateTimeKind.Local).AddTicks(8735));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 6, 21, 37, 7, 89, DateTimeKind.Local).AddTicks(8736));
        }
    }
}
