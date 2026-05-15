using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LyDoThu",
                table: "PhieuThuTien",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiTao",
                table: "HoaDon",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 23, 50, 2, 747, DateTimeKind.Local).AddTicks(1413));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 23, 50, 2, 747, DateTimeKind.Local).AddTicks(1439));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 23, 50, 2, 747, DateTimeKind.Local).AddTicks(1445));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 23, 50, 2, 747, DateTimeKind.Local).AddTicks(1441));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 15, 23, 50, 2, 747, DateTimeKind.Local).AddTicks(1443));

            migrationBuilder.UpdateData(
                table: "PhieuThuTien",
                keyColumn: "MaPhieuThuTien",
                keyValue: 1,
                column: "LyDoThu",
                value: "Thu tiền cho hóa đơn còn thiếu");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_NguoiTao",
                table: "HoaDon",
                column: "NguoiTao");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_NguoiDung_NguoiTao",
                table: "HoaDon",
                column: "NguoiTao",
                principalTable: "NguoiDung",
                principalColumn: "TenDangNhap",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_NguoiDung_NguoiTao",
                table: "HoaDon");

            migrationBuilder.DropIndex(
                name: "IX_HoaDon_NguoiTao",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "LyDoThu",
                table: "PhieuThuTien");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiTao",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
    }
}
