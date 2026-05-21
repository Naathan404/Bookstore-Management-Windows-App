using Bookstore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<NhomNguoiDung> NhomNguoiDung { get; set; }
        public DbSet<ChucNang> ChucNang { get; set; }
        public DbSet<PhanQuyen> PhanQuyen { get; set; }
        public DbSet<ThamSo> ThamSo { get; set; }

        //
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<LoaiKhachHang> LoaiKhachHang { get; set;  }
        //
        public DbSet<TacGia> TacGia { get; set; }
        public DbSet<TheLoai> TheLoai { get; set; }
        public DbSet<NhaXuatBan> NhaXuatBan { get; set; }
        public DbSet<Sach> Sach { get; set; }
        public DbSet<TacGia_Sach> TacGia_Sach { get; set; }
        public DbSet<PhienBanSach> PhienBanSach { get; set; }
        //
        public DbSet<NhaCungCap> NhaCungCap { get; set; }
        public DbSet<PhieuNhapSach> PhieuNhapSach { get; set; }
        public DbSet<CT_PhieuNhapSach> CT_PhieuNhapSach { get; set; }

        //
        public DbSet<UuDai> UuDai { get; set; }
        public DbSet<LoaiUuDai> LoaiUuDai { get; set; }
        public DbSet<CTUD_HoaDon_Giam> CTUD_HoaDon_Giam { get; set; }
        public DbSet<CTUD_HoaDon_Qua> CTUD_HoaDon_Qua { get; set; }
        public DbSet<CTUD_Sach_Giam> CTUD_Sach_Giam { get; set; }
        public DbSet<CTUD_Sach_Qua> CTUD_Sach_Qua { get; set; }
        public DbSet<UuDai_SachDieuKien> UuDai_SachDieuKien { get; set; }
        public DbSet<UuDai_SachTang> UuDai_SachTang { get; set; }

        //
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<CT_HoaDon> CT_HoaDon { get; set; }
        public DbSet<HoaDon_UuDai> HoaDon_Uudai { get; set;  }
        public DbSet<PhieuThuTien> PhieuThuTien { get; set; }

        //
        public DbSet<BC_Sach> BC_Sach { get; set; }
        public DbSet<CT_BC_Sach> CT_BC_Sach { get; set; }
        public DbSet<BC_KhachHang> BC_KhachHang { get; set; }
        public DbSet<CT_BC_KhachHang> CT_BC_KhachHang { get;set; }

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
            modelBuilder.Entity<KhachHang>().HasIndex(k => k.SoDienThoai).IsUnique();
            modelBuilder.Entity<KhachHang>().HasIndex(k => k.Email).IsUnique();
            modelBuilder.Entity<NguoiDung>().HasIndex(n => n.Email).IsUnique();


            // FOREIGNKEY
            modelBuilder.Entity<Sach>()
                .HasOne<TheLoai>().WithMany().HasForeignKey(s => s.MaTheLoai)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Sach>()
                .HasOne(p => p.TheLoai)
                .WithMany()
                .HasForeignKey(p => p.MaTheLoai)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhienBanSach>()
                .HasOne(s => s.Sach).WithMany().HasForeignKey(p => p.MaSach)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PhienBanSach>()
                            .HasOne(p => p.NhaXuatBan)
                            .WithMany()
                            .HasForeignKey(p => p.MaNhaXuatBan)
                            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TacGia_Sach>()
                            .HasOne(p => p.TacGia)
                            .WithMany()
                            .HasForeignKey(p => p.MaTacGia)
                            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TacGia_Sach>()
                            .HasOne(p => p.Sach)
                            .WithMany()
                            .HasForeignKey(p => p.MaSach)
                            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuNhapSach>()
                .HasOne(p => p.NhaCungCap).WithMany().HasForeignKey(n => n.MaNhaCungCap)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PhieuNhapSach>()
                .HasOne(p => p.NguoiDung).WithMany().HasForeignKey(n => n.NguoiTao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CT_PhieuNhapSach>()
                .HasOne(p => p.PhieuNhapSach)
                .WithMany(p => p.CT_PhieuNhapSach) 
                .HasForeignKey(p => p.MaPhieuNhapSach)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CT_PhieuNhapSach>()
                .HasOne(p => p.PhienBanSach) 
                .WithMany()
                .HasForeignKey(p => p.ISBN)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<KhachHang>()
                .HasOne(k => k.LoaiKhachHang).WithMany().HasForeignKey(k => k.MaLoaiKhachHang)
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
                .HasOne(p => p.KhachHang).WithMany().HasForeignKey(p => p.MaKhachHang)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<HoaDon>()
                .HasOne(p => p.NguoiDung).WithMany().HasForeignKey(p => p.NguoiTao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CT_HoaDon>()
                .HasOne(p => p.HoaDon)
                .WithMany(h => h.ChiTietHoaDons) 
                .HasForeignKey(p => p.MaHoaDon)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CT_HoaDon>()
                .HasOne(p => p.PhienBanSach)
                .WithMany()
                .HasForeignKey(p => p.ISBN)
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
                .HasOne(p => p.KhachHang).WithMany().HasForeignKey(p => p.MaKhachHang)
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
               .HasOne(n => n.NhomNguoiDung)                     
               .WithMany(nhom => nhom.NguoiDungs)                
               .HasForeignKey(n => n.MaNhomNguoiDung)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhanQuyen>()
               .HasOne(p => p.NhomNguoiDung)
               .WithMany(nhom => nhom.PhanQuyens)
               .HasForeignKey(p => p.MaNhomNguoiDung)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhanQuyen>()
               .HasOne(p => p.ChucNang)
               .WithMany(c => c.PhanQuyens)
               .HasForeignKey(p => p.MaChucNang)
               .OnDelete(DeleteBehavior.Restrict);


            /// SEED data cho NHÓM NGƯỜI DÙNG
            modelBuilder.Entity<NhomNguoiDung>().HasData(
                new NhomNguoiDung { MaNhomNguoiDung = 1, TenNhomNguoiDung = "ADMIN" },
                new NhomNguoiDung { MaNhomNguoiDung = 2, TenNhomNguoiDung = "NHÂN VIÊN" },
                new NhomNguoiDung { MaNhomNguoiDung = 3, TenNhomNguoiDung = "QUẢN LÝ" }
            );

            /// SEED data cho CHUC NANG
            /// // Note đây kiểm tra lại sau khi có tên các màn hình
            modelBuilder.Entity<ChucNang>().HasData(
                    new ChucNang { MaChucNang = 1, TenChucNang = "Trang chủ", TenManHinh = "DashboardView" },
                    new ChucNang { MaChucNang = 2, TenChucNang = "Bán hàng", TenManHinh = "SaleView" },
                    new ChucNang { MaChucNang = 3, TenChucNang = "Tra cứu sách", TenManHinh = "ProductView" },
                    new ChucNang { MaChucNang = 4, TenChucNang = "Khách hàng", TenManHinh = "CustomerView" },
                    new ChucNang { MaChucNang = 5, TenChucNang = "Nhập kho", TenManHinh = "ImportView" },
                    new ChucNang { MaChucNang = 6, TenChucNang = "Nhà cung cấp", TenManHinh = "SupplierView" },
                    new ChucNang { MaChucNang = 7, TenChucNang = "Ưu đãi", TenManHinh = "PromotionView" },
                    new ChucNang { MaChucNang = 8, TenChucNang = "Danh mục", TenManHinh = "CategoryView" },
                    new ChucNang { MaChucNang = 9, TenChucNang = "Báo cáo", TenManHinh = "ReportView" },
                    new ChucNang { MaChucNang = 10, TenChucNang = "Tài khoản", TenManHinh = "AccountView" },
                    new ChucNang { MaChucNang = 11, TenChucNang = "Cài đặt", TenManHinh = "SettingView" }
            );

            /// SEED data cho PHÂN QUYỀN
            modelBuilder.Entity<PhanQuyen>().HasData(
                // ADMIN có all quyền
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 1 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 2 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 3 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 4 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 5 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 6 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 7 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 8 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 9 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 10 },
                new PhanQuyen { MaNhomNguoiDung = 1, MaChucNang = 11 },

                // QUẢN LÝ không bán hàng, thêm tài khoản và cài đặt
                new PhanQuyen { MaNhomNguoiDung = 3, MaChucNang = 1 },
                new PhanQuyen { MaNhomNguoiDung = 3, MaChucNang = 3 },
                new PhanQuyen { MaNhomNguoiDung = 3, MaChucNang = 4 },
                new PhanQuyen { MaNhomNguoiDung = 3, MaChucNang = 5 },
                new PhanQuyen { MaNhomNguoiDung = 3, MaChucNang = 6 },
                new PhanQuyen { MaNhomNguoiDung = 3, MaChucNang = 7 },
                new PhanQuyen { MaNhomNguoiDung = 3, MaChucNang = 8 },
                new PhanQuyen { MaNhomNguoiDung = 3, MaChucNang = 9 },

                // NHÂN VIÊN có quyền bán hàng, tra cứu sách, quản lý khách hàng.
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 2 },
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 3 },
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 4 },
                new PhanQuyen { MaNhomNguoiDung = 2, MaChucNang = 8 }
            );

            string defaultHash = "f0e65975ed9a43805b030b5e0d4af83239a652b1ddd7fa26b108424e19f48676";
            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung
                {
                    TenDangNhap = "admin",
                    MatKhau = defaultHash,
                    MaNhomNguoiDung = 1,
                    HoTen = "Nguyễn Chí Nguyên",
                    GioiTinh = "Nam",
                    ChucVu = "Quản trị viên",
                    Email = "24521186@gm.uit.edu.vn",
                    DangLamViec = true,
                    NgaySinh = new DateOnly(2006, 3, 10),
                    NgayVaoLam = new DateOnly(2025, 1, 1)
                },
                new NguoiDung
                {
                    TenDangNhap = "hungng",
                    MatKhau = defaultHash,
                    MaNhomNguoiDung = 3,
                    HoTen = "Nguyễn Gia Hưng",
                    GioiTinh = "Nam",
                    ChucVu = "Quản lý Cửa hàng",
                    Email = "24520604@gm.uit.edu.vn",
                    DangLamViec = true,
                    NgaySinh = new DateOnly(2006, 1, 11),
                    NgayVaoLam = new DateOnly(2025, 2, 1)
                },

                new NguoiDung
                {
                    TenDangNhap = "quanlh",
                    MatKhau = defaultHash,
                    MaNhomNguoiDung = 3,
                    HoTen = "Lê Hoàng Quân",
                    GioiTinh = "Nam",
                    ChucVu = "Quản lý Cửa hàng",
                    Email = "24521432@gm.uit.edu.vn",
                    DangLamViec = true,
                    NgaySinh = new DateOnly(2006, 1, 11),
                    NgayVaoLam = new DateOnly(2025, 2, 1)
                },

                new NguoiDung
                {
                    TenDangNhap = "sonph",
                    MatKhau = defaultHash,
                    MaNhomNguoiDung = 3,
                    HoTen = "Phạm Hoàng Sơn",
                    GioiTinh = "Nam",
                    ChucVu = "Quản lý Cửa hàng",
                    Email = "24521536@gm.uit.edu.vn",
                    DangLamViec = true,
                    NgaySinh = new DateOnly(2006, 1, 11),
                    NgayVaoLam = new DateOnly(2025, 2, 1)
                },

                new NguoiDung
                {
                    TenDangNhap = "phunlv",
                    MatKhau = defaultHash,
                    MaNhomNguoiDung = 2,
                    HoTen = "Nguyễn Lưu Văn Phú",
                    GioiTinh = "Nam",
                    ChucVu = "Nhân viên Bán hàng",
                    Email = "24521360@g.uit.edu.vn",
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
                    TenKhachHang = "Khách Vãng Lai",
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
                new Sach { MaSach = 1, TenSach = "Clean Code", MaTheLoai = 1, MoTa = "Sách gối đầu giường của mọi Dev", ImageUrl = "/Resources/Images/Books/cleancode.jpg" },
                new Sach { MaSach = 2, TenSach = "Refactoring", MaTheLoai = 1, MoTa = "Cải thiện thiết kế code cũ", ImageUrl = "/Resources/Images/Books/refactoring.jpg" },
                new Sach { MaSach = 3, TenSach = "Design Patterns", MaTheLoai = 1, MoTa = "Các mẫu thiết kế chuẩn GOF", ImageUrl = "/Resources/Images/Books/designpatterns.jpg" },
                new Sach { MaSach = 4, TenSach = "The Clean Coder", MaTheLoai = 1, MoTa = "Quy tắc hành nghề coder chuyên nghiệp", ImageUrl = "/Resources/Images/Books/cleancoder.jpg" },
                new Sach { MaSach = 5, TenSach = "300 Bài Code Thiếu Nhi", MaTheLoai = 1, MoTa = "Học xong code bao lương 3 ngàn đô", ImageUrl = "/Resources/Images/Books/300baicode.png" },

                // NHÓM VĂN HỌC VIỆT NAM 
                new Sach { MaSach = 6, TenSach = "Mắt Biếc", MaTheLoai = 2, MoTa = "Truyện dài cực hay, tình yêu đau đớn của Ngạn", ImageUrl = "/Resources/Images/Books/matbiec.jpg" },
                new Sach { MaSach = 7, TenSach = "Cho Tôi Xin Một Vé Đi Tuổi Thơ", MaTheLoai = 2, MoTa = "Ký ức tuổi thơ dữ dội", ImageUrl = "/Resources/Images/Books/vetuoitho.jpg" },
                new Sach { MaSach = 8, TenSach = "Số Đỏ", MaTheLoai = 2, MoTa = "Hành trình thăng tiến của Xuân Tóc Đỏ", ImageUrl = "/Resources/Images/Books/sodo.jpg" },
                new Sach { MaSach = 9, TenSach = "Chí Phèo", MaTheLoai = 2, MoTa = "Tuyển tập truyện ngắn Nam Cao", ImageUrl = "/Resources/Images/Books/chipheo.jpg" },
                new Sach { MaSach = 10, TenSach = "Cánh Đồng Bất Tận", MaTheLoai = 2, MoTa = "Nỗi đau trên miền sông nước", ImageUrl = "/Resources/Images/Books/canhdongbattan.jpg" },

                // NHÓM VĂN HỌC NƯỚC NGOÀI
                new Sach { MaSach = 11, TenSach = "Rừng Na Uy", MaTheLoai = 2, MoTa = "Tiểu thuyết nổi tiếng của Haruki Murakami", ImageUrl = "/Resources/Images/Books/rungnauy.jpg" },
                new Sach { MaSach = 12, TenSach = "Kafka Bên Bờ Biển", MaTheLoai = 2, MoTa = "Chuyến phiêu lưu kỳ bí", ImageUrl = "/Resources/Images/Books/kafka.jpg" },
                new Sach { MaSach = 13, TenSach = "Nhà Giả Kim", MaTheLoai = 2, MoTa = "Hành trình đi tìm kho báu của Santiago", ImageUrl = "/Resources/Images/Books/nhagiakim.jpg" },
                new Sach { MaSach = 14, TenSach = "Harry Potter và Hòn Đá Phù Thủy", MaTheLoai = 2, MoTa = "Khởi đầu thế giới phép thuật", ImageUrl = "/Resources/Images/Books/harrypotter1.jpg" },
                new Sach { MaSach = 15, TenSach = "Harry Potter và Phòng Chứa Bí Mật", MaTheLoai = 2, MoTa = "Năm học thứ hai tại Hogwarts", ImageUrl = "/Resources/Images/Books/harrypotter2.jpg" },

                // NHÓM TRUYỆN TRANH
                new Sach { MaSach = 16, TenSach = "Doraemon Tập 1", MaTheLoai = 6, MoTa = "Mèo máy đến từ tương lai", ImageUrl = "/Resources/Images/Books/doraemon1.jpg" },
                new Sach { MaSach = 17, TenSach = "Doraemon Tập 2", MaTheLoai = 6, MoTa = "Những bảo bối thần kỳ", ImageUrl = "/Resources/Images/Books/doraemon2.jpg" },
                new Sach { MaSach = 18, TenSach = "Conan Tập 1", MaTheLoai = 6, MoTa = "Sự khởi đầu của thám tử teo nhỏ", ImageUrl = "/Resources/Images/Books/conan1.jpg" },
                new Sach { MaSach = 19, TenSach = "Conan Tập 2", MaTheLoai = 6, MoTa = "Vụ án mới", ImageUrl = "/Resources/Images/Books/conan2.png" },

                // NHÓM KỸ NĂNG, TÂM LÝ, KINH TẾ 
                new Sach { MaSach = 20, TenSach = "Đắc Nhân Tâm", MaTheLoai = 4, MoTa = "Sách kỹ năng giao tiếp hay nhất", ImageUrl = "/Resources/Images/Books/dacnhantam.jpg" },
                new Sach { MaSach = 21, TenSach = "Quẳng Gánh Lo Đi Và Vui Sống", MaTheLoai = 4, MoTa = "Nghệ thuật sống hạnh phúc", ImageUrl = "/Resources/Images/Books/quangganhlo.jpg" },
                new Sach { MaSach = 22, TenSach = "Giận", MaTheLoai = 4, MoTa = "Làm chủ cảm xúc", ImageUrl = "/Resources/Images/Books/gian.jpg" },
                new Sach { MaSach = 23, TenSach = "Thiện, Ác và Smartphone", MaTheLoai = 4, MoTa = "Tâm lý học trên mạng xã hội", ImageUrl = "/Resources/Images/Books/thienac.jpg" },
                new Sach { MaSach = 24, TenSach = "Trên Đường Băng", MaTheLoai = 3, MoTa = "Khởi nghiệp và kinh doanh", ImageUrl = "/Resources/Images/Books/trenduongbang.jpg" },
                new Sach { MaSach = 25, TenSach = "Cà Phê Cùng Tony", MaTheLoai = 3, MoTa = "Chuyện đời chuyện nghề", ImageUrl = "/Resources/Images/Books/caphecungtony.jpg" }
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


            // BẢNG PHIÊN BẢN SÁCH 
            modelBuilder.Entity<PhienBanSach>().HasData(
                // 
                new PhienBanSach { ISBN = "978-0132350884", MaSach = 1, MaNhaXuatBan = 10, NamXuatBan = 2008, LanTaiBan = 1, HinhThucBia = "Bìa mềm", GiaNiemYet = 450000m, DonGiaBan = 450000m, TonKho = 50, TongSoDaBan = 15 },
                new PhienBanSach { ISBN = "978-0201485677", MaSach = 2, MaNhaXuatBan = 10, NamXuatBan = 2018, LanTaiBan = 2, HinhThucBia = "Bìa cứng", GiaNiemYet = 550000m, DonGiaBan = 520000m, TonKho = 30, TongSoDaBan = 5 },
                new PhienBanSach { ISBN = "978-0201633610", MaSach = 3, MaNhaXuatBan = 10, NamXuatBan = 1994, LanTaiBan = 5, HinhThucBia = "Bìa cứng", GiaNiemYet = 600000m, DonGiaBan = 600000m, TonKho = 20, TongSoDaBan = 2 },
                new PhienBanSach { ISBN = "978-0137081073", MaSach = 4, MaNhaXuatBan = 10, NamXuatBan = 2011, LanTaiBan = 1, HinhThucBia = "Bìa mềm", GiaNiemYet = 350000m, DonGiaBan = 350000m, TonKho = 40, TongSoDaBan = 12 },
                new PhienBanSach { ISBN = "978-604-MEME-01", MaSach = 5, MaNhaXuatBan = 7, NamXuatBan = 2024, LanTaiBan = 1, HinhThucBia = "Bìa mềm", GiaNiemYet = 3000m, DonGiaBan = 3000m, TonKho = 300, TongSoDaBan = 0 }, // 300 bài code bán 3 ngàn đồng!

                //
                new PhienBanSach { ISBN = "978-604-1-09887-1", MaSach = 6, MaNhaXuatBan = 1, NamXuatBan = 2019, LanTaiBan = 15, HinhThucBia = "Bìa mềm", GiaNiemYet = 110000m, DonGiaBan = 110000m, TonKho = 100, TongSoDaBan = 50 },
                new PhienBanSach { ISBN = "978-604-1-09887-2", MaSach = 6, MaNhaXuatBan = 1, NamXuatBan = 2020, LanTaiBan = 1, HinhThucBia = "Bìa cứng kỷ niệm", GiaNiemYet = 250000m, DonGiaBan = 220000m, TonKho = 15, TongSoDaBan = 10 }, // Cảnh báo tồn kho thấp!
                new PhienBanSach { ISBN = "978-604-1-12345-6", MaSach = 7, MaNhaXuatBan = 1, NamXuatBan = 2015, LanTaiBan = 10, HinhThucBia = "Bìa mềm", GiaNiemYet = 85000m, DonGiaBan = 85000m, TonKho = 80, TongSoDaBan = 30 },

                // 
                new PhienBanSach { ISBN = "978-604-6-12301-2", MaSach = 8, MaNhaXuatBan = 6, NamXuatBan = 2018, LanTaiBan = 5, HinhThucBia = "Bìa mềm", GiaNiemYet = 75000m, DonGiaBan = 75000m, TonKho = 45, TongSoDaBan = 10 },
                new PhienBanSach { ISBN = "978-604-6-12302-9", MaSach = 9, MaNhaXuatBan = 6, NamXuatBan = 2017, LanTaiBan = 8, HinhThucBia = "Bìa mềm", GiaNiemYet = 60000m, DonGiaBan = 60000m, TonKho = 60, TongSoDaBan = 25 },
                new PhienBanSach { ISBN = "978-604-1-15555-6", MaSach = 10, MaNhaXuatBan = 1, NamXuatBan = 2010, LanTaiBan = 12, HinhThucBia = "Bìa mềm", GiaNiemYet = 90000m, DonGiaBan = 90000m, TonKho = 55, TongSoDaBan = 40 },

                // 
                new PhienBanSach { ISBN = "978-604-56-7890-1", MaSach = 11, MaNhaXuatBan = 8, NamXuatBan = 2021, LanTaiBan = 5, HinhThucBia = "Bìa mềm", GiaNiemYet = 150000m, DonGiaBan = 145000m, TonKho = 70, TongSoDaBan = 20 },
                new PhienBanSach { ISBN = "978-604-56-7891-8", MaSach = 12, MaNhaXuatBan = 8, NamXuatBan = 2022, LanTaiBan = 3, HinhThucBia = "Bìa mềm", GiaNiemYet = 180000m, DonGiaBan = 175000m, TonKho = 40, TongSoDaBan = 15 },
                new PhienBanSach { ISBN = "978-604-1-23456-7", MaSach = 13, MaNhaXuatBan = 1, NamXuatBan = 2020, LanTaiBan = 20, HinhThucBia = "Bìa mềm", GiaNiemYet = 79000m, DonGiaBan = 79000m, TonKho = 200, TongSoDaBan = 150 },
                new PhienBanSach { ISBN = "978-604-1-34567-8", MaSach = 14, MaNhaXuatBan = 1, NamXuatBan = 2018, LanTaiBan = 10, HinhThucBia = "Bìa mềm", GiaNiemYet = 135000m, DonGiaBan = 130000m, TonKho = 90, TongSoDaBan = 60 },
                new PhienBanSach { ISBN = "978-604-1-45678-9", MaSach = 15, MaNhaXuatBan = 1, NamXuatBan = 2019, LanTaiBan = 8, HinhThucBia = "Bìa mềm", GiaNiemYet = 140000m, DonGiaBan = 135000m, TonKho = 85, TongSoDaBan = 55 },

                // 
                new PhienBanSach { ISBN = "978-604-2-11111-1", MaSach = 16, MaNhaXuatBan = 3, NamXuatBan = 2023, LanTaiBan = 30, HinhThucBia = "Bìa mềm", GiaNiemYet = 20000m, DonGiaBan = 20000m, TonKho = 500, TongSoDaBan = 200 },
                new PhienBanSach { ISBN = "978-604-2-11111-2", MaSach = 17, MaNhaXuatBan = 3, NamXuatBan = 2023, LanTaiBan = 30, HinhThucBia = "Bìa mềm", GiaNiemYet = 20000m, DonGiaBan = 20000m, TonKho = 480, TongSoDaBan = 190 },
                new PhienBanSach { ISBN = "978-604-2-22222-1", MaSach = 18, MaNhaXuatBan = 3, NamXuatBan = 2022, LanTaiBan = 25, HinhThucBia = "Bìa mềm", GiaNiemYet = 22000m, DonGiaBan = 22000m, TonKho = 300, TongSoDaBan = 100 },
                new PhienBanSach { ISBN = "978-604-2-22222-2", MaSach = 19, MaNhaXuatBan = 3, NamXuatBan = 2022, LanTaiBan = 25, HinhThucBia = "Bìa mềm", GiaNiemYet = 22000m, DonGiaBan = 22000m, TonKho = 290, TongSoDaBan = 95 },

                // 
                new PhienBanSach { ISBN = "978-604-4-33333-1", MaSach = 20, MaNhaXuatBan = 4, NamXuatBan = 2021, LanTaiBan = 15, HinhThucBia = "Bìa mềm", GiaNiemYet = 85000m, DonGiaBan = 80000m, TonKho = 150, TongSoDaBan = 80 },
                new PhienBanSach { ISBN = "978-604-4-33333-2", MaSach = 21, MaNhaXuatBan = 4, NamXuatBan = 2020, LanTaiBan = 10, HinhThucBia = "Bìa mềm", GiaNiemYet = 75000m, DonGiaBan = 70000m, TonKho = 120, TongSoDaBan = 50 },
                new PhienBanSach { ISBN = "978-604-4-33333-3", MaSach = 22, MaNhaXuatBan = 4, NamXuatBan = 2019, LanTaiBan = 8, HinhThucBia = "Bìa mềm", GiaNiemYet = 95000m, DonGiaBan = 90000m, TonKho = 60, TongSoDaBan = 30 },
                new PhienBanSach { ISBN = "978-604-4-33333-4", MaSach = 23, MaNhaXuatBan = 4, NamXuatBan = 2022, LanTaiBan = 2, HinhThucBia = "Bìa mềm", GiaNiemYet = 120000m, DonGiaBan = 115000m, TonKho = 80, TongSoDaBan = 20 },
                new PhienBanSach { ISBN = "978-604-1-55555-1", MaSach = 24, MaNhaXuatBan = 1, NamXuatBan = 2017, LanTaiBan = 12, HinhThucBia = "Bìa mềm", GiaNiemYet = 85000m, DonGiaBan = 85000m, TonKho = 100, TongSoDaBan = 150 },
                new PhienBanSach { ISBN = "978-604-1-55555-2", MaSach = 25, MaNhaXuatBan = 1, NamXuatBan = 2016, LanTaiBan = 15, HinhThucBia = "Bìa mềm", GiaNiemYet = 75000m, DonGiaBan = 75000m, TonKho = 90, TongSoDaBan = 140 }
            );


            // BẢNG NHÀ CUNG CẤP
            modelBuilder.Entity<NhaCungCap>().HasData(
                new NhaCungCap
                {
                    MaNhaCungCap = 1,
                    TenNhaCungCap = "Công ty CP Phát hành sách FAHASA",
                    DiaChi = "387-389 Hai Bà Trưng, Quận 3, TP.HCM",
                    MaSoThue = "0300435133",
                    SoDienThoai = "1900636467",
                    Email = "info@fahasa.com",
                    NganHang = "Vietcombank",
                    SoTaiKhoan = "0071000123456"
                },
                new NhaCungCap
                {
                    MaNhaCungCap = 2,
                    TenNhaCungCap = "Nhà sách Phương Nam",
                    DiaChi = "212 Nguyễn Trãi, Quận 1, TP.HCM",
                    MaSoThue = "0302221113",
                    SoDienThoai = "1900555555",
                    Email = "contact@phuongnam.com",
                    NganHang = "Techcombank",
                    SoTaiKhoan = "1901234567890"
                }
            );

            // BẢNG PHIẾU NHẬP SÁCH
            modelBuilder.Entity<PhieuNhapSach>().HasData(
                // Phiếu nhập từ FAHASA (Tổng tiền: 22,000,000)
                new PhieuNhapSach
                {
                    MaPhieuNhapSach = 1,
                    NgayTao = new DateTime(2024, 3, 1, 9, 0, 0),
                    NguoiTao = "admin", 
                    MaNhaCungCap = 1,
                    TongTien = 22000000m
                },

                // Phiếu nhập từ Phương Nam (Tổng tiền: 3,000,000)
                new PhieuNhapSach
                {
                    MaPhieuNhapSach = 2,
                    NgayTao = new DateTime(2024, 4, 15, 14, 30, 0),
                    NguoiTao = "hungng",
                    MaNhaCungCap = 2,
                    TongTien = 3000000m
                }
            );

            // BẢNG CHI TIẾT PHIẾU NHẬP SÁCH
            modelBuilder.Entity<CT_PhieuNhapSach>().HasData(
                // ---- Chi tiết cho Phiếu Nhập 1 ----
                // Nhập 50 cuốn Clean Code x 300k = 15,000,000
                new CT_PhieuNhapSach
                {
                    MaPhieuNhapSach = 1,
                    ISBN = "978-0132350884",
                    SoLuong = 50,
                    DonGiaNhap = 300000m
                },
                // Nhập 100 cuốn Mắt Biếc x 70k = 7,000,000
                // (15tr + 7tr = 22tr
                new CT_PhieuNhapSach
                {
                    MaPhieuNhapSach = 1,
                    ISBN = "978-604-1-09887-1",
                    SoLuong = 100,
                    DonGiaNhap = 70000m
                },

                // ---- Chi tiết cho Phiếu Nhập 2 ----
                // Nhập 200 cuốn Doraemon x 15k = 3,000,000 
                new CT_PhieuNhapSach
                {
                    MaPhieuNhapSach = 2,
                    ISBN = "978-604-2-11111-1",
                    SoLuong = 200,
                    DonGiaNhap = 15000m
                }
            );

            /// Những bạn liên quan đến ưu đãi
            modelBuilder.Entity<LoaiUuDai>().HasData(
                new LoaiUuDai { MaLoaiUuDai = 1, TenLoaiUuDai = "Giảm giá Hóa đơn", ApDungToiDa = 1 },
                new LoaiUuDai { MaLoaiUuDai = 2, TenLoaiUuDai = "Tặng quà theo Hóa đơn", ApDungToiDa = 1 },
                new LoaiUuDai { MaLoaiUuDai = 3, TenLoaiUuDai = "Giảm giá trực tiếp trên Sách", ApDungToiDa = 5 },
                new LoaiUuDai { MaLoaiUuDai = 4, TenLoaiUuDai = "Tặng sách khi mua Sách", ApDungToiDa = 5 }
            );

            modelBuilder.Entity<UuDai>().HasData(
                // Giảm 10% (Tối đa 100k) cho hóa đơn từ 500k
                new UuDai { MaUuDai = 1, NgayTao = new DateTime(2024, 1, 1), NguoiTao = "admin", MaLoaiUuDai = 1, TenUuDai = "Giảm 10% Hóa đơn > 500k", MoTa = "Chương trình kích cầu", NgayBatDau = new DateTime(2024, 1, 1), NgayKetThuc = new DateTime(2025, 12, 31), SoLuongToiDa = 1000, SoLuongDaDung = 0, MaLoaiKhachHang = 1, CoTheSuDung = true },

                // Hóa đơn từ 1 Triệu tặng cuốn "Đắc Nhân Tâm"
                new UuDai { MaUuDai = 2, NgayTao = new DateTime(2024, 1, 1), NguoiTao = "quanlh", MaLoaiUuDai = 2, TenUuDai = "Hóa đơn 1Tr tặng Đắc Nhân Tâm", MoTa = "Tri ân khách VIP", NgayBatDau = new DateTime(2024, 1, 1), NgayKetThuc = new DateTime(2025, 12, 31), SoLuongToiDa = 50, SoLuongDaDung = 0, MaLoaiKhachHang = 2, CoTheSuDung = true },

                // Giảm 20k trực tiếp khi mua cuốn "Mắt Biếc"
                new UuDai { MaUuDai = 3, NgayTao = new DateTime(2024, 1, 1), NguoiTao = "hungng", MaLoaiUuDai = 3, TenUuDai = "Giảm 20k Mắt Biếc", MoTa = "Sale sách Hot", NgayBatDau = new DateTime(2024, 1, 1), NgayKetThuc = new DateTime(2025, 12, 31), SoLuongToiDa = 200, SoLuongDaDung = 0, MaLoaiKhachHang = 1, CoTheSuDung = true },

                // Mua 2 cuốn "Clean Code" tặng 1 cuốn "300 Bài Code"
                new UuDai { MaUuDai = 4, NgayTao = new DateTime(2024, 1, 1), NguoiTao = "sonph", MaLoaiUuDai = 4, TenUuDai = "Combo Dev: Mua 2 tặng 1", MoTa = "Đồng hành cùng IT", NgayBatDau = new DateTime(2024, 1, 1), NgayKetThuc = new DateTime(2025, 12, 31), SoLuongToiDa = 100, SoLuongDaDung = 0, MaLoaiKhachHang = 1, CoTheSuDung = true }
            );

            // ct giảm theo hd
            modelBuilder.Entity<CTUD_HoaDon_Giam>().HasData(
                new CTUD_HoaDon_Giam { MaCT = 1, MaUuDai = 1, SoTienToiThieu = 500000m, SoTienToiDa = 999999999m, SoTienGiam = 0m, TiLeGiam = 0.1f, GiamToiDa = 100m}
            );

            // ct tặng quà theo hd
            modelBuilder.Entity<CTUD_HoaDon_Qua>().HasData(
                new CTUD_HoaDon_Qua { MaCT = 1, MaUuDai = 2, SoTienToiThieu = 1000000m, SoTienToiDa = 999999999m }
            );
            modelBuilder.Entity<UuDai_SachTang>().HasData(
                new UuDai_SachTang { MaUuDai = 2, ISBN = "978-604-4-33333-1", SoLuongTang = 1 } // Đắc Nhân Tâm
            );

            /// ct giảm tía theo sách
            modelBuilder.Entity<CTUD_Sach_Giam>().HasData(
                new CTUD_Sach_Giam { MaCT = 1, MaUuDai = 3, SoTienGiam = 20000m, TiLeGiam = 0, GiamToiDa = 20000m }
            );
            modelBuilder.Entity<UuDai_SachDieuKien>().HasData(
                new UuDai_SachDieuKien { MaUuDai = 3, ISBN = "978-604-1-09887-1", SoLuongMua = 1 } // Mắt Biếc bìa mềm
            );

            // ct mua sách tặng sách
            modelBuilder.Entity<CTUD_Sach_Qua>().HasData(
                new CTUD_Sach_Qua { MaCT = 1, MaUuDai = 4 }
            );
            modelBuilder.Entity<UuDai_SachDieuKien>().HasData(
                new UuDai_SachDieuKien { MaUuDai = 4, ISBN = "978-0132350884", SoLuongMua = 2 } // Điều kiện: Mua 2 cuốn Clean Code
            );
            modelBuilder.Entity<UuDai_SachTang>().HasData(
                new UuDai_SachTang { MaUuDai = 4, ISBN = "978-604-MEME-01", SoLuongTang = 1 } // Quà tặng: 1 cuốn 300 Bài code thíu nhi
            );

            // BẢNG HÓA ĐƠN 
            modelBuilder.Entity<HoaDon>().HasData(
                // Hóa đơn 1: Khách lẻ mua 1 cuốn Mắt Biếc
                // Tạm tính: 110k, Giảm giá: 20k (UuDai 3), Phải trả: 90k, Đã trả: 90k (Không nợ)
                new HoaDon { MaHoaDon = 1, NgayTao = new DateTime(2024, 5, 1, 8, 30, 0), NguoiTao = "phunlv", MaKhachHang = 1, TongTienTamTinh = 110000m, GiamGia = 20000m, Thue = 0m, TongTien = 90000m, SoTienTra = 90000m },

                // Hóa đơn 2: mua 2 cuốn Clean Code
                // Tạm tính: 900k, Giảm giá: 0 (Được tặng quà), Phải trả: 900k, Đã trả: 650k (Nợ 250k)
                new HoaDon { MaHoaDon = 2, NgayTao = new DateTime(2024, 5, 2, 14, 15, 0), NguoiTao = "phunlv", MaKhachHang = 2, TongTienTamTinh = 900000m, GiamGia = 0m, Thue = 0m, TongTien = 900000m, SoTienTra = 650000m },

                // Hóa đơn 3: Cty Sahara (MaKhachHang = 3) mua nhiều, hóa đơn > 500k
                // Tạm tính: 1000k, Giảm giá 10% (UuDai 1) = 100k, Phải trả: 900k, Đã trả: 900k (Không nợ)
                new HoaDon { MaHoaDon = 3, NgayTao = new DateTime(2024, 5, 3, 10, 0, 0), NguoiTao = "phunlv", MaKhachHang = 3, TongTienTamTinh = 1000000m, GiamGia = 100000m, Thue = 0m, TongTien = 900000m, SoTienTra = 900000m }
            );

            // CHI TIẾT HÓA ĐƠN
            modelBuilder.Entity<CT_HoaDon>().HasData(
                // Chi tiết HD 1: 1 Mắt biếc (Giá bán 110k)
                new CT_HoaDon { MaHoaDon = 1, ISBN = "978-604-1-09887-1", SoLuong = 1, DonGia = 110000m, GiaVon = 100000m },

                // Chi tiết HD 2: 2 Clean Code (Giá bán 450k/cuốn)
                new CT_HoaDon { MaHoaDon = 2, ISBN = "978-0132350884", SoLuong = 2, DonGia = 450000m, GiaVon = 440000m},

                // Chi tiết HD 3: 5 Doraemon tập 1 (20k/cuốn) + 1 Rừng Na Uy (145k)
                new CT_HoaDon { MaHoaDon = 3, ISBN = "978-604-2-11111-1", SoLuong = 5, DonGia = 20000m, GiaVon = 18000m },
                new CT_HoaDon { MaHoaDon = 3, ISBN = "978-604-56-7890-1", SoLuong = 6, DonGia = 150000m, GiaVon = 140000m }
            );

            // HÓA ĐƠN - ƯU ĐÃI
            modelBuilder.Entity<HoaDon_UuDai>().HasData(
                // HD 1 được áp dụng UuDai 3: Giảm 20k trực tiếp cho cuốn Mắt Biếc
                new HoaDon_UuDai { MaCT_HoaDon_UuDai = 1, MaHoaDon = 1, MaUuDai = 3, ISBN = "978-604-1-09887-1", SoTienGiam = 20000m },

                // HD 2 được áp dụng UuDai 4: Tặng 1 cuốn "300 Bài Code" (Giảm giá = 0 vì là quà tặng)
                new HoaDon_UuDai { MaCT_HoaDon_UuDai = 2, MaHoaDon = 2, MaUuDai = 4, ISBN = "978-604-MEME-01", SoTienGiam = 0m },

                // HD 3 được áp dụng UuDai 1: Giảm 10% tổng hóa đơn (Tối đa 100k)
                new HoaDon_UuDai { MaCT_HoaDon_UuDai = 3, MaHoaDon = 3, MaUuDai = 1, ISBN = null, SoTienGiam = 100000m }
            );

            // PHIẾU THU TIỀN 
            modelBuilder.Entity<PhieuThuTien>().HasData(
                new PhieuThuTien { MaPhieuThuTien = 1, NgayTao = new DateTime(2024, 5, 5, 17, 0, 0), NguoiTao = "phunlv", MaKhachHang = 2, SoTienThu = 100000m }
            );

            // BẢNG BÁO CÁO TỒN KHO VÀ DOANH THU SÁCH (BC_Sach)
            modelBuilder.Entity<BC_Sach>().HasData(
                // Tháng 3: Nhập hàng 22tr, chưa bán
                new BC_Sach { MaBaoCaoSach = 1, Thang = 3, Nam = 2024, TongDoanhThu = 0m, TongChiPhi = 22000000m },
                // Tháng 4: Nhập hàng 3tr, chưa bá
                new BC_Sach { MaBaoCaoSach = 2, Thang = 4, Nam = 2024, TongDoanhThu = 0m, TongChiPhi = 3000000m },
                // Tháng 5: Bán hàng, doanh thu 1.890.000đ, không nhập thêm
                new BC_Sach { MaBaoCaoSach = 3, Thang = 5, Nam = 2024, TongDoanhThu = 1890000m, TongChiPhi = 0m }
            );

            //CHI TIẾT BÁO CÁO SÁCH (CT_BC_Sach)
            modelBuilder.Entity<CT_BC_Sach>().HasData(
                // --- THÁNG 3: Nhập lô đầu tiên (Clean Code, Mắt Biếc) ---
                new CT_BC_Sach { MaBaoCaoSach = 1, ISBN = "978-0132350884", DoanhThu = 0m, ChiPhiNhap = 15000000m, TonDau = 0, TongNhap = 50, TongXuat = 0, TonCuoi = 50 },
                new CT_BC_Sach { MaBaoCaoSach = 1, ISBN = "978-604-1-09887-1", DoanhThu = 0m, ChiPhiNhap = 7000000m, TonDau = 0, TongNhap = 100, TongXuat = 0, TonCuoi = 100 },

                // --- THÁNG 4: Nhập thêm Doraemon --
                new CT_BC_Sach { MaBaoCaoSach = 2, ISBN = "978-0132350884", DoanhThu = 0m, ChiPhiNhap = 0m, TonDau = 50, TongNhap = 0, TongXuat = 0, TonCuoi = 50 },
                new CT_BC_Sach { MaBaoCaoSach = 2, ISBN = "978-604-1-09887-1", DoanhThu = 0m, ChiPhiNhap = 0m, TonDau = 100, TongNhap = 0, TongXuat = 0, TonCuoi = 100 },
                new CT_BC_Sach { MaBaoCaoSach = 2, ISBN = "978-604-2-11111-1", DoanhThu = 0m, ChiPhiNhap = 3000000m, TonDau = 0, TongNhap = 200, TongXuat = 0, TonCuoi = 200 },

                // --- THÁNG 5: bắt đầu bán hàng ---
                new CT_BC_Sach { MaBaoCaoSach = 3, ISBN = "978-0132350884", DoanhThu = 900000m, ChiPhiNhap = 0m, TonDau = 50, TongNhap = 0, TongXuat = 2, TonCuoi = 48 },
                new CT_BC_Sach { MaBaoCaoSach = 3, ISBN = "978-604-1-09887-1", DoanhThu = 90000m, ChiPhiNhap = 0m, TonDau = 100, TongNhap = 0, TongXuat = 1, TonCuoi = 99 },
                new CT_BC_Sach { MaBaoCaoSach = 3, ISBN = "978-604-2-11111-1", DoanhThu = 100000m, ChiPhiNhap = 0m, TonDau = 200, TongNhap = 0, TongXuat = 5, TonCuoi = 195 }
            );

            // BÁO CÁO CÔNG NỢ VÀ DOANH THU KHÁCH HÀNG (BC_KhachHang)
            modelBuilder.Entity<BC_KhachHang>().HasData(
                new BC_KhachHang { MaBaoCaoKhachHang = 1, Thang = 3, Nam = 2024, TongDoanhThu = 0m, TongNo = 0m },
                new BC_KhachHang { MaBaoCaoKhachHang = 2, Thang = 4, Nam = 2024, TongDoanhThu = 0m, TongNo = 0m },
                // Tháng 5: Tổng doanh thu 1.890.000đ, Tổng dư nợ của các khách hàng còn lại 150.000đ
                new BC_KhachHang { MaBaoCaoKhachHang = 3, Thang = 5, Nam = 2024, TongDoanhThu = 1890000m, TongNo = 150000m }
            );

            ///CHI TIẾT BÁO CÁO KHÁCH HÀNG (CT_BC_KhachHang)
            modelBuilder.Entity<CT_BC_KhachHang>().HasData(
                // --- THÁNG 5:
                // Khách lẻ
                new CT_BC_KhachHang { MaBaoCaoKhachHang = 3, MaKhachHang = 1, SoHoaDon = 1, DoanhThu = 90000m, TiLeDoanhThu = 4.76f, NoDau = 0m, NoPhatSinh = 0m, DaTra = 0m, NoCuoi = 0m },
                // Khách quen 
                new CT_BC_KhachHang { MaBaoCaoKhachHang = 3, MaKhachHang = 2, SoHoaDon = 1, DoanhThu = 900000m, TiLeDoanhThu = 47.62f, NoDau = 0m, NoPhatSinh = 250000m, DaTra = 100000m, NoCuoi = 150000m },
                // Cty Sahara (
                new CT_BC_KhachHang { MaBaoCaoKhachHang = 3, MaKhachHang = 3, SoHoaDon = 1, DoanhThu = 900000m, TiLeDoanhThu = 47.62f, NoDau = 0m, NoPhatSinh = 0m, DaTra = 0m, NoCuoi = 0m }
            );
        }
    }
}