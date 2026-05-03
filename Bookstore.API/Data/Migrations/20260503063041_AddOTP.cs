using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HanOTP",
                table: "NguoiDung",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MaOTP",
                table: "NguoiDung",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                columns: new[] { "HanOTP", "MaOTP" },
                values: new object[] { new DateTime(2026, 5, 3, 13, 30, 40, 561, DateTimeKind.Local).AddTicks(8182), "" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                columns: new[] { "HanOTP", "MaOTP" },
                values: new object[] { new DateTime(2026, 5, 3, 13, 30, 40, 561, DateTimeKind.Local).AddTicks(8216), "" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                columns: new[] { "HanOTP", "MaOTP" },
                values: new object[] { new DateTime(2026, 5, 3, 13, 30, 40, 561, DateTimeKind.Local).AddTicks(8222), "" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                columns: new[] { "HanOTP", "MaOTP" },
                values: new object[] { new DateTime(2026, 5, 3, 13, 30, 40, 561, DateTimeKind.Local).AddTicks(8218), "" });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                columns: new[] { "HanOTP", "MaOTP" },
                values: new object[] { new DateTime(2026, 5, 3, 13, 30, 40, 561, DateTimeKind.Local).AddTicks(8220), "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HanOTP",
                table: "NguoiDung");

            migrationBuilder.DropColumn(
                name: "MaOTP",
                table: "NguoiDung");
        }
    }
}
