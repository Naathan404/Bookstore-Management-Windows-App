using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReceipPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PhanQuyen",
                keyColumns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 5,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Phiếu nhập", "ReceiptView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 6,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Nhập kho", "ImportView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 7,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Nhà cung cấp", "SupplierView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 8,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Ưu đãi", "PromotionView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 9,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Danh mục", "CategoryView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 10,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Báo cáo", "ReportView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 11,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Tài khoản", "AccountView" });

            migrationBuilder.InsertData(
                table: "ChucNang",
                columns: new[] { "MaChucNang", "TenChucNang", "TenManHinh" },
                values: new object[] { 12, "Cài đặt", "SettingView" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 18, 51, 41, 486, DateTimeKind.Local).AddTicks(9277));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 18, 51, 41, 486, DateTimeKind.Local).AddTicks(9287));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 18, 51, 41, 486, DateTimeKind.Local).AddTicks(9299));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 18, 51, 41, 486, DateTimeKind.Local).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 30, 18, 51, 41, 486, DateTimeKind.Local).AddTicks(9293));

            migrationBuilder.InsertData(
                table: "PhanQuyen",
                columns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                values: new object[,]
                {
                    { 9, 2 },
                    { 10, 3 },
                    { 12, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PhanQuyen",
                keyColumns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                keyValues: new object[] { 9, 2 });

            migrationBuilder.DeleteData(
                table: "PhanQuyen",
                keyColumns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                keyValues: new object[] { 10, 3 });

            migrationBuilder.DeleteData(
                table: "PhanQuyen",
                keyColumns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                keyValues: new object[] { 12, 1 });

            migrationBuilder.DeleteData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 5,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Nhập kho", "ImportView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 6,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Nhà cung cấp", "SupplierView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 7,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Ưu đãi", "PromotionView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 8,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Danh mục", "CategoryView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 9,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Báo cáo", "ReportView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 10,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Tài khoản", "AccountView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 11,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Cài đặt", "SettingView" });

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

            migrationBuilder.InsertData(
                table: "PhanQuyen",
                columns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                values: new object[] { 8, 2 });
        }
    }
}
