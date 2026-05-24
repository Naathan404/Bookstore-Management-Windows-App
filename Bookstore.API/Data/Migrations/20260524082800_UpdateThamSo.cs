using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateThamSo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_LoaiKhachHang_LoaiKhachHangMaLoaiKhachHang",
                table: "KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_KhachHang_LoaiKhachHangMaLoaiKhachHang",
                table: "KhachHang");

            migrationBuilder.DeleteData(
                table: "ThamSo",
                keyColumn: "TenThamSo",
                keyValue: "TiLeTinhdonGiaBan");

            migrationBuilder.DropColumn(
                name: "LoaiKhachHangMaLoaiKhachHang",
                table: "KhachHang");

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

            migrationBuilder.InsertData(
                table: "ThamSo",
                columns: new[] { "TenThamSo", "GiaTri" },
                values: new object[] { "TiLeDonGiaBan", 110 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ThamSo",
                keyColumn: "TenThamSo",
                keyValue: "TiLeDonGiaBan");

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
                value: new DateTime(2026, 5, 16, 2, 11, 50, 0, DateTimeKind.Local).AddTicks(3183));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 16, 2, 11, 50, 0, DateTimeKind.Local).AddTicks(3195));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 16, 2, 11, 50, 0, DateTimeKind.Local).AddTicks(3200));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 16, 2, 11, 50, 0, DateTimeKind.Local).AddTicks(3197));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 16, 2, 11, 50, 0, DateTimeKind.Local).AddTicks(3199));

            migrationBuilder.InsertData(
                table: "ThamSo",
                columns: new[] { "TenThamSo", "GiaTri" },
                values: new object[] { "TiLeTinhdonGiaBan", 110 });

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
    }
}
