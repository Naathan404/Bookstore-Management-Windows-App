using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLoaiUuDai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaLoaiKhachHang",
                table: "UuDai",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // a. Xóa khóa ngoại và Khóa chính (như cũ)
            migrationBuilder.DropForeignKey(name: "FK_UuDai_LoaiUuDai_MaLoaiUuDai", table: "UuDai");
            migrationBuilder.DropPrimaryKey(name: "PK_LoaiUuDai", table: "LoaiUuDai");

            // b. TẠO MỘT CỘT TẠM (Tạm thời chứa dữ liệu)
            migrationBuilder.AddColumn<int>(
                name: "TempMaLoai",
                table: "LoaiUuDai",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // c. Bơm dữ liệu từ cột cũ (có Identity) sang cột tạm (không có Identity)
            migrationBuilder.Sql("UPDATE LoaiUuDai SET TempMaLoai = MaLoaiUuDai");

            // d. XÓA cột cũ đi (Cột có Identity chính thức bay màu)
            migrationBuilder.DropColumn(name: "MaLoaiUuDai", table: "LoaiUuDai");

            // e. ĐỔI TÊN cột tạm thành tên cột chính thức
            migrationBuilder.RenameColumn(
                name: "TempMaLoai",
                table: "LoaiUuDai",
                newName: "MaLoaiUuDai");

            // f. Gắn lại Khóa chính cho cột mới
            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiUuDai",
                table: "LoaiUuDai",
                column: "MaLoaiUuDai");

            // g. Nối lại khóa ngoại
            migrationBuilder.AddForeignKey(
                name: "FK_UuDai_LoaiUuDai_MaLoaiUuDai",
                table: "UuDai",
                column: "MaLoaiUuDai",
                principalTable: "LoaiUuDai",
                principalColumn: "MaLoaiUuDai",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "admin",
                column: "HanOTP",
                value: new DateTime(2026, 6, 3, 13, 16, 9, 733, DateTimeKind.Local).AddTicks(8877));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "hungng",
                column: "HanOTP",
                value: new DateTime(2026, 6, 3, 13, 16, 9, 733, DateTimeKind.Local).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "phunlv",
                column: "HanOTP",
                value: new DateTime(2026, 6, 3, 13, 16, 9, 733, DateTimeKind.Local).AddTicks(8909));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "quanlh",
                column: "HanOTP",
                value: new DateTime(2026, 6, 3, 13, 16, 9, 733, DateTimeKind.Local).AddTicks(8898));

            migrationBuilder.UpdateData(
                table: "NguoiDung",
                keyColumn: "TenDangNhap",
                keyValue: "sonph",
                column: "HanOTP",
                value: new DateTime(2026, 6, 3, 13, 16, 9, 733, DateTimeKind.Local).AddTicks(8903));

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 1,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 5, 29, 13, 16, 9, 734, DateTimeKind.Local).AddTicks(101), new DateTime(2026, 7, 3, 13, 16, 9, 734, DateTimeKind.Local).AddTicks(102), new DateTime(2026, 5, 24, 13, 16, 9, 734, DateTimeKind.Local).AddTicks(91) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 3,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 2, 13, 16, 9, 734, DateTimeKind.Local).AddTicks(120), new DateTime(2026, 8, 2, 13, 16, 9, 734, DateTimeKind.Local).AddTicks(121), new DateTime(2026, 6, 1, 13, 16, 9, 734, DateTimeKind.Local).AddTicks(117) });

            migrationBuilder.UpdateData(
                table: "UuDai",
                keyColumn: "MaUuDai",
                keyValue: 4,
                columns: new[] { "NgayBatDau", "NgayKetThuc", "NgayTao" },
                values: new object[] { new DateTime(2026, 6, 18, 13, 16, 9, 734, DateTimeKind.Local).AddTicks(128), new DateTime(2026, 7, 18, 13, 16, 9, 734, DateTimeKind.Local).AddTicks(129), new DateTime(2026, 6, 3, 13, 16, 9, 734, DateTimeKind.Local).AddTicks(125) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaLoaiKhachHang",
                table: "UuDai",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaLoaiUuDai",
                table: "LoaiUuDai",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

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
