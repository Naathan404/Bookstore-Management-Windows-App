using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCustomerConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KhachHang_Email",
                table: "KhachHang");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "KhachHang",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 16, 0, 32, 627, DateTimeKind.Local).AddTicks(9051));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 16, 0, 32, 627, DateTimeKind.Local).AddTicks(9062));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 16, 0, 32, 627, DateTimeKind.Local).AddTicks(9071));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 16, 0, 32, 627, DateTimeKind.Local).AddTicks(9066));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 29, 16, 0, 32, 627, DateTimeKind.Local).AddTicks(9069));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "KhachHang",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Email",
                table: "KhachHang",
                column: "Email",
                unique: true);
        }
    }
}
