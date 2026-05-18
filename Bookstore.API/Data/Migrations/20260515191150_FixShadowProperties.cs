using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixShadowProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CT_PhieuNhapSach_PhienBanSach_PhienBanSachISBN",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_CT_PhieuNhapSach_PhieuNhapSach_PhieuNhapSachMaPhieuNhapSach",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropIndex(
                name: "IX_CT_PhieuNhapSach_PhienBanSachISBN",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropIndex(
                name: "IX_CT_PhieuNhapSach_PhieuNhapSachMaPhieuNhapSach",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropColumn(
                name: "PhienBanSachISBN",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropColumn(
                name: "PhieuNhapSachMaPhieuNhapSach",
                table: "CT_PhieuNhapSach");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhienBanSachISBN",
                table: "CT_PhieuNhapSach",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhieuNhapSachMaPhieuNhapSach",
                table: "CT_PhieuNhapSach",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CT_PhieuNhapSach",
                keyColumns: new[] { "ISBN", "MaPhieuNhapSach" },
                keyValues: new object[] { "978-0132350884", 1 },
                columns: new[] { "PhienBanSachISBN", "PhieuNhapSachMaPhieuNhapSach" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "CT_PhieuNhapSach",
                keyColumns: new[] { "ISBN", "MaPhieuNhapSach" },
                keyValues: new object[] { "978-604-1-09887-1", 1 },
                columns: new[] { "PhienBanSachISBN", "PhieuNhapSachMaPhieuNhapSach" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "CT_PhieuNhapSach",
                keyColumns: new[] { "ISBN", "MaPhieuNhapSach" },
                keyValues: new object[] { "978-604-2-11111-1", 2 },
                columns: new[] { "PhienBanSachISBN", "PhieuNhapSachMaPhieuNhapSach" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 16, 1, 30, 10, 946, DateTimeKind.Local).AddTicks(8962));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 16, 1, 30, 10, 946, DateTimeKind.Local).AddTicks(8973));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 16, 1, 30, 10, 946, DateTimeKind.Local).AddTicks(8978));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 16, 1, 30, 10, 946, DateTimeKind.Local).AddTicks(8975));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 16, 1, 30, 10, 946, DateTimeKind.Local).AddTicks(8977));

            migrationBuilder.CreateIndex(
                name: "IX_CT_PhieuNhapSach_PhienBanSachISBN",
                table: "CT_PhieuNhapSach",
                column: "PhienBanSachISBN");

            migrationBuilder.CreateIndex(
                name: "IX_CT_PhieuNhapSach_PhieuNhapSachMaPhieuNhapSach",
                table: "CT_PhieuNhapSach",
                column: "PhieuNhapSachMaPhieuNhapSach");

            migrationBuilder.AddForeignKey(
                name: "FK_CT_PhieuNhapSach_PhienBanSach_PhienBanSachISBN",
                table: "CT_PhieuNhapSach",
                column: "PhienBanSachISBN",
                principalTable: "PhienBanSach",
                principalColumn: "ISBN");

            migrationBuilder.AddForeignKey(
                name: "FK_CT_PhieuNhapSach_PhieuNhapSach_PhieuNhapSachMaPhieuNhapSach",
                table: "CT_PhieuNhapSach",
                column: "PhieuNhapSachMaPhieuNhapSach",
                principalTable: "PhieuNhapSach",
                principalColumn: "MaPhieuNhapSach");
        }
    }
}
