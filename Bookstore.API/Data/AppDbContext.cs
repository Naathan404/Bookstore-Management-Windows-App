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
        }
    }
}