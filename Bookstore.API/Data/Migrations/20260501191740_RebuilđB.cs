using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookstore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RebuilđB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCounts");

            migrationBuilder.DropTable(
                name: "ImportDetails");

            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "MonthlyDebtReports");

            migrationBuilder.DropTable(
                name: "MonthlyStockReports");

            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.DropTable(
                name: "StockTransferDetails");

            migrationBuilder.DropTable(
                name: "ImportReceipts");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "StockTransfers");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Categories");

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
                    NhaNhaCungCap = table.Column<int>(type: "int", nullable: false)
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
                    table.PrimaryKey("PK_NhaCungCap", x => x.NhaNhaCungCap);
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
                    MaSoThue = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    NguoiTao = table.Column<int>(type: "int", nullable: false),
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
                    NguoiTao = table.Column<int>(type: "int", nullable: false),
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
                        principalColumn: "NhaNhaCungCap",
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
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    NguoiTao = table.Column<int>(type: "int", nullable: false),
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
                    NguoiTao = table.Column<int>(type: "int", nullable: false),
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
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                name: "IX_KhachHang_MaSoThue",
                table: "KhachHang",
                column: "MaSoThue",
                unique: true);

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

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OTP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OTPExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    StockID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.StockID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalDebt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOrders = table.Column<int>(type: "int", nullable: false),
                    TotalPurchaseValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_Customers_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edition = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookID);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransfers",
                columns: table => new
                {
                    TransferID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    FromStockID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ToStockID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransfers", x => x.TransferID);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Positions_FromStockID",
                        column: x => x.FromStockID,
                        principalTable: "Positions",
                        principalColumn: "StockID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Positions_ToStockID",
                        column: x => x.ToStockID,
                        principalTable: "Positions",
                        principalColumn: "StockID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportReceipts",
                columns: table => new
                {
                    ImportReceiptID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ReviewedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StockID = table.Column<int>(type: "int", nullable: false),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportReceipts", x => x.ImportReceiptID);
                    table.ForeignKey(
                        name: "FK_ImportReceipts_Positions_StockID",
                        column: x => x.StockID,
                        principalTable: "Positions",
                        principalColumn: "StockID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportReceipts_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyDebtReports",
                columns: table => new
                {
                    ReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    EndDebt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncurredDebt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDebt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyDebtReports", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK_MonthlyDebtReports_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookCounts",
                columns: table => new
                {
                    BookCountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookID = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    StockID = table.Column<int>(type: "int", nullable: false),
                    Threshold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCounts", x => x.BookCountID);
                    table.ForeignKey(
                        name: "FK_BookCounts_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCounts_Positions_StockID",
                        column: x => x.StockID,
                        principalTable: "Positions",
                        principalColumn: "StockID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyStockReports",
                columns: table => new
                {
                    ReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookID = table.Column<int>(type: "int", nullable: false),
                    EndCount = table.Column<int>(type: "int", nullable: false),
                    ExportedCount = table.Column<int>(type: "int", nullable: false),
                    ImportedCount = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    StartCount = table.Column<int>(type: "int", nullable: false),
                    StockID = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyStockReports", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK_MonthlyStockReports_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyStockReports_Positions_StockID",
                        column: x => x.StockID,
                        principalTable: "Positions",
                        principalColumn: "StockID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    PromotionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountRate = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GiftBookID = table.Column<int>(type: "int", nullable: false),
                    GiftQuantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitUsed = table.Column<int>(type: "int", nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumOrderValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PromoType = table.Column<int>(type: "int", nullable: false),
                    RequiredBookID = table.Column<int>(type: "int", nullable: false),
                    RequiredQuantity = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalUsed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.PromotionID);
                    table.ForeignKey(
                        name: "FK_Promotions_Books_GiftBookID",
                        column: x => x.GiftBookID,
                        principalTable: "Books",
                        principalColumn: "BookID");
                    table.ForeignKey(
                        name: "FK_Promotions_Books_RequiredBookID",
                        column: x => x.RequiredBookID,
                        principalTable: "Books",
                        principalColumn: "BookID");
                });

            migrationBuilder.CreateTable(
                name: "StockTransferDetails",
                columns: table => new
                {
                    StockTransferDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TransferID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransferDetails", x => x.StockTransferDetailID);
                    table.ForeignKey(
                        name: "FK_StockTransferDetails_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransferDetails_StockTransfers_TransferID",
                        column: x => x.TransferID,
                        principalTable: "StockTransfers",
                        principalColumn: "TransferID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportDetails",
                columns: table => new
                {
                    ImportDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookID = table.Column<int>(type: "int", nullable: false),
                    ImportReceiptID = table.Column<int>(type: "int", nullable: false),
                    ImportedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportDetails", x => x.ImportDetailID);
                    table.ForeignKey(
                        name: "FK_ImportDetails_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportDetails_ImportReceipts_ImportReceiptID",
                        column: x => x.ImportReceiptID,
                        principalTable: "ImportReceipts",
                        principalColumn: "ImportReceiptID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    CustomerPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DebtIncurred = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PromotionID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_Invoices_Promotions_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "Promotions",
                        principalColumn: "PromotionID");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    InvoiceDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookID = table.Column<int>(type: "int", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceID = table.Column<int>(type: "int", nullable: false),
                    PromotionID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StockID = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.InvoiceDetailID);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Invoices_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Positions_StockID",
                        column: x => x.StockID,
                        principalTable: "Positions",
                        principalColumn: "StockID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Promotions_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "Promotions",
                        principalColumn: "PromotionID");
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    ReceiptID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    InvoiceID = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    ReceiptValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.ReceiptID);
                    table.ForeignKey(
                        name: "FK_Receipts_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_Receipts_Invoices_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceID");
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountID", "Email", "IsDeleted", "OTP", "OTPExpire", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "nathannguyen6002@gmail.com", false, "", null, "fa980dbf4533c98fa5ed792374bea691610dfaabc62558182f4cc814ef0d69db", 0, "admin" },
                    { 2, "24521186@gm.uit.edu.vn", false, "", null, "fa980dbf4533c98fa5ed792374bea691610dfaabc62558182f4cc814ef0d69db", 1, "staff" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Truyện tranh" },
                    { 2, "Giáo trình" },
                    { 3, "Kỹ năng sống" },
                    { 4, "Văn học" },
                    { 5, "Kinh tế" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerID", "AccountID", "Address", "CreatedAt", "CustomerName", "Email", "Gender", "IsDeleted", "PhoneNumber", "TotalDebt", "TotalOrders", "TotalPurchaseValue" },
                values: new object[] { 1, null, "Cần Thơ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyễn Gia Hưng", "", 0, false, "0901234567", 0m, 0, 0m });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "StockID", "Description", "Priority", "StockName" },
                values: new object[,]
                {
                    { 1, "Kệ chính, đối diện cửa ra vào", 0, "Dãy A" },
                    { 2, "Kho tầng 1, lưu trữ hàng mới nhập về", 1, "Kho tổng 1" },
                    { 3, "Kho tầng 2, lưu trữ hàng tồn kho, chưa xả được", 1, "Kho tổng 2" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierID", "Address", "Email", "Phonenumber", "SupplierName" },
                values: new object[,]
                {
                    { 1, "55 Quang Trung, Hà Nội", "cskh@nxbkimdong.com.vn", "1900571595", "NXB Kim Đồng" },
                    { 2, "161B Lý Chính Thắng, Quận 3, TP.HCM", "hopthu@nxbtre.com.vn", "02839316289", "NXB Trẻ" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookID", "Author", "CategoryID", "CreatedAt", "CreatedBy", "Description", "Edition", "ISBN", "ImageURL", "ImportedPrice", "IsDeleted", "ListPrice", "Price", "Publisher", "StockQuantity", "Title" },
                values: new object[,]
                {
                    { 1, "Aoyama Gosho", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "", 1, "978-604-2-27297-1", "", 18000m, false, 25000m, 25000m, "NXB Kim Đồng", 100, "Conan Tập 100" },
                    { 2, "Nguyễn Hữu Hùng", 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "", 5, "978-604-1-18321-4", "", 85000m, false, 120000m, 120000m, "NXB Trẻ", 20, "Lập trình C#" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Username",
                table: "Accounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookCounts_BookID",
                table: "BookCounts",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_BookCounts_StockID",
                table: "BookCounts",
                column: "StockID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryID",
                table: "Books",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AccountID",
                table: "Customers",
                column: "AccountID",
                unique: true,
                filter: "[AccountID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportDetails_BookID",
                table: "ImportDetails",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_ImportDetails_ImportReceiptID",
                table: "ImportDetails",
                column: "ImportReceiptID");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceipts_StockID",
                table: "ImportReceipts",
                column: "StockID");

            migrationBuilder.CreateIndex(
                name: "IX_ImportReceipts_SupplierID",
                table: "ImportReceipts",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_BookID",
                table: "InvoiceDetails",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_InvoiceID",
                table: "InvoiceDetails",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_PromotionID",
                table: "InvoiceDetails",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_StockID",
                table: "InvoiceDetails",
                column: "StockID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerID",
                table: "Invoices",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PromotionID",
                table: "Invoices",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyDebtReports_CustomerID",
                table: "MonthlyDebtReports",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyStockReports_BookID",
                table: "MonthlyStockReports",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyStockReports_StockID",
                table: "MonthlyStockReports",
                column: "StockID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Code",
                table: "Promotions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_GiftBookID",
                table: "Promotions",
                column: "GiftBookID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_RequiredBookID",
                table: "Promotions",
                column: "RequiredBookID");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_CustomerID",
                table: "Receipts",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_InvoiceID",
                table: "Receipts",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferDetails_BookID",
                table: "StockTransferDetails",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferDetails_TransferID",
                table: "StockTransferDetails",
                column: "TransferID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_FromStockID",
                table: "StockTransfers",
                column: "FromStockID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_ToStockID",
                table: "StockTransfers",
                column: "ToStockID");
        }
    }
}
