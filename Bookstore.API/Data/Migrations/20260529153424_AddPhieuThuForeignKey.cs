using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhieuThuForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NguoiTao",
                table: "PhieuThuTien",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 22, 34, 23, 390, DateTimeKind.Local).AddTicks(66));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 22, 34, 23, 390, DateTimeKind.Local).AddTicks(77));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 22, 34, 23, 390, DateTimeKind.Local).AddTicks(90));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 22, 34, 23, 390, DateTimeKind.Local).AddTicks(82));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 22, 34, 23, 390, DateTimeKind.Local).AddTicks(86));

            migrationBuilder.CreateIndex(
                name: "IX_PhieuThuTien_NguoiTao",
                table: "PhieuThuTien",
                column: "NguoiTao");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuThuTien_NguoiDung_NguoiTao",
                table: "PhieuThuTien",
                column: "NguoiTao",
                principalTable: "NguoiDung",
                principalColumn: "TenDangNhap",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuThuTien_NguoiDung_NguoiTao",
                table: "PhieuThuTien");

            migrationBuilder.DropIndex(
                name: "IX_PhieuThuTien_NguoiTao",
                table: "PhieuThuTien");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiTao",
                table: "PhieuThuTien",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 16, 22, 59, 172, DateTimeKind.Local).AddTicks(9321));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 16, 22, 59, 172, DateTimeKind.Local).AddTicks(9331));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 16, 22, 59, 172, DateTimeKind.Local).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 16, 22, 59, 172, DateTimeKind.Local).AddTicks(9334));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 16, 22, 59, 172, DateTimeKind.Local).AddTicks(9337));
        }
    }
}
