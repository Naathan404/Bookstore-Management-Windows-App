using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                columns: new[] { "ChucVu", "Email", "HanOTP" },
                values: new object[] { "Quản trị viên", "24521186@gm.uit.edu.vn", new DateTime(2026, 5, 3, 14, 34, 51, 304, DateTimeKind.Local).AddTicks(7892) });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                columns: new[] { "Email", "HanOTP" },
                values: new object[] { "24520604@gm.uit.edu.vn", new DateTime(2026, 5, 3, 14, 34, 51, 304, DateTimeKind.Local).AddTicks(7949) });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                columns: new[] { "Email", "HanOTP" },
                values: new object[] { "24521360@g.uit.edu.vn", new DateTime(2026, 5, 3, 14, 34, 51, 304, DateTimeKind.Local).AddTicks(7963) });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                columns: new[] { "Email", "HanOTP" },
                values: new object[] { "24521432@gm.uit.edu.vn", new DateTime(2026, 5, 3, 14, 34, 51, 304, DateTimeKind.Local).AddTicks(7954) });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                columns: new[] { "Email", "HanOTP" },
                values: new object[] { "24521536@gm.uit.edu.vn", new DateTime(2026, 5, 3, 14, 34, 51, 304, DateTimeKind.Local).AddTicks(7956) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                columns: new[] { "ChucVu", "Email", "HanOTP" },
                values: new object[] { "Giám đốc", "admin@sahara.com", new DateTime(2026, 5, 3, 13, 32, 42, 821, DateTimeKind.Local).AddTicks(6826) });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                columns: new[] { "Email", "HanOTP" },
                values: new object[] { "hungng@sahara.com", new DateTime(2026, 5, 3, 13, 32, 42, 821, DateTimeKind.Local).AddTicks(6840) });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                columns: new[] { "Email", "HanOTP" },
                values: new object[] { "phunlv@sahara.com", new DateTime(2026, 5, 3, 13, 32, 42, 821, DateTimeKind.Local).AddTicks(6845) });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                columns: new[] { "Email", "HanOTP" },
                values: new object[] { "quanlh@sahara.com", new DateTime(2026, 5, 3, 13, 32, 42, 821, DateTimeKind.Local).AddTicks(6841) });

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                columns: new[] { "Email", "HanOTP" },
                values: new object[] { "sonph@sahara.com", new DateTime(2026, 5, 3, 13, 32, 42, 821, DateTimeKind.Local).AddTicks(6843) });
        }
    }
}
