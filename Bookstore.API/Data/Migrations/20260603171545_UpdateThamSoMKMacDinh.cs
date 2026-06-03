using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateThamSoMKMacDinh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "ThamSo",
                columns: new[] { "TenThamSo", "GiaTri" },
                values: new object[] { "MatKhauMacDinh", 123456 });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ThamSo",
                keyColumn: "TenThamSo",
                keyValue: "MatKhauMacDinh");

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(1575));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(1601));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(1606));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(1603));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(1604));

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 1,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 5, 28, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2370), new DateTime(2026, 7, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2372), new DateTime(2026, 5, 23, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2348) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 3,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 1, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2386), new DateTime(2026, 8, 1, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2388), new DateTime(2026, 5, 31, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2383) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 4,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 17, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2391), new DateTime(2026, 7, 17, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2392), new DateTime(2026, 6, 2, 15, 21, 9, 124, DateTimeKind.Local).AddTicks(2389) });
        }
    }
}
