using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePromoCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenUuDai",
                table: "UuDai",
                newName: "TenChuongTrinh");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "UuDai",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 20, 32, 46, 289, DateTimeKind.Local).AddTicks(3288));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 20, 32, 46, 289, DateTimeKind.Local).AddTicks(3302));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 20, 32, 46, 289, DateTimeKind.Local).AddTicks(3307));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 20, 32, 46, 289, DateTimeKind.Local).AddTicks(3304));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 20, 32, 46, 289, DateTimeKind.Local).AddTicks(3306));

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 1,
                column: "Code",
                value: "PROMO10");

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 2,
                column: "Code",
                value: "DTN1M");

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 3,
                column: "Code",
                value: "MATBIEC20K");

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 4,
                column: "Code",
                value: "CODESTACK300");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "UuDai");

            migrationBuilder.RenameColumn(
                name: "TenChuongTrinh",
                table: "UuDai",
                newName: "TenUuDai");

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
