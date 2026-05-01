using Bookstore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Bookstore.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Stock> Positions { get; set; }
        public DbSet<BookCount> BookCounts { get; set; }
        public DbSet<ImportReceipt> ImportReceipts { get; set; }
        public DbSet<ImportDetail> ImportDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Accounts { get; set; }
        public DbSet<MonthlyCustomerReport> MonthlyDebtReports { get; set; }
        public DbSet<MonthlyBookReport> MonthlyStockReports { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<StockTransferDetail> StockTransferDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Primary keys
            modelBuilder.Entity<Book>().HasKey(k => k.BookID);
            modelBuilder.Entity<Category>().HasKey(k => k.CategoryID);
            modelBuilder.Entity<Supplier>().HasKey(k => k.SupplierID);
            modelBuilder.Entity<Stock>().HasKey(k => k.StockID);
            modelBuilder.Entity<BookCount>().HasKey(k => k.BookCountID);
            modelBuilder.Entity<StockTransfer>().HasKey(k => k.TransferID);
            modelBuilder.Entity<StockTransferDetail>().HasKey(k => k.StockTransferDetailID);
            modelBuilder.Entity<ImportReceipt>().HasKey(k => k.ImportReceiptID);
            modelBuilder.Entity<ImportDetail>().HasKey(k => k.ImportDetailID);
            modelBuilder.Entity<Customer>().HasKey(k => k.CustomerID);
            modelBuilder.Entity<User>().HasKey(k => k.UserID);
            modelBuilder.Entity<Promotion>().HasKey(k => k.PromotionCode);
            modelBuilder.Entity<Invoice>().HasKey(k => k.InvoiceID);
            modelBuilder.Entity<InvoiceDetail>().HasKey(k => k.InvoiceDetailID);
            modelBuilder.Entity<Receipt>().HasKey(k => k.ReceiptID);
            modelBuilder.Entity<MonthlyCustomerReport>().HasKey(k => k.ReportID);
            modelBuilder.Entity<MonthlyBookReport>().HasKey(k => k.ReportID);

            // Unique
            modelBuilder.Entity<Book>().HasIndex(b => b.ISBN).IsUnique();
            modelBuilder.Entity<Customer>().HasIndex(c => c.PhoneNumber).IsUnique();
            modelBuilder.Entity<User>().HasIndex(a => a.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(a => a.Email).IsUnique();
            modelBuilder.Entity<Promotion>().HasIndex(p => p.Code).IsUnique();

            // Forgein keys
            // Sách & Kho
            modelBuilder.Entity<Book>().HasOne<Category>().WithMany().HasForeignKey(b => b.CategoryID);
            modelBuilder.Entity<BookCount>().HasOne<Book>().WithMany().HasForeignKey(bc => bc.BookID);
            modelBuilder.Entity<BookCount>().HasOne<Stock>().WithMany().HasForeignKey(bc => bc.StockID);

            // Chuyển kho 
            modelBuilder.Entity<StockTransfer>().HasOne<Stock>().WithMany().HasForeignKey(st => st.FromStockID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StockTransfer>().HasOne<Stock>().WithMany().HasForeignKey(st => st.ToStockID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StockTransferDetail>().HasOne<StockTransfer>().WithMany().HasForeignKey(std => std.TransferID);
            modelBuilder.Entity<StockTransferDetail>().HasOne<Book>().WithMany().HasForeignKey(std => std.BookID);

            // Nhập hàng
            modelBuilder.Entity<ImportReceipt>().HasOne<Supplier>().WithMany().HasForeignKey(ir => ir.SupplierID);
            modelBuilder.Entity<ImportReceipt>().HasOne<Stock>().WithMany().HasForeignKey(ir => ir.StockID);
            modelBuilder.Entity<ImportDetail>().HasOne<ImportReceipt>().WithMany().HasForeignKey(id => id.ImportReceiptID);
            modelBuilder.Entity<ImportDetail>().HasOne<Book>().WithMany().HasForeignKey(id => id.BookID);

            // Khách hàng
            modelBuilder.Entity<Customer>().HasOne<User>().WithOne().HasForeignKey<Customer>(c => c.AccountID).IsRequired(false);
            

            // Ưu đãi 
            modelBuilder.Entity<Promotion>().HasOne<Book>().WithMany().HasForeignKey(p => p.RequiredBookID).IsRequired(false);
            modelBuilder.Entity<Promotion>().HasOne<Book>().WithMany().HasForeignKey(p => p.GiftBookID).IsRequired(false);

            // Hóa đơn & Thanh toán
            modelBuilder.Entity<Invoice>().HasOne<Customer>().WithMany().HasForeignKey(i => i.CustomerID).IsRequired(false);
            modelBuilder.Entity<Invoice>().HasOne<Promotion>().WithMany().HasForeignKey(i => i.PromotionID).IsRequired(false);
            modelBuilder.Entity<InvoiceDetail>().HasOne<Invoice>().WithMany().HasForeignKey(id => id.InvoiceID);
            modelBuilder.Entity<InvoiceDetail>().HasOne<Book>().WithMany().HasForeignKey(id => id.BookID);
            modelBuilder.Entity<InvoiceDetail>().HasOne<Stock>().WithMany().HasForeignKey(id => id.StockID);
            modelBuilder.Entity<InvoiceDetail>().HasOne<Promotion>().WithMany().HasForeignKey(id => id.PromotionID).IsRequired(false);

            modelBuilder.Entity<Receipt>().HasOne<Customer>().WithMany().HasForeignKey(r => r.CustomerID).IsRequired(false);
            modelBuilder.Entity<Receipt>().HasOne<Invoice>().WithMany().HasForeignKey(r => r.InvoiceID).IsRequired(false);

            // Nhóm Báo cáo
            modelBuilder.Entity<MonthlyCustomerReport>().HasOne<Customer>().WithMany().HasForeignKey(m => m.CustomerID);
            modelBuilder.Entity<MonthlyBookReport>().HasOne<Stock>().WithMany().HasForeignKey(m => m.StockID);
            modelBuilder.Entity<MonthlyBookReport>().HasOne<Book>().WithMany().HasForeignKey(m => m.BookID);

            // sEeD dAtA dE tEsT tHu ApI

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Username = "admin",
                    PasswordHash = "fa980dbf4533c98fa5ed792374bea691610dfaabc62558182f4cc814ef0d69db",
                    Email = "nathannguyen6002@gmail.com",
                    RoleID = 0,
                },
                new User
                {
                    UserID = 2,
                    Username = "staff",
                    PasswordHash = "fa980dbf4533c98fa5ed792374bea691610dfaabc62558182f4cc814ef0d69db",
                    Email = "24521186@gm.uit.edu.vn",
                    RoleID = 1,
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Truyện tranh" },
                new Category { CategoryID = 2, CategoryName = "Giáo trình" },
                new Category { CategoryID = 3, CategoryName = "Kỹ năng sống" },
                new Category { CategoryID = 4, CategoryName = "Văn học" },
                new Category { CategoryID = 5, CategoryName = "Kinh tế" }
            );

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    SupplierID = 1,
                    SupplierName = "NXB Kim Đồng",
                    Address = "55 Quang Trung, Hà Nội",
                    Email = "cskh@nxbkimdong.com.vn",
                    Phonenumber = "1900571595"
                },
                new Supplier
                {
                    SupplierID = 2,
                    SupplierName = "NXB Trẻ",
                    Address = "161B Lý Chính Thắng, Quận 3, TP.HCM",
                    Email = "hopthu@nxbtre.com.vn",
                    Phonenumber = "02839316289"
                }
            );

            modelBuilder.Entity<Stock>().HasData(
                new Stock
                {
                    StockID = 1,
                    StockName = "Dãy A",
                    Priority = 0,
                    Description = "Kệ chính, đối diện cửa ra vào"
                },
                new Stock
                {
                    StockID = 2,
                    StockName = "Kho tổng 1",
                    Priority = 1,
                    Description = "Kho tầng 1, lưu trữ hàng mới nhập về"
                },
                new Stock
                {
                    StockID = 3,
                    StockName = "Kho tổng 2",
                    Priority = 1,
                    Description = "Kho tầng 2, lưu trữ hàng tồn kho, chưa xả được"
                }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookID = 1,
                    ISBN = "978-604-2-27297-1",
                    Title = "Conan Tập 100",
                    Author = "Aoyama Gosho",
                    Publisher = "NXB Kim Đồng",
                    Edition = 1,
                    CategoryID = 1,
                    Price = 25000m,
                    ListPrice = 25000m,
                    ImportedPrice = 18000m, // Giá vốn tham chiếu
                    StockQuantity = 100,
                    IsDeleted = false,
                    CreatedBy = 1 // Admin tạo
                },
                new Book
                {
                    BookID = 2,
                    ISBN = "978-604-1-18321-4",
                    Title = "Lập trình C#",
                    Author = "Nguyễn Hữu Hùng",
                    Publisher = "NXB Trẻ",
                    Edition = 5,
                    CategoryID = 2,
                    Price = 120000m,
                    ListPrice = 120000m,
                    ImportedPrice = 85000m,
                    StockQuantity = 20,
                    IsDeleted = false,
                    CreatedBy = 1
                }
            );

            modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                CustomerID = 1,
                CustomerName = "Nguyễn Gia Hưng",
                Gender = 0,
                PhoneNumber = "0901234567",
                Address = "Cần Thơ",
                TotalDebt = 0,
                TotalPurchaseValue = 0,
                TotalOrders = 0,
                AccountID = null // vãng lai lãi vang
            }
        );
        }
    }
}