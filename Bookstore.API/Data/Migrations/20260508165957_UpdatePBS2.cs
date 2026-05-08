using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePBS2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhienBanSach_Sach_SachMaSach",
                table: "PhienBanSach");

            migrationBuilder.DropIndex(
                name: "IX_PhienBanSach_SachMaSach",
                table: "PhienBanSach");

            migrationBuilder.DropColumn(
                name: "SachMaSach",
                table: "PhienBanSach");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 23, 59, 56, 408, DateTimeKind.Local).AddTicks(5598));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 23, 59, 56, 408, DateTimeKind.Local).AddTicks(5611));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 23, 59, 56, 408, DateTimeKind.Local).AddTicks(5616));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 23, 59, 56, 408, DateTimeKind.Local).AddTicks(5613));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 23, 59, 56, 408, DateTimeKind.Local).AddTicks(5614));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SachMaSach",
                table: "PhienBanSach",
                type: "int",
                nullable: true);

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

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-0132350884",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-0137081073",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-0201485677",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-0201633610",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-1-09887-1",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-1-09887-2",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-1-12345-6",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-1-15555-6",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-1-23456-7",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-1-34567-8",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-1-45678-9",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-1-55555-1",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-1-55555-2",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-2-11111-1",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-2-11111-2",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-2-22222-1",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-2-22222-2",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-4-33333-1",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-4-33333-2",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-4-33333-3",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-4-33333-4",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-56-7890-1",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-56-7891-8",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-6-12301-2",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-6-12302-9",
                column: "SachMaSach",
                value: null);

            migrationBuilder.UpdateData(
                table: "PhienBanSach",
                keyColumn: "ISBN",
                keyValue: "978-604-MEME-01",
                column: "SachMaSach",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_PhienBanSach_SachMaSach",
                table: "PhienBanSach",
                column: "SachMaSach");

            migrationBuilder.AddForeignKey(
                name: "FK_PhienBanSach_Sach_SachMaSach",
                table: "PhienBanSach",
                column: "SachMaSach",
                principalTable: "Sach",
                principalColumn: "MaSach");
        }
    }
}
