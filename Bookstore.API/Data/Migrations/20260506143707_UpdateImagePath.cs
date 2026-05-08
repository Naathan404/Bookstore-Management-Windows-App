using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 1,
                column: "ImageUrl",
                value: "/Resources/Images/Books/cleancode.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 2,
                column: "ImageUrl",
                value: "/Resources/Images/Books/refactoring.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 3,
                column: "ImageUrl",
                value: "/Resources/Images/Books/designpatterns.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 4,
                column: "ImageUrl",
                value: "/Resources/Images/Books/cleancoder.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 5,
                column: "ImageUrl",
                value: "/Resources/Images/Books/300baicode.png");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 6,
                column: "ImageUrl",
                value: "/Resources/Images/Books/matbiec.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 7,
                column: "ImageUrl",
                value: "/Resources/Images/Books/vetuoitho.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 8,
                column: "ImageUrl",
                value: "/Resources/Images/Books/sodo.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 9,
                column: "ImageUrl",
                value: "/Resources/Images/Books/chipheo.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 10,
                column: "ImageUrl",
                value: "/Resources/Images/Books/canhdongbattan.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 11,
                column: "ImageUrl",
                value: "/Resources/Images/Books/rungnauy.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 12,
                column: "ImageUrl",
                value: "/Resources/Images/Books/kafka.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 13,
                column: "ImageUrl",
                value: "/Resources/Images/Books/nhagiakim.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 14,
                column: "ImageUrl",
                value: "/Resources/Images/Books/harrypotter1.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 15,
                column: "ImageUrl",
                value: "/Resources/Images/Books/harrypotter2.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 16,
                column: "ImageUrl",
                value: "/Resources/Images/Books/doraemon1.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 17,
                column: "ImageUrl",
                value: "/Resources/Images/Books/doraemon2.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 18,
                column: "ImageUrl",
                value: "/Resources/Images/Books/conan1.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 19,
                column: "ImageUrl",
                value: "/Resources/Images/Books/conan2.png");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 20,
                column: "ImageUrl",
                value: "/Resources/Images/Books/dacnhantam.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 21,
                column: "ImageUrl",
                value: "/Resources/Images/Books/quangganhlo.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 22,
                column: "ImageUrl",
                value: "/Resources/Images/Books/gian.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 23,
                column: "ImageUrl",
                value: "/Resources/Images/Books/thienac.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 24,
                column: "ImageUrl",
                value: "/Resources/Images/Books/trenduongbang.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 25,
                column: "ImageUrl",
                value: "/Resources/Images/Books/caphecungtony.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 5, 3, 16, 49, 40, 631, DateTimeKind.Local).AddTicks(8556));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 5, 3, 16, 49, 40, 631, DateTimeKind.Local).AddTicks(8570));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 5, 3, 16, 49, 40, 631, DateTimeKind.Local).AddTicks(8574));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 5, 3, 16, 49, 40, 631, DateTimeKind.Local).AddTicks(8571));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 5, 3, 16, 49, 40, 631, DateTimeKind.Local).AddTicks(8573));

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 1,
                column: "ImageUrl",
                value: "/Assets/Images/cleancode.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 2,
                column: "ImageUrl",
                value: "/Assets/Images/refactoring.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 3,
                column: "ImageUrl",
                value: "/Assets/Images/designpatterns.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 4,
                column: "ImageUrl",
                value: "/Assets/Images/cleancoder.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 5,
                column: "ImageUrl",
                value: "/Assets/Images/300baicode.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 6,
                column: "ImageUrl",
                value: "/Assets/Images/matbiec.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 7,
                column: "ImageUrl",
                value: "/Assets/Images/vetuoitho.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 8,
                column: "ImageUrl",
                value: "/Assets/Images/sodo.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 9,
                column: "ImageUrl",
                value: "/Assets/Images/chipheo.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 10,
                column: "ImageUrl",
                value: "/Assets/Images/canhdongbattan.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 11,
                column: "ImageUrl",
                value: "/Assets/Images/rungnauy.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 12,
                column: "ImageUrl",
                value: "/Assets/Images/kafka.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 13,
                column: "ImageUrl",
                value: "/Assets/Images/nhagiakim.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 14,
                column: "ImageUrl",
                value: "/Assets/Images/harrypotter1.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 15,
                column: "ImageUrl",
                value: "/Assets/Images/harrypotter2.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 16,
                column: "ImageUrl",
                value: "/Assets/Images/doraemon1.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 17,
                column: "ImageUrl",
                value: "/Assets/Images/doraemon2.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 18,
                column: "ImageUrl",
                value: "/Assets/Images/conan1.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 19,
                column: "ImageUrl",
                value: "/Assets/Images/conan2.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 20,
                column: "ImageUrl",
                value: "/Assets/Images/dacnhantam.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 21,
                column: "ImageUrl",
                value: "/Assets/Images/quangganhlo.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 22,
                column: "ImageUrl",
                value: "/Assets/Images/gian.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 23,
                column: "ImageUrl",
                value: "/Assets/Images/thienac.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 24,
                column: "ImageUrl",
                value: "/Assets/Images/trenduongbang.jpg");

            migrationBuilder.UpdateData(
                table: "Sach",
                keyColumn: "MaSach",
                keyValue: 25,
                column: "ImageUrl",
                value: "/Assets/Images/caphecungtony.jpg");
        }
    }
}
