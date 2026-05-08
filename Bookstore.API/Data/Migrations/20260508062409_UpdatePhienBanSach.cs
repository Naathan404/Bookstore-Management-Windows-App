using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhienBanSach : Migration
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
                name: "FK_PhieuNhapSach_NguoiDung_NguoiTao",
                table: "PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapSach_NhaCungCap_MaNhaCungCap",
                table: "PhieuNhapSach");

            migrationBuilder.AddColumn<int>(
                name: "SachMaSach",
                table: "PhienBanSach",
                type: "int",
                nullable: true);

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
                value: new DateTime(2026, 5, 8, 13, 24, 8, 631, DateTimeKind.Local).AddTicks(6510));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 13, 24, 8, 631, DateTimeKind.Local).AddTicks(6525));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 13, 24, 8, 631, DateTimeKind.Local).AddTicks(6607));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 13, 24, 8, 631, DateTimeKind.Local).AddTicks(6531));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 8, 13, 24, 8, 631, DateTimeKind.Local).AddTicks(6601));

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

            migrationBuilder.CreateIndex(
                name: "IX_CT_PhieuNhapSach_PhienBanSachISBN",
                table: "CT_PhieuNhapSach",
                column: "PhienBanSachISBN");

            migrationBuilder.CreateIndex(
                name: "IX_CT_PhieuNhapSach_PhieuNhapSachMaPhieuNhapSach",
                table: "CT_PhieuNhapSach",
                column: "PhieuNhapSachMaPhieuNhapSach");

            migrationBuilder.AddForeignKey(
                name: "FK_CT_PhieuNhapSach_PhienBanSach_ISBN",
                table: "CT_PhieuNhapSach",
                column: "ISBN",
                principalTable: "PhienBanSach",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CT_PhieuNhapSach_PhienBanSach_PhienBanSachISBN",
                table: "CT_PhieuNhapSach",
                column: "PhienBanSachISBN",
                principalTable: "PhienBanSach",
                principalColumn: "ISBN");

            migrationBuilder.AddForeignKey(
                name: "FK_CT_PhieuNhapSach_PhieuNhapSach_MaPhieuNhapSach",
                table: "CT_PhieuNhapSach",
                column: "MaPhieuNhapSach",
                principalTable: "PhieuNhapSach",
                principalColumn: "MaPhieuNhapSach",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CT_PhieuNhapSach_PhieuNhapSach_PhieuNhapSachMaPhieuNhapSach",
                table: "CT_PhieuNhapSach",
                column: "PhieuNhapSachMaPhieuNhapSach",
                principalTable: "PhieuNhapSach",
                principalColumn: "MaPhieuNhapSach");

            migrationBuilder.AddForeignKey(
                name: "FK_PhienBanSach_Sach_SachMaSach",
                table: "PhienBanSach",
                column: "SachMaSach",
                principalTable: "Sach",
                principalColumn: "MaSach");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapSach_NguoiDung_NguoiTao",
                table: "PhieuNhapSach",
                column: "NguoiTao",
                principalTable: "NguoiDung",
                principalColumn: "TenDangNhap",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapSach_NhaCungCap_MaNhaCungCap",
                table: "PhieuNhapSach",
                column: "MaNhaCungCap",
                principalTable: "NhaCungCap",
                principalColumn: "MaNhaCungCap",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CT_PhieuNhapSach_PhienBanSach_ISBN",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_CT_PhieuNhapSach_PhienBanSach_PhienBanSachISBN",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_CT_PhieuNhapSach_PhieuNhapSach_MaPhieuNhapSach",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_CT_PhieuNhapSach_PhieuNhapSach_PhieuNhapSachMaPhieuNhapSach",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_PhienBanSach_Sach_SachMaSach",
                table: "PhienBanSach");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapSach_NguoiDung_NguoiTao",
                table: "PhieuNhapSach");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapSach_NhaCungCap_MaNhaCungCap",
                table: "PhieuNhapSach");

            migrationBuilder.DropIndex(
                name: "IX_PhienBanSach_SachMaSach",
                table: "PhienBanSach");

            migrationBuilder.DropIndex(
                name: "IX_CT_PhieuNhapSach_PhienBanSachISBN",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropIndex(
                name: "IX_CT_PhieuNhapSach_PhieuNhapSachMaPhieuNhapSach",
                table: "CT_PhieuNhapSach");

            migrationBuilder.DropColumn(
                name: "SachMaSach",
                table: "PhienBanSach");

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
    }
}
