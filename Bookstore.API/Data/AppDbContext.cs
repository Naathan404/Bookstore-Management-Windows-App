using Bookstore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "Conan Tập 100", Author = "Aoyama Gosho", Category = "Truyện tranh", Price = 25000, StockQuantity = 50 },
                new Book { BookId = 2, Title = "Lập trình C#", Author = "NXB Trẻ", Category = "Giáo trình", Price = 120000, StockQuantity = 20 }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Nguyễn Gia Hưng", Address = "Cần Thơ" }
            );
        }
    }
}
