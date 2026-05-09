using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhanQuyen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "ChucNang",
                columns: new[] { "MaChucNang", "TenChucNang", "TenManHinh" },
                values: new object[] { 11, "Cài đặt", "SettingView" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 10, 1, 51, 20, 534, DateTimeKind.Local).AddTicks(8094));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 10, 1, 51, 20, 534, DateTimeKind.Local).AddTicks(8119));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 10, 1, 51, 20, 534, DateTimeKind.Local).AddTicks(8126));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 10, 1, 51, 20, 534, DateTimeKind.Local).AddTicks(8121));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 10, 1, 51, 20, 534, DateTimeKind.Local).AddTicks(8125));

            migrationBuilder.InsertData(
                table: "PhanQuyen",
                columns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                values: new object[,]
                {
                    { 8, 2 },
                    { 9, 3 },
                    { 11, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PhanQuyen",
                keyColumns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.DeleteData(
                table: "PhanQuyen",
                keyColumns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "PhanQuyen",
                keyColumns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                keyValues: new object[] { 11, 1 });

            migrationBuilder.DeleteData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 11);

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 8,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Báo cáo", "ReportView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 9,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Tài khoản", "AccountView" });

            migrationBuilder.UpdateData(
                table: "ChucNang",
                keyColumn: "MaChucNang",
                keyValue: 10,
                columns: new[] { "TenChucNang", "TenManHinh" },
                values: new object[] { "Cài đặt", "SettingView" });

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
    }
}
