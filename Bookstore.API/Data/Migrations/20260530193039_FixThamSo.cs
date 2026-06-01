using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixThamSo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ThamSo",
                keyColumn: "TenThamSo",
                keyValue: "CoKhoangachCacKhoangGia");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 2, 30, 38, 287, DateTimeKind.Local).AddTicks(736));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 2, 30, 38, 287, DateTimeKind.Local).AddTicks(751));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 2, 30, 38, 287, DateTimeKind.Local).AddTicks(767));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 2, 30, 38, 287, DateTimeKind.Local).AddTicks(757));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 2, 30, 38, 287, DateTimeKind.Local).AddTicks(762));

            migrationBuilder.InsertData(
                table: "ThamSo",
                columns: new[] { "TenThamSo", "GiaTri" },
                values: new object[] { "CoKhoangCachCacKhoangGia", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ThamSo",
                keyColumn: "TenThamSo",
                keyValue: "CoKhoangCachCacKhoangGia");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 0, 41, 27, 205, DateTimeKind.Local).AddTicks(6211));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 0, 41, 27, 205, DateTimeKind.Local).AddTicks(6220));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 0, 41, 27, 205, DateTimeKind.Local).AddTicks(6230));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 0, 41, 27, 205, DateTimeKind.Local).AddTicks(6224));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 31, 0, 41, 27, 205, DateTimeKind.Local).AddTicks(6227));

            migrationBuilder.InsertData(
                table: "ThamSo",
                columns: new[] { "TenThamSo", "GiaTri" },
                values: new object[] { "CoKhoangachCacKhoangGia", 1 });
        }
    }
}
