using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BC_KhachHang",
                columns: table => new
                {
                    MaBaoCaoKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Thang = table.Column<int>(type: "int", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    TongDoanhThu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongNo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BC_KhachHang", x => x.MaBaoCaoKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "BC_Sach",
                columns: table => new
                {
                    MaBaoCaoSach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Thang = table.Column<int>(type: "int", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    TongDoanhThu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongChiPhi = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BC_Sach", x => x.MaBaoCaoSach);
                });

            migrationBuilder.CreateTable(
                name: "ChucNang",
                columns: table => new
                {
                    MaChucNang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChucNang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenManHinh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucNang", x => x.MaChucNang);
                });

            migrationBuilder.CreateTable(
                name: "LoaiKhachHang",
                columns: table => new
                {
                    MaLoaiKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TiLeTraToiThieu = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiKhachHang", x => x.MaLoaiKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "LoaiUuDai",
                columns: table => new
                {
                    MaLoaiUuDai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiUuDai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApDungToiDa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiUuDai", x => x.MaLoaiUuDai);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaSoThue = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NganHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoTaiKhoan = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCap", x => x.MaNhaCungCap);
                });

            migrationBuilder.CreateTable(
                name: "NhaXuatBan",
                columns: table => new
                {
                    MaNhaXuatBan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaXuatBan = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaXuatBan", x => x.MaNhaXuatBan);
                });

            migrationBuilder.CreateTable(
                name: "NhomNguoiDung",
                columns: table => new
                {
                    MaNhomNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhomNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhomNguoiDung", x => x.MaNhomNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "TacGia",
                columns: table => new
                {
                    MaTacGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTacGia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TacGia", x => x.MaTacGia);
                });

            migrationBuilder.CreateTable(
                name: "ThamSo",
                columns: table => new
                {
                    TenThamSo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GiaTri = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThamSo", x => x.TenThamSo);
                });

            migrationBuilder.CreateTable(
                name: "TheLoai",
                columns: table => new
                {
                    MaTheLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTheLoai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheLoai", x => x.MaTheLoai);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaLoaiKhachHang = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioiTinh = table.Column<int>(type: "int", nullable: false),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: false),
                    MaSoThue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TongTienDaMua = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongDonDaMua = table.Column<int>(type: "int", nullable: false),
                    TienNo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.MaKhachHang);
                    table.ForeignKey(
                        name: "FK_KhachHang_LoaiKhachHang_MaLoaiKhachHang",
                        column: x => x.MaLoaiKhachHang,
                        principalTable: "LoaiKhachHang",
                        principalColumn: "MaLoaiKhachHang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UuDai",
                columns: table => new
                {
                    MaUuDai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaLoaiUuDai = table.Column<int>(type: "int", nullable: false),
                    TenUuDai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuongToiDa = table.Column<int>(type: "int", nullable: false),
                    SoLuongDaDung = table.Column<int>(type: "int", nullable: false),
                    MaLoaiKhachHang = table.Column<int>(type: "int", nullable: false),
                    CoTheSuDung = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UuDai", x => x.MaUuDai);
                    table.ForeignKey(
                        name: "FK_UuDai_LoaiKhachHang_MaLoaiKhachHang",
                        column: x => x.MaLoaiKhachHang,
                        principalTable: "LoaiKhachHang",
                        principalColumn: "MaLoaiKhachHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UuDai_LoaiUuDai_MaLoaiUuDai",
                        column: x => x.MaLoaiUuDai,
                        principalTable: "LoaiUuDai",
                        principalColumn: "MaLoaiUuDai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhieuNhapSach",
                columns: table => new
                {
                    MaPhieuNhapSach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuNhapSach", x => x.MaPhieuNhapSach);
                    table.ForeignKey(
                        name: "FK_PhieuNhapSach_NhaCungCap_MaNhaCungCap",
                        column: x => x.MaNhaCungCap,
                        principalTable: "NhaCungCap",
                        principalColumn: "MaNhaCungCap",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    TenDangNhap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNhomNguoiDung = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: false),
                    NgayVaoLam = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChucVu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DangLamViec = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.TenDangNhap);
                    table.ForeignKey(
                        name: "FK_NguoiDung_NhomNguoiDung_MaNhomNguoiDung",
                        column: x => x.MaNhomNguoiDung,
                        principalTable: "NhomNguoiDung",
                        principalColumn: "MaNhomNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhanQuyen",
                columns: table => new
                {
                    MaNhomNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaChucNang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanQuyen", x => new { x.MaChucNang, x.MaNhomNguoiDung });
                    table.ForeignKey(
                        name: "FK_PhanQuyen_ChucNang_MaChucNang",
                        column: x => x.MaChucNang,
                        principalTable: "ChucNang",
                        principalColumn: "MaChucNang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhanQuyen_NhomNguoiDung_MaNhomNguoiDung",
                        column: x => x.MaNhomNguoiDung,
                        principalTable: "NhomNguoiDung",
                        principalColumn: "MaNhomNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sach",
                columns: table => new
                {
                    MaSach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSach = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaTheLoai = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sach", x => x.MaSach);
                    table.ForeignKey(
                        name: "FK_Sach_TheLoai_MaTheLoai",
                        column: x => x.MaTheLoai,
                        principalTable: "TheLoai",
                        principalColumn: "MaTheLoai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CT_BC_KhachHang",
                columns: table => new
                {
                    MaBaoCaoKhachHang = table.Column<int>(type: "int", nullable: false),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    SoHoaDon = table.Column<int>(type: "int", nullable: false),
                    DoanhThu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TiLeDoanhThu = table.Column<float>(type: "real", nullable: false),
                    NoDau = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoPhatSinh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DaTra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoCuoi = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_BC_KhachHang", x => new { x.MaBaoCaoKhachHang, x.MaKhachHang });
                    table.ForeignKey(
                        name: "FK_CT_BC_KhachHang_BC_KhachHang_MaBaoCaoKhachHang",
                        column: x => x.MaBaoCaoKhachHang,
                        principalTable: "BC_KhachHang",
                        principalColumn: "MaBaoCaoKhachHang",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CT_BC_KhachHang_KhachHang_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    MaHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    TongTienTamTinh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Thue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoTienTra = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.MaHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDon_KhachHang_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhieuThuTien",
                columns: table => new
                {
                    MaPhieuThuTien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    SoTienThu = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuThuTien", x => x.MaPhieuThuTien);
                    table.ForeignKey(
                        name: "FK_PhieuThuTien_KhachHang_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTUD_HoaDon_Giam",
                columns: table => new
                {
                    MaCT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaUuDai = table.Column<int>(type: "int", nullable: false),
                    SoTienToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoTienToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoTienGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TiLeGiam = table.Column<double>(type: "float", nullable: false),
                    GiamToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTUD_HoaDon_Giam", x => x.MaCT);
                    table.ForeignKey(
                        name: "FK_CTUD_HoaDon_Giam_UuDai_MaUuDai",
                        column: x => x.MaUuDai,
                        principalTable: "UuDai",
                        principalColumn: "MaUuDai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTUD_HoaDon_Qua",
                columns: table => new
                {
                    MaCT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaUuDai = table.Column<int>(type: "int", nullable: false),
                    SoTienToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoTienToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTUD_HoaDon_Qua", x => x.MaCT);
                    table.ForeignKey(
                        name: "FK_CTUD_HoaDon_Qua_UuDai_MaUuDai",
                        column: x => x.MaUuDai,
                        principalTable: "UuDai",
                        principalColumn: "MaUuDai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTUD_Sach_Giam",
                columns: table => new
                {
                    MaCT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaUuDai = table.Column<int>(type: "int", nullable: false),
                    SoTienGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TiLeGiam = table.Column<double>(type: "float", nullable: false),
                    GiamToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTUD_Sach_Giam", x => x.MaCT);
                    table.ForeignKey(
                        name: "FK_CTUD_Sach_Giam_UuDai_MaUuDai",
                        column: x => x.MaUuDai,
                        principalTable: "UuDai",
                        principalColumn: "MaUuDai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CTUD_Sach_Qua",
                columns: table => new
                {
                    MaCT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaUuDai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTUD_Sach_Qua", x => x.MaCT);
                    table.ForeignKey(
                        name: "FK_CTUD_Sach_Qua_UuDai_MaUuDai",
                        column: x => x.MaUuDai,
                        principalTable: "UuDai",
                        principalColumn: "MaUuDai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhienBanSach",
                columns: table => new
                {
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    MaNhaXuatBan = table.Column<int>(type: "int", nullable: false),
                    NamXuatBan = table.Column<int>(type: "int", nullable: false),
                    LanTaiBan = table.Column<int>(type: "int", nullable: false),
                    HinhThucBia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaNiemYet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonGiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TonKho = table.Column<int>(type: "int", nullable: false),
                    TongSoDaBan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhienBanSach", x => x.ISBN);
                    table.ForeignKey(
                        name: "FK_PhienBanSach_NhaXuatBan_MaNhaXuatBan",
                        column: x => x.MaNhaXuatBan,
                        principalTable: "NhaXuatBan",
                        principalColumn: "MaNhaXuatBan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhienBanSach_Sach_MaSach",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TacGia_Sach",
                columns: table => new
                {
                    MaTacGia = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TacGia_Sach", x => new { x.MaTacGia, x.MaSach });
                    table.ForeignKey(
                        name: "FK_TacGia_Sach_Sach_MaSach",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TacGia_Sach_TacGia_MaTacGia",
                        column: x => x.MaTacGia,
                        principalTable: "TacGia",
                        principalColumn: "MaTacGia",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CT_BC_Sach",
                columns: table => new
                {
                    MaBaoCaoSach = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoanhThu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChiPhiNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TonDau = table.Column<int>(type: "int", nullable: false),
                    TongNhap = table.Column<int>(type: "int", nullable: false),
                    TongXuat = table.Column<int>(type: "int", nullable: false),
                    TonCuoi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_BC_Sach", x => new { x.MaBaoCaoSach, x.ISBN });
                    table.ForeignKey(
                        name: "FK_CT_BC_Sach_BC_Sach_MaBaoCaoSach",
                        column: x => x.MaBaoCaoSach,
                        principalTable: "BC_Sach",
                        principalColumn: "MaBaoCaoSach",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CT_BC_Sach_PhienBanSach_ISBN",
                        column: x => x.ISBN,
                        principalTable: "PhienBanSach",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CT_HoaDon",
                columns: table => new
                {
                    MaHoaDon = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_HoaDon", x => new { x.MaHoaDon, x.ISBN });
                    table.ForeignKey(
                        name: "FK_CT_HoaDon_HoaDon_MaHoaDon",
                        column: x => x.MaHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "MaHoaDon",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CT_HoaDon_PhienBanSach_ISBN",
                        column: x => x.ISBN,
                        principalTable: "PhienBanSach",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CT_PhieuNhapSach",
                columns: table => new
                {
                    MaPhieuNhapSach = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CT_PhieuNhapSach", x => new { x.MaPhieuNhapSach, x.ISBN });
                    table.ForeignKey(
                        name: "FK_CT_PhieuNhapSach_PhienBanSach_ISBN",
                        column: x => x.ISBN,
                        principalTable: "PhienBanSach",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CT_PhieuNhapSach_PhieuNhapSach_MaPhieuNhapSach",
                        column: x => x.MaPhieuNhapSach,
                        principalTable: "PhieuNhapSach",
                        principalColumn: "MaPhieuNhapSach",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon_Uudai",
                columns: table => new
                {
                    MaCT_HoaDon_UuDai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoaDon = table.Column<int>(type: "int", nullable: false),
                    MaUuDai = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoTienGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon_Uudai", x => x.MaCT_HoaDon_UuDai);
                    table.ForeignKey(
                        name: "FK_HoaDon_Uudai_HoaDon_MaHoaDon",
                        column: x => x.MaHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "MaHoaDon",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDon_Uudai_PhienBanSach_ISBN",
                        column: x => x.ISBN,
                        principalTable: "PhienBanSach",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDon_Uudai_UuDai_MaUuDai",
                        column: x => x.MaUuDai,
                        principalTable: "UuDai",
                        principalColumn: "MaUuDai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UuDai_SachDieuKien",
                columns: table => new
                {
                    MaUuDai = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuongMua = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UuDai_SachDieuKien", x => new { x.MaUuDai, x.ISBN });
                    table.ForeignKey(
                        name: "FK_UuDai_SachDieuKien_PhienBanSach_ISBN",
                        column: x => x.ISBN,
                        principalTable: "PhienBanSach",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UuDai_SachDieuKien_UuDai_MaUuDai",
                        column: x => x.MaUuDai,
                        principalTable: "UuDai",
                        principalColumn: "MaUuDai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UuDai_SachTang",
                columns: table => new
                {
                    MaUuDai = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuongTang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UuDai_SachTang", x => new { x.MaUuDai, x.ISBN });
                    table.ForeignKey(
                        name: "FK_UuDai_SachTang_PhienBanSach_ISBN",
                        column: x => x.ISBN,
                        principalTable: "PhienBanSach",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UuDai_SachTang_UuDai_MaUuDai",
                        column: x => x.MaUuDai,
                        principalTable: "UuDai",
                        principalColumn: "MaUuDai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BC_KhachHang",
                columns: new[] { "MaBaoCaoKhachHang", "Nam", "Thang", "TongDoanhThu", "TongNo" },
                values: new object[,]
                {
                    { 1, 2024, 3, 0m, 0m },
                    { 2, 2024, 4, 0m, 0m },
                    { 3, 2024, 5, 1890000m, 150000m }
                });

            migrationBuilder.InsertData(
                table: "BC_Sach",
                columns: new[] { "MaBaoCaoSach", "Nam", "Thang", "TongChiPhi", "TongDoanhThu" },
                values: new object[,]
                {
                    { 1, 2024, 3, 22000000m, 0m },
                    { 2, 2024, 4, 3000000m, 0m },
                    { 3, 2024, 5, 0m, 1890000m }
                });

            migrationBuilder.InsertData(
                table: "ChucNang",
                columns: new[] { "MaChucNang", "TenChucNang", "TenManHinh" },
                values: new object[,]
                {
                    { 1, "Trang chủ", "DashboardView" },
                    { 2, "Bán hàng", "SaleView" },
                    { 3, "Tra cứu sách", "ProductView" },
                    { 4, "Khách hàng", "CustomerView" },
                    { 5, "Nhập kho", "ImportView" },
                    { 6, "Nhà cung cấp", "SupplierView" },
                    { 7, "Ưu đãi", "PromotionView" },
                    { 8, "Báo cáo", "ReportView" },
                    { 9, "Tài khoản", "AccountView" },
                    { 10, "Cài đặt", "SettingView" }
                });

            migrationBuilder.InsertData(
                table: "LoaiKhachHang",
                columns: new[] { "MaLoaiKhachHang", "NoToiDa", "TenLoaiKhachHang", "TiLeTraToiThieu" },
                values: new object[,]
                {
                    { 1, 1000000m, "Cá nhân", 0.5 },
                    { 2, 5000000m, "Doanh nghiệp", 0.25 }
                });

            migrationBuilder.InsertData(
                table: "LoaiUuDai",
                columns: new[] { "MaLoaiUuDai", "ApDungToiDa", "TenLoaiUuDai" },
                values: new object[,]
                {
                    { 1, 1, "Giảm giá Hóa đơn" },
                    { 2, 1, "Tặng quà theo Hóa đơn" },
                    { 3, 5, "Giảm giá trực tiếp trên Sách" },
                    { 4, 5, "Tặng sách khi mua Sách" }
                });

            migrationBuilder.InsertData(
                table: "NhaCungCap",
                columns: new[] { "MaNhaCungCap", "DiaChi", "Email", "MaSoThue", "NganHang", "SoDienThoai", "SoTaiKhoan", "TenNhaCungCap" },
                values: new object[,]
                {
                    { 1, "387-389 Hai Bà Trưng, Quận 3, TP.HCM", "info@fahasa.com", "0300435133", "Vietcombank", "1900636467", "0071000123456", "Công ty CP Phát hành sách FAHASA" },
                    { 2, "212 Nguyễn Trãi, Quận 1, TP.HCM", "contact@phuongnam.com", "0302221113", "Techcombank", "1900555555", "1901234567890", "Nhà sách Phương Nam" }
                });

            migrationBuilder.InsertData(
                table: "NhaXuatBan",
                columns: new[] { "MaNhaXuatBan", "TenNhaXuatBan" },
                values: new object[,]
                {
                    { 1, "NXB Trẻ" },
                    { 2, "NXB Đại học Quốc gia TPHCM" },
                    { 3, "NXB Kim Đồng" },
                    { 4, "NXB Tổng hợp TPHCM" },
                    { 5, "NXB Giáo dục Việt Nam" },
                    { 6, "NXB Hội Nhà văn" },
                    { 7, "NXB Thông tin và Truyền thông" },
                    { 8, "NXB Phụ Nữ" },
                    { 9, "O'Reilly Media" },
                    { 10, "Pearson Education" }
                });

            migrationBuilder.InsertData(
                table: "NhomNguoiDung",
                columns: new[] { "MaNhomNguoiDung", "TenNhomNguoiDung" },
                values: new object[,]
                {
                    { 1, "ADMIN" },
                    { 2, "NHÂN VIÊN" },
                    { 3, "QUẢN LÝ" }
                });

            migrationBuilder.InsertData(
                table: "TacGia",
                columns: new[] { "MaTacGia", "TenTacGia" },
                values: new object[,]
                {
                    { 1, "Robert C. Martin" },
                    { 2, "Martin Fowler" },
                    { 3, "Erich Gamma" },
                    { 4, "Nguyễn Nhật Ánh" },
                    { 5, "Vũ Trọng Phụng" },
                    { 6, "Nam Cao" },
                    { 7, "Nguyễn Ngọc Tư" },
                    { 8, "Haruki Murakami" },
                    { 9, "Paulo Coelho" },
                    { 10, "J.K. Rowling" },
                    { 11, "Fujiko F. Fujio" },
                    { 12, "Aoyama Gosho" },
                    { 13, "Dale Carnegie" },
                    { 14, "Thích Nhất Hạnh" },
                    { 15, "Nguyễn Hiến Lê" },
                    { 16, "Đặng Hoàng Giang" },
                    { 17, "Tony Buổi Sáng" }
                });

            migrationBuilder.InsertData(
                table: "ThamSo",
                columns: new[] { "TenThamSo", "GiaTri" },
                values: new object[,]
                {
                    { "ChoPhepKetThucUuDai", 1 },
                    { "CoKhoangachCacKhoangGia", 1 },
                    { "SoLuongNhapToiThieu", 150 },
                    { "SoLuongTonToiDaCoTheNhap", 300 },
                    { "SoLuongTonToiThieu", 20 },
                    { "SoLuongUuDaiToiDa", 200 },
                    { "SoLuongUuDaiToiThieu", 1 },
                    { "ThueVAT", 8 },
                    { "TienThuLonHonNo", 1 },
                    { "TiLeTinhdonGiaBan", 110 }
                });

            migrationBuilder.InsertData(
                table: "TheLoai",
                columns: new[] { "MaTheLoai", "TenTheLoai" },
                values: new object[,]
                {
                    { 1, "Công nghệ thông tin" },
                    { 2, "Văn học nghệ thuật" },
                    { 3, "Kinh tế - Quản trị" },
                    { 4, "Tâm lý - Kỹ năng sống" },
                    { 5, "Thiếu nhi" },
                    { 6, "Truyện tranh" },
                    { 7, "Ngoại ngữ" },
                    { 8, "Lịch sử - Địa lý" },
                    { 9, "Khoa học - Kỹ thuật" },
                    { 10, "Sách giáo khoa - Tham khảo" }
                });

            migrationBuilder.InsertData(
                table: "KhachHang",
                columns: new[] { "MaKhachHang", "DiaChi", "Email", "GioiTinh", "MaLoaiKhachHang", "MaSoThue", "NgaySinh", "NgayTao", "SoDienThoai", "TenKhachHang", "TienNo", "TongDonDaMua", "TongTienDaMua" },
                values: new object[,]
                {
                    { 1, "", "", 0, 1, "", new DateOnly(1990, 1, 1), new DateTime(2024, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "", "Khách Vãng Lai", 0m, 0, 0m },
                    { 2, "Ký túc xá khu B", "vanphu@gmail.com", 0, 1, "", new DateOnly(1998, 5, 20), new DateTime(2024, 2, 15, 9, 30, 0, 0, DateTimeKind.Unspecified), "0987654321", "Nguyễn Lưu Văn Phú", 250000m, 5, 1250000m },
                    { 3, "Khu công nghệ cao, TP. Thủ Đức", "contact@sahara.vn", 0, 2, "0312345678", new DateOnly(2020, 10, 10), new DateTime(2024, 3, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), "02838383838", "Công ty TNHH Phần mềm Sahara", 0m, 12, 15000000m },
                    { 4, "Linh Trung, TP. Thủ Đức", "mai.nguyen@uit.edu.vn", 0, 1, "", new DateOnly(2002, 8, 12), new DateTime(2024, 4, 1, 10, 15, 0, 0, DateTimeKind.Unspecified), "0912345678", "Lê Thành Nghĩa", 980000m, 3, 1800000m }
                });

            migrationBuilder.InsertData(
                table: "NguoiDung",
                columns: new[] { "TenDangNhap", "ChucVu", "DangLamViec", "Email", "GioiTinh", "HoTen", "MaNhomNguoiDung", "MatKhau", "NgaySinh", "NgayVaoLam" },
                values: new object[,]
                {
                    { "admin", "Giám đốc", true, "admin@sahara.com", "Nam", "Nguyễn Chí Nguyên", 1, "f0e65975ed9a43805b030b5e0d4af83239a652b1ddd7fa26b108424e19f48676", new DateOnly(2006, 3, 10), new DateOnly(2025, 1, 1) },
                    { "hungng", "Quản lý Cửa hàng", true, "hungng@sahara.com", "Nam", "Nguyễn Gia Hưng", 3, "f0e65975ed9a43805b030b5e0d4af83239a652b1ddd7fa26b108424e19f48676", new DateOnly(2006, 1, 11), new DateOnly(2025, 2, 1) },
                    { "phunlv", "Nhân viên Bán hàng", true, "phunlv@sahara.com", "Nam", "Nguyễn Lưu Văn Phú", 2, "f0e65975ed9a43805b030b5e0d4af83239a652b1ddd7fa26b108424e19f48676", new DateOnly(2000, 10, 20), new DateOnly(2025, 6, 1) },
                    { "quanlh", "Quản lý Cửa hàng", true, "quanlh@sahara.com", "Nam", "Lê Hoàng Quân", 3, "f0e65975ed9a43805b030b5e0d4af83239a652b1ddd7fa26b108424e19f48676", new DateOnly(2006, 1, 11), new DateOnly(2025, 2, 1) },
                    { "sonph", "Quản lý Cửa hàng", true, "sonph@sahara.com", "Nam", "Phạm Hoàng Sơn", 3, "f0e65975ed9a43805b030b5e0d4af83239a652b1ddd7fa26b108424e19f48676", new DateOnly(2006, 1, 11), new DateOnly(2025, 2, 1) }
                });

            migrationBuilder.InsertData(
                table: "PhanQuyen",
                columns: new[] { "MaChucNang", "MaNhomNguoiDung" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 3 },
                    { 4, 1 },
                    { 4, 2 },
                    { 4, 3 },
                    { 5, 1 },
                    { 5, 3 },
                    { 6, 1 },
                    { 6, 3 },
                    { 7, 1 },
                    { 7, 3 },
                    { 8, 1 },
                    { 8, 3 },
                    { 9, 1 },
                    { 10, 1 }
                });

            migrationBuilder.InsertData(
                table: "PhieuNhapSach",
                columns: new[] { "MaPhieuNhapSach", "MaNhaCungCap", "NgayTao", "NguoiTao", "TongTien" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 3, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "admin", 22000000m },
                    { 2, 2, new DateTime(2024, 4, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), "hungng", 3000000m }
                });

            migrationBuilder.InsertData(
                table: "Sach",
                columns: new[] { "MaSach", "ImageUrl", "MaTheLoai", "MoTa", "TenSach" },
                values: new object[,]
                {
                    { 1, "/Assets/Images/cleancode.jpg", 1, "Sách gối đầu giường của mọi Dev", "Clean Code" },
                    { 2, "/Assets/Images/refactoring.jpg", 1, "Cải thiện thiết kế code cũ", "Refactoring" },
                    { 3, "/Assets/Images/designpatterns.jpg", 1, "Các mẫu thiết kế chuẩn GOF", "Design Patterns" },
                    { 4, "/Assets/Images/cleancoder.jpg", 1, "Quy tắc hành nghề coder chuyên nghiệp", "The Clean Coder" },
                    { 5, "/Assets/Images/300baicode.jpg", 1, "Học xong code bao lương 3 ngàn đô", "300 Bài Code Thiếu Nhi" },
                    { 6, "/Assets/Images/matbiec.jpg", 2, "Truyện dài cực hay, tình yêu đau đớn của Ngạn", "Mắt Biếc" },
                    { 7, "/Assets/Images/vetuoitho.jpg", 2, "Ký ức tuổi thơ dữ dội", "Cho Tôi Xin Một Vé Đi Tuổi Thơ" },
                    { 8, "/Assets/Images/sodo.jpg", 2, "Hành trình thăng tiến của Xuân Tóc Đỏ", "Số Đỏ" },
                    { 9, "/Assets/Images/chipheo.jpg", 2, "Tuyển tập truyện ngắn Nam Cao", "Chí Phèo" },
                    { 10, "/Assets/Images/canhdongbattan.jpg", 2, "Nỗi đau trên miền sông nước", "Cánh Đồng Bất Tận" },
                    { 11, "/Assets/Images/rungnauy.jpg", 2, "Tiểu thuyết nổi tiếng của Haruki Murakami", "Rừng Na Uy" },
                    { 12, "/Assets/Images/kafka.jpg", 2, "Chuyến phiêu lưu kỳ bí", "Kafka Bên Bờ Biển" },
                    { 13, "/Assets/Images/nhagiakim.jpg", 2, "Hành trình đi tìm kho báu của Santiago", "Nhà Giả Kim" },
                    { 14, "/Assets/Images/harrypotter1.jpg", 2, "Khởi đầu thế giới phép thuật", "Harry Potter và Hòn Đá Phù Thủy" },
                    { 15, "/Assets/Images/harrypotter2.jpg", 2, "Năm học thứ hai tại Hogwarts", "Harry Potter và Phòng Chứa Bí Mật" },
                    { 16, "/Assets/Images/doraemon1.jpg", 6, "Mèo máy đến từ tương lai", "Doraemon Tập 1" },
                    { 17, "/Assets/Images/doraemon2.jpg", 6, "Những bảo bối thần kỳ", "Doraemon Tập 2" },
                    { 18, "/Assets/Images/conan1.jpg", 6, "Sự khởi đầu của thám tử teo nhỏ", "Conan Tập 1" },
                    { 19, "/Assets/Images/conan2.jpg", 6, "Vụ án mới", "Conan Tập 2" },
                    { 20, "/Assets/Images/dacnhantam.jpg", 4, "Sách kỹ năng giao tiếp hay nhất", "Đắc Nhân Tâm" },
                    { 21, "/Assets/Images/quangganhlo.jpg", 4, "Nghệ thuật sống hạnh phúc", "Quẳng Gánh Lo Đi Và Vui Sống" },
                    { 22, "/Assets/Images/gian.jpg", 4, "Làm chủ cảm xúc", "Giận" },
                    { 23, "/Assets/Images/thienac.jpg", 4, "Tâm lý học trên mạng xã hội", "Thiện, Ác và Smartphone" },
                    { 24, "/Assets/Images/trenduongbang.jpg", 3, "Khởi nghiệp và kinh doanh", "Trên Đường Băng" },
                    { 25, "/Assets/Images/caphecungtony.jpg", 3, "Chuyện đời chuyện nghề", "Cà Phê Cùng Tony" }
                });

            migrationBuilder.InsertData(
                table: "UuDai",
                columns: new[] { "MaUuDai", "CoTheSuDung", "MaLoaiKhachHang", "MaLoaiUuDai", "MoTa", "NgayBatDau", "NgayKetThuc", "NgayTao", "NguoiTao", "SoLuongDaDung", "SoLuongToiDa", "TenUuDai" },
                values: new object[,]
                {
                    { 1, true, 1, 1, "Chương trình kích cầu", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin", 0, 1000, "Giảm 10% Hóa đơn > 500k" },
                    { 2, true, 2, 2, "Tri ân khách VIP", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "quanlh", 0, 50, "Hóa đơn 1Tr tặng Đắc Nhân Tâm" },
                    { 3, true, 1, 3, "Sale sách Hot", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hungng", 0, 200, "Giảm 20k Mắt Biếc" },
                    { 4, true, 1, 4, "Đồng hành cùng IT", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sonph", 0, 100, "Combo Dev: Mua 2 tặng 1" }
                });

            migrationBuilder.InsertData(
                table: "CTUD_HoaDon_Giam",
                columns: new[] { "MaCT", "GiamToiDa", "MaUuDai", "SoTienGiam", "SoTienToiDa", "SoTienToiThieu", "TiLeGiam" },
                values: new object[] { 1, 100m, 1, 0m, 999999999m, 500000m, 0.10000000149011612 });

            migrationBuilder.InsertData(
                table: "CTUD_HoaDon_Qua",
                columns: new[] { "MaCT", "MaUuDai", "SoTienToiDa", "SoTienToiThieu" },
                values: new object[] { 1, 2, 999999999m, 1000000m });

            migrationBuilder.InsertData(
                table: "CTUD_Sach_Giam",
                columns: new[] { "MaCT", "GiamToiDa", "MaUuDai", "SoTienGiam", "TiLeGiam" },
                values: new object[] { 1, 20000m, 3, 20000m, 0.0 });

            migrationBuilder.InsertData(
                table: "CTUD_Sach_Qua",
                columns: new[] { "MaCT", "MaUuDai" },
                values: new object[] { 1, 4 });

            migrationBuilder.InsertData(
                table: "CT_BC_KhachHang",
                columns: new[] { "MaBaoCaoKhachHang", "MaKhachHang", "DaTra", "DoanhThu", "NoCuoi", "NoDau", "NoPhatSinh", "SoHoaDon", "TiLeDoanhThu" },
                values: new object[,]
                {
                    { 3, 1, 0m, 90000m, 0m, 0m, 0m, 1, 4.76f },
                    { 3, 2, 100000m, 900000m, 150000m, 0m, 250000m, 1, 47.62f },
                    { 3, 3, 0m, 900000m, 0m, 0m, 0m, 1, 47.62f }
                });

            migrationBuilder.InsertData(
                table: "HoaDon",
                columns: new[] { "MaHoaDon", "GiamGia", "MaKhachHang", "NgayTao", "NguoiTao", "SoTienTra", "Thue", "TongTien", "TongTienTamTinh" },
                values: new object[,]
                {
                    { 1, 20000m, 1, new DateTime(2024, 5, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), "phunlv", 90000m, 0m, 90000m, 110000m },
                    { 2, 0m, 2, new DateTime(2024, 5, 2, 14, 15, 0, 0, DateTimeKind.Unspecified), "phunlv", 650000m, 0m, 900000m, 900000m },
                    { 3, 100000m, 3, new DateTime(2024, 5, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), "phunlv", 900000m, 0m, 900000m, 1000000m }
                });

            migrationBuilder.InsertData(
                table: "PhienBanSach",
                columns: new[] { "ISBN", "DonGiaBan", "GiaNiemYet", "HinhThucBia", "LanTaiBan", "MaNhaXuatBan", "MaSach", "NamXuatBan", "TonKho", "TongSoDaBan" },
                values: new object[,]
                {
                    { "978-0132350884", 450000m, 450000m, "Bìa mềm", 1, 10, 1, 2008, 50, 15 },
                    { "978-0137081073", 350000m, 350000m, "Bìa mềm", 1, 10, 4, 2011, 40, 12 },
                    { "978-0201485677", 520000m, 550000m, "Bìa cứng", 2, 10, 2, 2018, 30, 5 },
                    { "978-0201633610", 600000m, 600000m, "Bìa cứng", 5, 10, 3, 1994, 20, 2 },
                    { "978-604-1-09887-1", 110000m, 110000m, "Bìa mềm", 15, 1, 6, 2019, 100, 50 },
                    { "978-604-1-09887-2", 220000m, 250000m, "Bìa cứng kỷ niệm", 1, 1, 6, 2020, 15, 10 },
                    { "978-604-1-12345-6", 85000m, 85000m, "Bìa mềm", 10, 1, 7, 2015, 80, 30 },
                    { "978-604-1-15555-6", 90000m, 90000m, "Bìa mềm", 12, 1, 10, 2010, 55, 40 },
                    { "978-604-1-23456-7", 79000m, 79000m, "Bìa mềm", 20, 1, 13, 2020, 200, 150 },
                    { "978-604-1-34567-8", 130000m, 135000m, "Bìa mềm", 10, 1, 14, 2018, 90, 60 },
                    { "978-604-1-45678-9", 135000m, 140000m, "Bìa mềm", 8, 1, 15, 2019, 85, 55 },
                    { "978-604-1-55555-1", 85000m, 85000m, "Bìa mềm", 12, 1, 24, 2017, 100, 150 },
                    { "978-604-1-55555-2", 75000m, 75000m, "Bìa mềm", 15, 1, 25, 2016, 90, 140 },
                    { "978-604-2-11111-1", 20000m, 20000m, "Bìa mềm", 30, 3, 16, 2023, 500, 200 },
                    { "978-604-2-11111-2", 20000m, 20000m, "Bìa mềm", 30, 3, 17, 2023, 480, 190 },
                    { "978-604-2-22222-1", 22000m, 22000m, "Bìa mềm", 25, 3, 18, 2022, 300, 100 },
                    { "978-604-2-22222-2", 22000m, 22000m, "Bìa mềm", 25, 3, 19, 2022, 290, 95 },
                    { "978-604-4-33333-1", 80000m, 85000m, "Bìa mềm", 15, 4, 20, 2021, 150, 80 },
                    { "978-604-4-33333-2", 70000m, 75000m, "Bìa mềm", 10, 4, 21, 2020, 120, 50 },
                    { "978-604-4-33333-3", 90000m, 95000m, "Bìa mềm", 8, 4, 22, 2019, 60, 30 },
                    { "978-604-4-33333-4", 115000m, 120000m, "Bìa mềm", 2, 4, 23, 2022, 80, 20 },
                    { "978-604-56-7890-1", 145000m, 150000m, "Bìa mềm", 5, 8, 11, 2021, 70, 20 },
                    { "978-604-56-7891-8", 175000m, 180000m, "Bìa mềm", 3, 8, 12, 2022, 40, 15 },
                    { "978-604-6-12301-2", 75000m, 75000m, "Bìa mềm", 5, 6, 8, 2018, 45, 10 },
                    { "978-604-6-12302-9", 60000m, 60000m, "Bìa mềm", 8, 6, 9, 2017, 60, 25 },
                    { "978-604-MEME-01", 3000m, 3000m, "Bìa mềm", 1, 7, 5, 2024, 300, 0 }
                });

            migrationBuilder.InsertData(
                table: "PhieuThuTien",
                columns: new[] { "MaPhieuThuTien", "MaKhachHang", "NgayTao", "NguoiTao", "SoTienThu" },
                values: new object[] { 1, 2, new DateTime(2024, 5, 5, 17, 0, 0, 0, DateTimeKind.Unspecified), "phunlv", 100000m });

            migrationBuilder.InsertData(
                table: "TacGia_Sach",
                columns: new[] { "MaSach", "MaTacGia" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 6, 4 },
                    { 7, 4 },
                    { 8, 5 },
                    { 9, 6 },
                    { 10, 7 },
                    { 11, 8 },
                    { 12, 8 },
                    { 13, 9 },
                    { 14, 10 },
                    { 15, 10 },
                    { 16, 11 },
                    { 17, 11 },
                    { 18, 12 },
                    { 19, 12 },
                    { 20, 13 },
                    { 21, 13 },
                    { 22, 14 },
                    { 23, 16 },
                    { 24, 17 },
                    { 25, 17 }
                });

            migrationBuilder.InsertData(
                table: "CT_BC_Sach",
                columns: new[] { "ISBN", "MaBaoCaoSach", "ChiPhiNhap", "DoanhThu", "TonCuoi", "TonDau", "TongNhap", "TongXuat" },
                values: new object[,]
                {
                    { "978-0132350884", 1, 15000000m, 0m, 50, 0, 50, 0 },
                    { "978-604-1-09887-1", 1, 7000000m, 0m, 100, 0, 100, 0 },
                    { "978-0132350884", 2, 0m, 0m, 50, 50, 0, 0 },
                    { "978-604-1-09887-1", 2, 0m, 0m, 100, 100, 0, 0 },
                    { "978-604-2-11111-1", 2, 3000000m, 0m, 200, 0, 200, 0 },
                    { "978-0132350884", 3, 0m, 900000m, 48, 50, 0, 2 },
                    { "978-604-1-09887-1", 3, 0m, 90000m, 99, 100, 0, 1 },
                    { "978-604-2-11111-1", 3, 0m, 100000m, 195, 200, 0, 5 }
                });

            migrationBuilder.InsertData(
                table: "CT_HoaDon",
                columns: new[] { "ISBN", "MaHoaDon", "DonGia", "SoLuong" },
                values: new object[,]
                {
                    { "978-604-1-09887-1", 1, 110000m, 1 },
                    { "978-0132350884", 2, 450000m, 2 },
                    { "978-604-2-11111-1", 3, 20000m, 5 },
                    { "978-604-56-7890-1", 3, 150000m, 6 }
                });

            migrationBuilder.InsertData(
                table: "CT_PhieuNhapSach",
                columns: new[] { "ISBN", "MaPhieuNhapSach", "DonGiaNhap", "SoLuong" },
                values: new object[,]
                {
                    { "978-0132350884", 1, 300000m, 50 },
                    { "978-604-1-09887-1", 1, 70000m, 100 },
                    { "978-604-2-11111-1", 2, 15000m, 200 }
                });

            migrationBuilder.InsertData(
                table: "HoaDon_Uudai",
                columns: new[] { "MaCT_HoaDon_UuDai", "ISBN", "MaHoaDon", "MaUuDai", "SoTienGiam" },
                values: new object[,]
                {
                    { 1, "978-604-1-09887-1", 1, 3, 20000m },
                    { 2, "978-604-MEME-01", 2, 4, 0m },
                    { 3, null, 3, 1, 100000m }
                });

            migrationBuilder.InsertData(
                table: "UuDai_SachDieuKien",
                columns: new[] { "ISBN", "MaUuDai", "SoLuongMua" },
                values: new object[,]
                {
                    { "978-604-1-09887-1", 3, 1 },
                    { "978-0132350884", 4, 2 }
                });

            migrationBuilder.InsertData(
                table: "UuDai_SachTang",
                columns: new[] { "ISBN", "MaUuDai", "SoLuongTang" },
                values: new object[,]
                {
                    { "978-604-4-33333-1", 2, 1 },
                    { "978-604-MEME-01", 4, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CT_BC_KhachHang_MaKhachHang",
                table: "CT_BC_KhachHang",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_CT_BC_Sach_ISBN",
                table: "CT_BC_Sach",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_CT_HoaDon_ISBN",
                table: "CT_HoaDon",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_CT_PhieuNhapSach_ISBN",
                table: "CT_PhieuNhapSach",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_CTUD_HoaDon_Giam_MaUuDai",
                table: "CTUD_HoaDon_Giam",
                column: "MaUuDai");

            migrationBuilder.CreateIndex(
                name: "IX_CTUD_HoaDon_Qua_MaUuDai",
                table: "CTUD_HoaDon_Qua",
                column: "MaUuDai");

            migrationBuilder.CreateIndex(
                name: "IX_CTUD_Sach_Giam_MaUuDai",
                table: "CTUD_Sach_Giam",
                column: "MaUuDai");

            migrationBuilder.CreateIndex(
                name: "IX_CTUD_Sach_Qua_MaUuDai",
                table: "CTUD_Sach_Qua",
                column: "MaUuDai");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaKhachHang",
                table: "HoaDon",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_Uudai_ISBN",
                table: "HoaDon_Uudai",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_Uudai_MaHoaDon",
                table: "HoaDon_Uudai",
                column: "MaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_Uudai_MaUuDai",
                table: "HoaDon_Uudai",
                column: "MaUuDai");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Email",
                table: "KhachHang",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_MaLoaiKhachHang",
                table: "KhachHang",
                column: "MaLoaiKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_SoDienThoai",
                table: "KhachHang",
                column: "SoDienThoai",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_Email",
                table: "NguoiDung",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_MaNhomNguoiDung",
                table: "NguoiDung",
                column: "MaNhomNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_NhaCungCap_Email",
                table: "NhaCungCap",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhaCungCap_MaSoThue",
                table: "NhaCungCap",
                column: "MaSoThue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhaCungCap_SoDienThoai",
                table: "NhaCungCap",
                column: "SoDienThoai",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhanQuyen_MaNhomNguoiDung",
                table: "PhanQuyen",
                column: "MaNhomNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_PhienBanSach_MaNhaXuatBan",
                table: "PhienBanSach",
                column: "MaNhaXuatBan");

            migrationBuilder.CreateIndex(
                name: "IX_PhienBanSach_MaSach",
                table: "PhienBanSach",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapSach_MaNhaCungCap",
                table: "PhieuNhapSach",
                column: "MaNhaCungCap");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuThuTien_MaKhachHang",
                table: "PhieuThuTien",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_Sach_MaTheLoai",
                table: "Sach",
                column: "MaTheLoai");

            migrationBuilder.CreateIndex(
                name: "IX_TacGia_Sach_MaSach",
                table: "TacGia_Sach",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_UuDai_MaLoaiKhachHang",
                table: "UuDai",
                column: "MaLoaiKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_UuDai_MaLoaiUuDai",
                table: "UuDai",
                column: "MaLoaiUuDai");

            migrationBuilder.CreateIndex(
                name: "IX_UuDai_SachDieuKien_ISBN",
                table: "UuDai_SachDieuKien",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_UuDai_SachTang_ISBN",
                table: "UuDai_SachTang",
                column: "ISBN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CT_BC_KhachHang");

            migrationBuilder.DropTable(
                name: "CT_BC_Sach");

            migrationBuilder.DropTable(
                name: "CT_HoaDon");

            migrationBuilder.DropTable(
                name: "CT_PhieuNhapSach");

            migrationBuilder.DropTable(
                name: "CTUD_HoaDon_Giam");

            migrationBuilder.DropTable(
                name: "CTUD_HoaDon_Qua");

            migrationBuilder.DropTable(
                name: "CTUD_Sach_Giam");

            migrationBuilder.DropTable(
                name: "CTUD_Sach_Qua");

            migrationBuilder.DropTable(
                name: "HoaDon_Uudai");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "PhanQuyen");

            migrationBuilder.DropTable(
                name: "PhieuThuTien");

            migrationBuilder.DropTable(
                name: "TacGia_Sach");

            migrationBuilder.DropTable(
                name: "ThamSo");

            migrationBuilder.DropTable(
                name: "UuDai_SachDieuKien");

            migrationBuilder.DropTable(
                name: "UuDai_SachTang");

            migrationBuilder.DropTable(
                name: "BC_KhachHang");

            migrationBuilder.DropTable(
                name: "BC_Sach");

            migrationBuilder.DropTable(
                name: "PhieuNhapSach");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "ChucNang");

            migrationBuilder.DropTable(
                name: "NhomNguoiDung");

            migrationBuilder.DropTable(
                name: "TacGia");

            migrationBuilder.DropTable(
                name: "PhienBanSach");

            migrationBuilder.DropTable(
                name: "UuDai");

            migrationBuilder.DropTable(
                name: "NhaCungCap");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "NhaXuatBan");

            migrationBuilder.DropTable(
                name: "Sach");

            migrationBuilder.DropTable(
                name: "LoaiUuDai");

            migrationBuilder.DropTable(
                name: "LoaiKhachHang");

            migrationBuilder.DropTable(
                name: "TheLoai");
        }
    }
}
