using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLoaiKH2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaLoaiKhachHang",
                table: "KhachHang",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "LoaiKhachHang",
                keyColumn: "MaLoaiKhachHang",
                keyValue: 1,
                column: "TiLeTraToiThieu",
                value: 50.0);

            migrationBuilder.UpdateData(
                table: "LoaiKhachHang",
                keyColumn: "MaLoaiKhachHang",
                keyValue: 2,
                column: "TiLeTraToiThieu",
                value: 25.0);

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 6, 4, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(6947));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 6, 4, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(6964));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 6, 4, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(6971));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 6, 4, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(6967));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 6, 4, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(6969));

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 1,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 5, 30, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(7548), new DateTime(2026, 7, 4, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(7550), new DateTime(2026, 5, 25, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(7536) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 3,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 3, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(7560), new DateTime(2026, 8, 3, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(7560), new DateTime(2026, 6, 2, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(7559) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 4,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 19, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(7562), new DateTime(2026, 7, 19, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(7562), new DateTime(2026, 6, 4, 2, 49, 49, 8, DateTimeKind.Local).AddTicks(7561) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaLoaiKhachHang",
                table: "KhachHang",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "LoaiKhachHang",
                keyColumn: "MaLoaiKhachHang",
                keyValue: 1,
                column: "TiLeTraToiThieu",
                value: 0.5);

            migrationBuilder.UpdateData(
                table: "LoaiKhachHang",
                keyColumn: "MaLoaiKhachHang",
                keyValue: 2,
                column: "TiLeTraToiThieu",
                value: 0.25);

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 6, 4, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(8529));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 6, 4, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(8545));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 6, 4, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(8550));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 6, 4, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(8546));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 6, 4, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(8548));

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 1,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 5, 30, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(9089), new DateTime(2026, 7, 4, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(9090), new DateTime(2026, 5, 25, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(9077) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 3,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 3, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(9096), new DateTime(2026, 8, 3, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(9096), new DateTime(2026, 6, 2, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(9095) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 4,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 19, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(9098), new DateTime(2026, 7, 19, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(9098), new DateTime(2026, 6, 4, 0, 15, 44, 693, DateTimeKind.Local).AddTicks(9097) });
        }
    }
}
