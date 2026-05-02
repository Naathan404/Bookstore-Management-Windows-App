using Bookstore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Identity.Client;
using System.Security.Policy;
using System.Security.Principal;

namespace Bookstore.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Sach> Sach { get; set; }
        public DbSet<PhienBanSach> PhienBanSach { get; set; }
        public DbSet<NhaXuatBan> NhaXuatBan { get; set; }
        public DbSet<TacGia> TacGia { get; set; }
        public DbSet<TacGia_Sach> TacGia_Sach { get; set; }
        public DbSet<TheLoai> TheLoai { get; set; }
        public DbSet<NhaCungCap> NhaCungCap { get; set; }
        public DbSet<PhieuNhapSach> PhieuNhapSach { get; set; }
        public DbSet<CT_PhieuNhapSach> CT_PhieuNhapSach { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<LoaiKhachHang> LoaiKhachHang { get; set;  }
        public DbSet<UuDai> UuDai { get; set; }
        public DbSet<LoaiUuDai> LoaiUuDai { get; set; }
        public DbSet<CTUD_HoaDon_Giam> CTUD_HoaDon_Giam { get; set; }
        public DbSet<CTUD_HoaDon_Qua> CTUD_HoaDon_Qua { get; set; }
        public DbSet<CTUD_Sach_Giam> CTUD_Sach_Giam { get; set; }
        public DbSet<CTUD_Sach_Qua> CTUD_Sach_Qua { get; set; }
        public DbSet<UuDai_SachDieuKien> UuDai_SachDieuKien { get; set; }
        public DbSet<UuDai_SachTang> UuDai_SachTang { get; set; }
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<CT_HoaDon> CT_HoaDon { get; set; }
        public DbSet<HoaDon_UuDai> HoaDon_Uudai { get; set;  }
        public DbSet<PhieuThuTien> PhieuThuTien { get; set; }
        public DbSet<BC_Sach> BC_Sach { get; set; }
        public DbSet<CT_BC_Sach> CT_BC_Sach { get; set; }
        public DbSet<BC_KhachHang> BC_KhachHang { get; set; }
        public DbSet<CT_BC_KhachHang> CT_BC_KhachHang { get;set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<NhomNguoiDung> NhomNguoiDung { get; set; }
        public DbSet<ChucNang> ChucNang { get; set; }
        public DbSet<PhanQuyen> PhanQuyen { get; set; }
        public DbSet<ThamSo> ThamSo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PRIMARY KEYS
            modelBuilder.Entity<TacGia_Sach>().HasKey(k => new { k.MaTacGia, k.MaSach });
            modelBuilder.Entity<CT_PhieuNhapSach>().HasKey(k => new { k.MaPhieuNhapSach, k.ISBN });
            modelBuilder.Entity<UuDai_SachDieuKien>().HasKey(k => new { k.MaUuDai, k.ISBN });
            modelBuilder.Entity<UuDai_SachTang>().HasKey(k => new { k.MaUuDai, k.ISBN });
            modelBuilder.Entity<CT_HoaDon>().HasKey(k => new { k.MaHoaDon, k.ISBN });
            modelBuilder.Entity<CT_BC_Sach>().HasKey(k => new { k.MaBaoCaoSach, k.ISBN });
            modelBuilder.Entity<CT_BC_KhachHang>().HasKey(k => new { k.MaBaoCaoKhachHang, k.MaKhachHang });
            modelBuilder.Entity<PhanQuyen>().HasKey(k => new { k.MaChucNang, k.MaNhomNguoiDung });

            // UNIQUE
            modelBuilder.Entity<NhaCungCap>().HasIndex(ncc => ncc.MaSoThue).IsUnique();
            modelBuilder.Entity<NhaCungCap>().HasIndex(ncc => ncc.SoDienThoai).IsUnique();
            modelBuilder.Entity<NhaCungCap>().HasIndex(ncc => ncc.Email).IsUnique();
            modelBuilder.Entity<KhachHang>().HasIndex(k => k.MaSoThue).IsUnique();
            modelBuilder.Entity<KhachHang>().HasIndex(k => k.SoDienThoai).IsUnique();
            modelBuilder.Entity<KhachHang>().HasIndex(k => k.Email).IsUnique();
            modelBuilder.Entity<NguoiDung>().HasIndex(n => n.Email).IsUnique();


            // FOREIGNKEY
            modelBuilder.Entity<Sach>()
                .HasOne<TheLoai>().WithMany().HasForeignKey(s => s.MaTheLoai)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhienBanSach>()
                .HasOne<Sach>().WithMany().HasForeignKey(p => p.MaSach)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PhienBanSach>()
                .HasOne<NhaXuatBan>().WithMany().HasForeignKey(p => p.MaNhaXuatBan)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TacGia_Sach>()
                .HasOne<TacGia>().WithMany().HasForeignKey(t => t.MaTacGia)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TacGia_Sach>()
                .HasOne<Sach>().WithMany().HasForeignKey(s => s.MaSach)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuNhapSach>()
                .HasOne<NhaCungCap>().WithMany().HasForeignKey(n => n.MaNhaCungCap)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CT_PhieuNhapSach>()
                .HasOne<PhieuNhapSach>().WithMany().HasForeignKey(p => p.MaPhieuNhapSach)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CT_PhieuNhapSach>()
                .HasOne<PhienBanSach>().WithMany().HasForeignKey(p => p.ISBN)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<KhachHang>()
                .HasOne<LoaiKhachHang>().WithMany().HasForeignKey(k => k.MaLoaiKhachHang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UuDai>()
                .HasOne<LoaiUuDai>().WithMany().HasForeignKey(u => u.MaLoaiUuDai)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UuDai>()
                .HasOne<LoaiKhachHang>().WithMany().HasForeignKey(u => u.MaLoaiKhachHang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CTUD_HoaDon_Giam>()
                .HasOne<UuDai>().WithMany().HasForeignKey(u => u.MaUuDai)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CTUD_HoaDon_Qua>()
                .HasOne<UuDai>().WithMany().HasForeignKey(u => u.MaUuDai)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CTUD_Sach_Giam>()
                .HasOne<UuDai>().WithMany().HasForeignKey(u => u.MaUuDai)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CTUD_Sach_Qua>()
                .HasOne<UuDai>().WithMany().HasForeignKey(u => u.MaUuDai)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UuDai_SachDieuKien>()
                .HasOne<PhienBanSach>().WithMany().HasForeignKey(dk => dk.ISBN)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UuDai_SachDieuKien>()
                .HasOne<UuDai>().WithMany().HasForeignKey(dk => dk.MaUuDai)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UuDai_SachTang>()
                .HasOne<PhienBanSach>().WithMany().HasForeignKey(dk => dk.ISBN)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UuDai_SachTang>()
                .HasOne<UuDai>().WithMany().HasForeignKey(dk => dk.MaUuDai)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HoaDon>()
                .HasOne<KhachHang>().WithMany().HasForeignKey(h => h.MaKhachHang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CT_HoaDon>()
                .HasOne<HoaDon>().WithMany().HasForeignKey(h => h.MaHoaDon)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CT_HoaDon>()
                .HasOne<PhienBanSach>().WithMany().HasForeignKey(h => h.ISBN)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<HoaDon_UuDai>()
                .HasOne<HoaDon>().WithMany().HasForeignKey(h => h.MaHoaDon)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<HoaDon_UuDai>()
                .HasOne<UuDai>().WithMany().HasForeignKey(h => h.MaUuDai)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<HoaDon_UuDai>()
                .HasOne<PhienBanSach>().WithMany().HasForeignKey(h => h.ISBN)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuThuTien>()
                .HasOne<KhachHang>().WithMany().HasForeignKey(k => k.MaKhachHang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CT_BC_Sach>()
                .HasOne<BC_Sach>().WithMany().HasForeignKey(b => b.MaBaoCaoSach)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CT_BC_Sach>()
                .HasOne<PhienBanSach>().WithMany().HasForeignKey(b => b.ISBN)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CT_BC_KhachHang>()
               .HasOne<BC_KhachHang>().WithMany().HasForeignKey(b => b.MaBaoCaoKhachHang)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CT_BC_KhachHang>()
               .HasOne<KhachHang>().WithMany().HasForeignKey(b => b.MaKhachHang)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NguoiDung>()
               .HasOne<NhomNguoiDung>().WithMany().HasForeignKey(n => n.MaNhomNguoiDung)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhanQuyen>()
               .HasOne<NhomNguoiDung>().WithMany().HasForeignKey(p => p.MaNhomNguoiDung)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PhanQuyen>()
               .HasOne<ChucNang>().WithMany().HasForeignKey(p => p.MaChucNang)
               .OnDelete(DeleteBehavior.Restrict);


            /// SEED data cho NHÓM NGƯỜI DÙNG
            modelBuilder.Entity<NhomNguoiDung>().HasData(
                new NhomNguoiDung { MaNhomNguoiDung = 0, TenNhomNguoiDung = "ADMIN" },
                new NhomNguoiDung { MaNhomNguoiDung = 1, TenNhomNguoiDung = "NHÂN VIÊN" },
                new NhomNguoiDung { MaNhomNguoiDung = 2, TenNhomNguoiDung = "QUẢN LÝ" }
            );

            /// SEED data cho CHUC NANG
            /// // Note đây kiểm tra lại sau khi có tên các màn hình
            modelBuilder.Entity<ChucNang>().HasData(
                    new ChucNang { MaChucNang = 1, TenChucNang = "Dashboard", TenManHinh = "DashboardView" },
                    new ChucNang { MaChucNang = 2, TenChucNang = "Bán hàng", TenManHinh = "BanHangView" },
                    new ChucNang { MaChucNang = 3, TenChucNang = "Tra cứu sách", TenManHinh = "TraCuuSachView" },
                    new ChucNang { MaChucNang = 4, TenChucNang = "Khách hàng", TenManHinh = "KhachHangView" },
                    new ChucNang { MaChucNang = 5, TenChucNang = "Nhập kho", TenManHinh = "NhapKhoView" },
                    new ChucNang { MaChucNang = 6, TenChucNang = "Nhà cung cấp", TenManHinh = "NhaCungCapView" },
                    new ChucNang { MaChucNang = 7, TenChucNang = "Ưu đãi", TenManHinh = "UuDaiView" },
                    new ChucNang { MaChucNang = 8, TenChucNang = "Báo cáo", TenManHinh = "BaoCaoView" },
                    new ChucNang { MaChucNang = 9, TenChucNang = "Tài khoản", TenManHinh = "TaiKhoanView" },
                    new ChucNang { MaChucNang = 10, TenChucNang = "Cài đặt", TenManHinh = "CaiDatView" }
            );

            /// SEED data cho PHÂN QUYỀN
            modelBuilder.Entity<PhanQuyen>().HasData(
                // ADMIN có all quyền
                new PhanQuyen { MaNhomNguoiDung = 0, MaChucNang = 1 },
                new PhanQuyen { MaNhomNguoiDung = 0, MaChucNang = 2 },
                new PhanQuyen { MaNhomNguoiDung = 0, MaChucNang = 3 },
                new PhanQuyen { MaNhomNguoiDung = 0, MaChucNang = 4 },
                new PhanQuyen { MaNhomNguoiDung = 0, MaChucNang = 5 },
                new PhanQuyen { MaNhomNguoiDung = 0, MaChucNang = 6 },
                new PhanQuyen { MaNhomNguoiDung = 0, MaChucNang = 7 },
                new PhanQuyen { MaNhomNguoiDung = 0, MaChucNang = 8 },
                new PhanQuyen { MaNhomNguoiDung = 0, MaChucNang = 9 },
                new PhanQuyen { MaNhomNguoiDung = 0, MaChucNang = 10 },

                // QUẢN LÝ không bán hàng, thêm tài khoản và cài đặt
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 1 },
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 3 },
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 4 },
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 5 },
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 6 },
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 7 },
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 8 },

                // NHÂN VIÊN có quyền bán hàng, tra cứu sách, quản lý khách hàng.
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 2 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 3 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 4 }
            );

            string defaultHash = "f0e65975ed9a43805b030b5e0d4af83239a652b1ddd7fa26b108424e19f48676";
            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung
                {
                    TenDangNhap = "admin",
                    MatKhau = defaultHash,
                    MaNhomNguoiDung = 0,
                    HoTen = "Nguyễn Chí Nguyên",
                    GioiTinh = "Nam",
                    ChucVu = "Giám đốc",
                    Email = "admin@sahara.com",
                    DangLamViec = true,
                    NgaySinh = new DateOnly(2006, 3, 10),
                    NgayVaoLam = new DateOnly(2025, 1, 1)
                },
                new NguoiDung
                {
                    TenDangNhap = "hungng",
                    MatKhau = defaultHash,
                    MaNhomNguoiDung = 2,
                    HoTen = "Nguyễn Gia Hưng",
                    GioiTinh = "Nam",
                    ChucVu = "Quản lý Cửa hàng",
                    Email = "hungng@sahara.com",
                    DangLamViec = true,
                    NgaySinh = new DateOnly(2006, 1, 11),
                    NgayVaoLam = new DateOnly(2025, 2, 1)
                },

                new NguoiDung
                {
                    TenDangNhap = "quanlh",
                    MatKhau = defaultHash,
                    MaNhomNguoiDung = 2,
                    HoTen = "Lê Hoàng Quân",
                    GioiTinh = "Nam",
                    ChucVu = "Quản lý Cửa hàng",
                    Email = "quanlh@sahara.com",
                    DangLamViec = true,
                    NgaySinh = new DateOnly(2006, 1, 11),
                    NgayVaoLam = new DateOnly(2025, 2, 1)
                },

                new NguoiDung
                {
                    TenDangNhap = "sonph",
                    MatKhau = defaultHash,
                    MaNhomNguoiDung = 2,
                    HoTen = "Phạm Hoàng Sơn",
                    GioiTinh = "Nam",
                    ChucVu = "Quản lý Cửa hàng",
                    Email = "sonph@sahara.com",
                    DangLamViec = true,
                    NgaySinh = new DateOnly(2006, 1, 11),
                    NgayVaoLam = new DateOnly(2025, 2, 1)
                },

                new NguoiDung
                {
                    TenDangNhap = "phunlv",
                    MatKhau = defaultHash,
                    MaNhomNguoiDung = 1,
                    HoTen = "Nguyễn Lưu Văn Phú",
                    GioiTinh = "Nam",
                    ChucVu = "Nhân viên Bán hàng",
                    Email = "phunlv@sahara.com",
                    DangLamViec = true,
                    NgaySinh = new DateOnly(2000, 10, 20),
                    NgayVaoLam = new DateOnly(2025, 6, 1)
                }
            );

            /// Tham số
            /// đã đủ
            modelBuilder.Entity<ThamSo>().HasData(
                new ThamSo { TenThamSo = "TiLeTinhdonGiaBan", GiaTri = 110 },
                new ThamSo { TenThamSo = "SoLuongNhapToiThieu", GiaTri = 150 },
                new ThamSo { TenThamSo = "SoLuongTonToiDaCoTheNhap", GiaTri = 300 },
                new ThamSo { TenThamSo = "SoLuongUuDaiToiThieu", GiaTri=1},
                new ThamSo { TenThamSo = "SoLuongUuDaiToiDa", GiaTri=200},
                new ThamSo { TenThamSo= "CoKhoangachCacKhoangGia", GiaTri=1},
                new ThamSo { TenThamSo="ChoPhepKetThucUuDai", GiaTri = 1},
                new ThamSo { TenThamSo="SoLuongTonToiThieu", GiaTri=20},
                new ThamSo { TenThamSo="ThueVAT", GiaTri=8},
                new ThamSo { TenThamSo="TienThuLonHonNo", GiaTri=1}
            );

            // Loại khách hàng và khách hàng
            modelBuilder.Entity<LoaiKhachHang>().HasData(
                new LoaiKhachHang { MaLoaiKhachHang = 1, TenLoaiKhachHang = "Cá nhân", NoToiDa = 1000000m, TiLeTraToiThieu = 0.5 },
                new LoaiKhachHang { MaLoaiKhachHang = 2, TenLoaiKhachHang = "Doanh nghiệp", NoToiDa = 5000000m, TiLeTraToiThieu = 0.25 }
            );

            modelBuilder.Entity<KhachHang>().HasData(
                // Khách vãng lai
                new KhachHang
                {
                    MaKhachHang = 1,
                    TenKhachHang = "Nguyễn Văn Vãng Lai",
                    MaLoaiKhachHang = 1,
                    GioiTinh = 0,
                    NgaySinh = new DateOnly(1990, 1, 1),
                    SoDienThoai = string.Empty,
                    Email = string.Empty,
                    NgayTao = new DateTime(2024, 1, 1, 8, 0, 0),
                    TongDonDaMua = 0,
                    TongTienDaMua = 0m,
                    TienNo = 0m,
                    DiaChi = "",
                    MaSoThue = ""
                },

                // Khách quen
                new KhachHang
                {
                    MaKhachHang = 2,
                    TenKhachHang = "Nguyễn Lưu Văn Phú",
                    MaLoaiKhachHang = 1,
                    GioiTinh = 0,
                    NgaySinh = new DateOnly(1998, 5, 20),
                    SoDienThoai = "0987654321",
                    Email = "vanphu@gmail.com",
                    NgayTao = new DateTime(2024, 2, 15, 9, 30, 0),
                    TongDonDaMua = 5,
                    TongTienDaMua = 1250000m,
                    TienNo = 250000m,
                    DiaChi = "Ký túc xá khu B",
                    MaSoThue = ""
                },

                // Khách Doanh Nghiệp
                new KhachHang
                {
                    MaKhachHang = 3,
                    TenKhachHang = "Công ty TNHH Phần mềm Sahara",
                    MaLoaiKhachHang = 2,
                    GioiTinh = 0,
                    NgaySinh = new DateOnly(2020, 10, 10), // Ngày thành lập cty
                    SoDienThoai = "02838383838",
                    Email = "contact@sahara.vn",
                    NgayTao = new DateTime(2024, 3, 10, 14, 0, 0),
                    TongDonDaMua = 12,
                    TongTienDaMua = 15000000m,
                    TienNo = 0m,
                    DiaChi = "Khu công nghệ cao, TP. Thủ Đức",
                    MaSoThue = "0312345678" 
                },

                // Khách thiếu nợ 
                new KhachHang
                {
                    MaKhachHang = 4,
                    TenKhachHang = "Lê Thành Nghĩa",
                    MaLoaiKhachHang = 1,
                    GioiTinh = 0,
                    NgaySinh = new DateOnly(2002, 8, 12),
                    SoDienThoai = "0912345678",
                    Email = "mai.nguyen@uit.edu.vn",
                    NgayTao = new DateTime(2024, 4, 1, 10, 15, 0),
                    TongDonDaMua = 3,
                    TongTienDaMua = 1800000m,
                    TienNo = 980000m,
                    DiaChi = "Linh Trung, TP. Thủ Đức",
                    MaSoThue = ""
                }
            );


            // THỂ LOẠI 
            modelBuilder.Entity<TheLoai>().HasData(
                new TheLoai { MaTheLoai = 1, TenTheLoai = "Công nghệ thông tin" },
                new TheLoai { MaTheLoai = 2, TenTheLoai = "Văn học nghệ thuật" },
                new TheLoai { MaTheLoai = 3, TenTheLoai = "Kinh tế - Quản trị" },
                new TheLoai { MaTheLoai = 4, TenTheLoai = "Tâm lý - Kỹ năng sống" },
                new TheLoai { MaTheLoai = 5, TenTheLoai = "Thiếu nhi" },
                new TheLoai { MaTheLoai = 6, TenTheLoai = "Truyện tranh" },
                new TheLoai { MaTheLoai = 7, TenTheLoai = "Ngoại ngữ" },
                new TheLoai { MaTheLoai = 8, TenTheLoai = "Lịch sử - Địa lý" },
                new TheLoai { MaTheLoai = 9, TenTheLoai = "Khoa học - Kỹ thuật" },
                new TheLoai { MaTheLoai = 10, TenTheLoai = "Sách giáo khoa - Tham khảo" }
            );

            // BẢNG NHÀ XUẤT BẢN
            modelBuilder.Entity<NhaXuatBan>().HasData(
                new NhaXuatBan { MaNhaXuatBan = 1, TenNhaXuatBan = "NXB Trẻ" },
                new NhaXuatBan { MaNhaXuatBan = 2, TenNhaXuatBan = "NXB Đại học Quốc gia TPHCM" },
                new NhaXuatBan { MaNhaXuatBan = 3, TenNhaXuatBan = "NXB Kim Đồng" },
                new NhaXuatBan { MaNhaXuatBan = 4, TenNhaXuatBan = "NXB Tổng hợp TPHCM" },
                new NhaXuatBan { MaNhaXuatBan = 5, TenNhaXuatBan = "NXB Giáo dục Việt Nam" },
                new NhaXuatBan { MaNhaXuatBan = 6, TenNhaXuatBan = "NXB Hội Nhà văn" },
                new NhaXuatBan { MaNhaXuatBan = 7, TenNhaXuatBan = "NXB Thông tin và Truyền thông" },
                new NhaXuatBan { MaNhaXuatBan = 8, TenNhaXuatBan = "NXB Phụ Nữ" },
                new NhaXuatBan { MaNhaXuatBan = 9, TenNhaXuatBan = "O'Reilly Media" }, 
                new NhaXuatBan { MaNhaXuatBan = 10, TenNhaXuatBan = "Pearson Education" }
            );

            // BẢNG TÁC GIẢ
            modelBuilder.Entity<TacGia>().HasData(
                // tác giả ai ti
                new TacGia { MaTacGia = 1, TenTacGia = "Robert C. Martin" },
                new TacGia { MaTacGia = 2, TenTacGia = "Martin Fowler" },
                new TacGia { MaTacGia = 3, TenTacGia = "Erich Gamma" },

                // văn học Việt Nam
                new TacGia { MaTacGia = 4, TenTacGia = "Nguyễn Nhật Ánh" },
                new TacGia { MaTacGia = 5, TenTacGia = "Vũ Trọng Phụng" },
                new TacGia { MaTacGia = 6, TenTacGia = "Nam Cao" },
                new TacGia { MaTacGia = 7, TenTacGia = "Nguyễn Ngọc Tư" },

                // Nước ngòai
                new TacGia { MaTacGia = 8, TenTacGia = "Haruki Murakami" },
                new TacGia { MaTacGia = 9, TenTacGia = "Paulo Coelho" },
                new TacGia { MaTacGia = 10, TenTacGia = "J.K. Rowling" },
                new TacGia { MaTacGia = 11, TenTacGia = "Fujiko F. Fujio" },
                new TacGia { MaTacGia = 12, TenTacGia = "Aoyama Gosho" },

                // kỹ năng, tâm lý, kinh tế, bla bla bla
                new TacGia { MaTacGia = 13, TenTacGia = "Dale Carnegie" },
                new TacGia { MaTacGia = 14, TenTacGia = "Thích Nhất Hạnh" },
                new TacGia { MaTacGia = 15, TenTacGia = "Nguyễn Hiến Lê" },
                new TacGia { MaTacGia = 16, TenTacGia = "Đặng Hoàng Giang" },
                new TacGia { MaTacGia = 17, TenTacGia = "Tony Buổi Sáng" }
            );

            // 
            // BẢNG SÁCH, à tui seed sẵn 25 cuốn á
            modelBuilder.Entity<Sach>().HasData(
                // NHÓM IT
                new Sach { MaSach = 1, TenSach = "Clean Code", MaTheLoai = 1, MoTa = "Sách gối đầu giường của mọi Dev", ImageUrl = "/Assets/Images/cleancode.jpg" },
                new Sach { MaSach = 2, TenSach = "Refactoring", MaTheLoai = 1, MoTa = "Cải thiện thiết kế code cũ", ImageUrl = "/Assets/Images/refactoring.jpg" },
                new Sach { MaSach = 3, TenSach = "Design Patterns", MaTheLoai = 1, MoTa = "Các mẫu thiết kế chuẩn GOF", ImageUrl = "/Assets/Images/designpatterns.jpg" },
                new Sach { MaSach = 4, TenSach = "The Clean Coder", MaTheLoai = 1, MoTa = "Quy tắc hành nghề coder chuyên nghiệp", ImageUrl = "/Assets/Images/cleancoder.jpg" },
                new Sach { MaSach = 5, TenSach = "300 Bài Code Thiếu Nhi", MaTheLoai = 1, MoTa = "Học xong code bao lương 3 ngàn đô", ImageUrl = "/Assets/Images/300baicode.jpg" },

                // NHÓM VĂN HỌC VIỆT NAM 
                new Sach { MaSach = 6, TenSach = "Mắt Biếc", MaTheLoai = 2, MoTa = "Truyện dài cực hay, tình yêu đau đớn của Ngạn", ImageUrl = "/Assets/Images/matbiec.jpg" },
                new Sach { MaSach = 7, TenSach = "Cho Tôi Xin Một Vé Đi Tuổi Thơ", MaTheLoai = 2, MoTa = "Ký ức tuổi thơ dữ dội", ImageUrl = "/Assets/Images/vetuoitho.jpg" },
                new Sach { MaSach = 8, TenSach = "Số Đỏ", MaTheLoai = 2, MoTa = "Hành trình thăng tiến của Xuân Tóc Đỏ", ImageUrl = "/Assets/Images/sodo.jpg" },
                new Sach { MaSach = 9, TenSach = "Chí Phèo", MaTheLoai = 2, MoTa = "Tuyển tập truyện ngắn Nam Cao", ImageUrl = "/Assets/Images/chipheo.jpg" },
                new Sach { MaSach = 10, TenSach = "Cánh Đồng Bất Tận", MaTheLoai = 2, MoTa = "Nỗi đau trên miền sông nước", ImageUrl = "/Assets/Images/canhdongbattan.jpg" },

                // NHÓM VĂN HỌC NƯỚC NGOÀI
                new Sach { MaSach = 11, TenSach = "Rừng Na Uy", MaTheLoai = 2, MoTa = "Tiểu thuyết nổi tiếng của Haruki Murakami", ImageUrl = "/Assets/Images/rungnauy.jpg" },
                new Sach { MaSach = 12, TenSach = "Kafka Bên Bờ Biển", MaTheLoai = 2, MoTa = "Chuyến phiêu lưu kỳ bí", ImageUrl = "/Assets/Images/kafka.jpg" },
                new Sach { MaSach = 13, TenSach = "Nhà Giả Kim", MaTheLoai = 2, MoTa = "Hành trình đi tìm kho báu của Santiago", ImageUrl = "/Assets/Images/nhagiakim.jpg" },
                new Sach { MaSach = 14, TenSach = "Harry Potter và Hòn Đá Phù Thủy", MaTheLoai = 2, MoTa = "Khởi đầu thế giới phép thuật", ImageUrl = "/Assets/Images/harrypotter1.jpg" },
                new Sach { MaSach = 15, TenSach = "Harry Potter và Phòng Chứa Bí Mật", MaTheLoai = 2, MoTa = "Năm học thứ hai tại Hogwarts", ImageUrl = "/Assets/Images/harrypotter2.jpg" },

                // NHÓM TRUYỆN TRANH
                new Sach { MaSach = 16, TenSach = "Doraemon Tập 1", MaTheLoai = 6, MoTa = "Mèo máy đến từ tương lai", ImageUrl = "/Assets/Images/doraemon1.jpg" },
                new Sach { MaSach = 17, TenSach = "Doraemon Tập 2", MaTheLoai = 6, MoTa = "Những bảo bối thần kỳ", ImageUrl = "/Assets/Images/doraemon2.jpg" },
                new Sach { MaSach = 18, TenSach = "Conan Tập 1", MaTheLoai = 6, MoTa = "Sự khởi đầu của thám tử teo nhỏ", ImageUrl = "/Assets/Images/conan1.jpg" },
                new Sach { MaSach = 19, TenSach = "Conan Tập 2", MaTheLoai = 6, MoTa = "Vụ án mới", ImageUrl = "/Assets/Images/conan2.jpg" },

                // NHÓM KỸ NĂNG, TÂM LÝ, KINH TẾ 
                new Sach { MaSach = 20, TenSach = "Đắc Nhân Tâm", MaTheLoai = 4, MoTa = "Sách kỹ năng giao tiếp hay nhất", ImageUrl = "/Assets/Images/dacnhantam.jpg" },
                new Sach { MaSach = 21, TenSach = "Quẳng Gánh Lo Đi Và Vui Sống", MaTheLoai = 4, MoTa = "Nghệ thuật sống hạnh phúc", ImageUrl = "/Assets/Images/quangganhlo.jpg" },
                new Sach { MaSach = 22, TenSach = "Giận", MaTheLoai = 4, MoTa = "Làm chủ cảm xúc", ImageUrl = "/Assets/Images/gian.jpg" },
                new Sach { MaSach = 23, TenSach = "Thiện, Ác và Smartphone", MaTheLoai = 4, MoTa = "Tâm lý học trên mạng xã hội", ImageUrl = "/Assets/Images/thienac.jpg" },
                new Sach { MaSach = 24, TenSach = "Trên Đường Băng", MaTheLoai = 3, MoTa = "Khởi nghiệp và kinh doanh", ImageUrl = "/Assets/Images/trenduongbang.jpg" },
                new Sach { MaSach = 25, TenSach = "Cà Phê Cùng Tony", MaTheLoai = 3, MoTa = "Chuyện đời chuyện nghề", ImageUrl = "/Assets/Images/caphecungtony.jpg" }
            );
            /// bảng TG_SACH
            modelBuilder.Entity<TacGia_Sach>().HasData(
                new TacGia_Sach { MaSach = 1, MaTacGia = 1 }, // Clean Code - Robert C. Martin
                new TacGia_Sach { MaSach = 2, MaTacGia = 2 }, // Refactoring - Martin Fowler
                new TacGia_Sach { MaSach = 3, MaTacGia = 3 }, // Design Patterns - Erich Gamma
                new TacGia_Sach { MaSach = 4, MaTacGia = 1 }, // The Clean Coder - Robert C. Martin
                new TacGia_Sach { MaSach = 5, MaTacGia = 1 }, // 300 Bài Code - Robert C. Martin

                new TacGia_Sach { MaSach = 6, MaTacGia = 4 }, // Mắt Biếc - NNA
                new TacGia_Sach { MaSach = 7, MaTacGia = 4 }, // Cho tôi xin một vé - NNA
                new TacGia_Sach { MaSach = 8, MaTacGia = 5 }, // Số Đỏ - Vũ Trọng Phụng
                new TacGia_Sach { MaSach = 9, MaTacGia = 6 }, // Chí Phèo - Nam Cao
                new TacGia_Sach { MaSach = 10, MaTacGia = 7 }, // Cánh Đồng Bất Tận - Nguyễn Ngọc Tư

                new TacGia_Sach { MaSach = 11, MaTacGia = 8 }, // Rừng Na Uy - Murakami
                new TacGia_Sach { MaSach = 12, MaTacGia = 8 }, // Kafka - Murakami
                new TacGia_Sach { MaSach = 13, MaTacGia = 9 }, // Nhà Giả Kim - Paulo Coelho
                new TacGia_Sach { MaSach = 14, MaTacGia = 10 }, // Harry Potter 1 - J.K. Rowling
                new TacGia_Sach { MaSach = 15, MaTacGia = 10 }, // Harry Potter 2 - J.K. Rowling

                new TacGia_Sach { MaSach = 16, MaTacGia = 11 }, // Doraemon - Fujiko
                new TacGia_Sach { MaSach = 17, MaTacGia = 11 }, // Doraemon - Fujiko
                new TacGia_Sach { MaSach = 18, MaTacGia = 12 }, // Conan - Aoyama
                new TacGia_Sach { MaSach = 19, MaTacGia = 12 }, // Conan - Aoyama

                new TacGia_Sach { MaSach = 20, MaTacGia = 13 }, // Đắc Nhân Tâm - Dale Carnegie
                new TacGia_Sach { MaSach = 21, MaTacGia = 13 }, // Quẳng Gánh Lo Đi - Dale Carnegie
                new TacGia_Sach { MaSach = 22, MaTacGia = 14 }, // Giận - Thích Nhất Hạnh
                new TacGia_Sach { MaSach = 23, MaTacGia = 16 }, // Thiện, Ác... - Đặng Hoàng Giang
                new TacGia_Sach { MaSach = 24, MaTacGia = 17 }, // Trên Đường Băng - Tony
                new TacGia_Sach { MaSach = 25, MaTacGia = 17 }  // Cà Phê Cùng Tony - Tony
            );


            // 8. PHIÊN BẢN SÁCH
            modelBuilder.Entity<PhienBanSach>().HasData(
            );
        }
    }
}