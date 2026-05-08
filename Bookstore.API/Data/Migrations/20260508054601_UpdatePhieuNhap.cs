using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhieuNhap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CT_PhieuNhapSach_PhienBanSach_ISBN",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_CT_PhieuNhapSach_PhieuNhapSach_MaPhieuNhapSach",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapSach_NhaCungCap_MaNhaCungCap",
                table: "PhieuNhapSach");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiTao",
                table: "PhieuNhapSach",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 12, 45, 59, 720, DateTimeKind.Local).AddTicks(8822));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 12, 45, 59, 720, DateTimeKind.Local).AddTicks(8835));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 12, 45, 59, 720, DateTimeKind.Local).AddTicks(8844));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 12, 45, 59, 720, DateTimeKind.Local).AddTicks(8838));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 12, 45, 59, 720, DateTimeKind.Local).AddTicks(8842));

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapSach_NguoiTao",
                table: "PhieuNhapSach",
                column: "NguoiTao");

            migrationBuilder.AddForeignKey(
                name: "FK_CT_PhieuNhapSach_PhienBanSach_ISBN",
                table: "CT_PhieuNhapSach",
                column: "ISBN",
                principalTable: "PhienBanSach",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CT_PhieuNhapSach_PhieuNhapSach_MaPhieuNhapSach",
                table: "CT_PhieuNhapSach",
                column: "MaPhieuNhapSach",
                principalTable: "PhieuNhapSach",
                principalColumn: "MaPhieuNhapSach",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapSach_NguoiDung_NguoiTao",
                table: "PhieuNhapSach",
                column: "NguoiTao",
                principalTable: "NguoiDung",
                principalColumn: "TenDangNhap",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapSach_NhaCungCap_MaNhaCungCap",
                table: "PhieuNhapSach",
                column: "MaNhaCungCap",
                principalTable: "NhaCungCap",
                principalColumn: "MaNhaCungCap",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CT_PhieuNhapSach_PhienBanSach_ISBN",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_CT_PhieuNhapSach_PhieuNhapSach_MaPhieuNhapSach",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapSach_NguoiDung_NguoiTao",
                table: "PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapSach_NhaCungCap_MaNhaCungCap",
                table: "PhieuNhapSach");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNhapSach_NguoiTao",
                table: "PhieuNhapSach");

            migrationBuilder.AlterColumn<string>(
                name: "NguoiTao",
                table: "PhieuNhapSach",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 3, 16, 49, 40, 631, DateTimeKind.Local).AddTicks(8556));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 3, 16, 49, 40, 631, DateTimeKind.Local).AddTicks(8570));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 3, 16, 49, 40, 631, DateTimeKind.Local).AddTicks(8574));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 3, 16, 49, 40, 631, DateTimeKind.Local).AddTicks(8571));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 3, 16, 49, 40, 631, DateTimeKind.Local).AddTicks(8573));

            migrationBuilder.AddForeignKey(
                name: "FK_CT_PhieuNhapSach_PhienBanSach_ISBN",
                table: "CT_PhieuNhapSach",
                column: "ISBN",
                principalTable: "PhienBanSach",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CT_PhieuNhapSach_PhieuNhapSach_MaPhieuNhapSach",
                table: "CT_PhieuNhapSach",
                column: "MaPhieuNhapSach",
                principalTable: "PhieuNhapSach",
                principalColumn: "MaPhieuNhapSach",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapSach_NhaCungCap_MaNhaCungCap",
                table: "PhieuNhapSach",
                column: "MaNhaCungCap",
                principalTable: "NhaCungCap",
                principalColumn: "MaNhaCungCap",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
