using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.WPF.Services;
using Bookstore.Share;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Bookstore.WPF.ViewModels
{
    public class DashBoardViewModel : BaseViewModel
    {
        public ObservableCollection<Book> TopBooks { get; set; }
        public ObservableCollection<Receipt> Items { get; set; }
        public ObservableCollection<InventoryItem> InventoryItems { get; set; }
        public ObservableCollection<StockWarning> StockWarnings { get; set; }
        public PieData[] Data { get; set; } = [
            new("Mary", 10),
            new("John", 20),
            new("Alice", 30),
            new("Bob", 40),
            new("Charlie", 50)
        ];
        public ISeries[] PieSeries { get; set; }
        public float Profit { get; set; } = 1000000; // Lợi nhuận mẫu
        public float ProfitPercent { get; set; } = 15; // Tỷ lệ phần trăm lợi nhuận mẫu
        public float Sale { get; set; } = 5000000; // Doanh số mẫu
        public int CustNum { get; set; } = 200; // Số lượng khách hàng mẫu
        public float Expense { get; set; } = 4000000; // Chi phí mẫu
        public int ReceiptNum { get; set; } = 150; // Số lượng hóa đơn mẫu

        public DashBoardViewModel()
        {
            

            // Chuyển đổi từ danh sách Data sang mảng ISeries của LiveCharts2
            PieSeries = new ISeries[]
            {
            new PieSeries<double> { Values = new double[] { 40 }, Name = "Fiction" },
            new PieSeries<double> { Values = new double[] { 30 }, Name = "Non-Fiction" },
            new PieSeries<double> { Values = new double[] { 20 }, Name = "Science" },
            new PieSeries<double> { Values = new double[] { 10 }, Name = "History" }
            };

            // Dữ liệu mẫu cho Top 5 sách bán chạy
            TopBooks = new ObservableCollection<Book>
            {
                new Book { Rank = 1, BookImage = "D:\\UIT\\SE104\\Bookstore-Management-Windows-App\\Bookstore.WPF\\Resources\\Images\\book1.png" },
                new Book { Rank = 2, BookImage = "D:\\UIT\\SE104\\Bookstore-Management-Windows-App\\Bookstore.WPF\\Resources\\Images\\book2.png" },
                new Book { Rank = 3, BookImage = "D:\\UIT\\SE104\\Bookstore-Management-Windows-App\\Bookstore.WPF\\Resources\\Images\\book3.png" },
                new Book { Rank = 4, BookImage = "D:\\UIT\\SE104\\Bookstore-Management-Windows-App\\Bookstore.WPF\\Resources\\Images\\book4.png" },
                new Book { Rank = 5, BookImage = "D:\\UIT\\SE104\\Bookstore-Management-Windows-App\\Bookstore.WPF\\Resources\\Images\\book5.png" }
            };

            // Dữ liệu mẫu cho bảng hóa đơn
            Items = new ObservableCollection<Receipt>
            {
                new Receipt { ReceiptNum = "001", CustomerName = "John Doe", CasherName = "Jane Smith", Date = "2023-10-01", TotalCost = "500,000đ" },
                new Receipt { ReceiptNum = "002", CustomerName = "Alice Brown", CasherName = "Tom White", Date = "2023-10-02", TotalCost = "300,000đ" },
                new Receipt { ReceiptNum = "003", CustomerName = "Charlie Green", CasherName = "Emma Black", Date = "2023-10-03", TotalCost = "700,000đ" }
            };

            // Dữ liệu mẫu cho thông tin hàng nhập
            InventoryItems = new ObservableCollection<InventoryItem>
            {
                new InventoryItem { Name = "Book A", BrandName = "Brand X", Date = "2023-10-01", Number = 100 },
                new InventoryItem { Name = "Book B", BrandName = "Brand Y", Date = "2023-10-02", Number = 50 },
                new InventoryItem { Name = "Book C", BrandName = "Brand Z", Date = "2023-10-03", Number = 75 }
            };

            // Dữ liệu mẫu cho cảnh báo tồn kho
            StockWarnings = new ObservableCollection<StockWarning>
            {
                new StockWarning { Name = "Book D",BrandName = "Brand X", RemainingQuantity = 5 },
                new StockWarning { Name = "Book E",BrandName = "Brand X", RemainingQuantity = 2 },
                new StockWarning { Name = "Book F",BrandName = "Brand Y", RemainingQuantity = 0 }
            };
        }
    }
    public class PieData(string name, double value)
    {
        public string Name { get; set; } = name;
        public double[] Values { get; set; } = new double[] { value };
    }
    public class BookGenreSale
    {
        public string Name { get; set; }
        public double[] Values { get; set; }
        public double Pushout { get; set; }
    }

    public class Book
    {
        public int Rank { get; set; }
        public string BookImage { get; set; }
    }

    public class Receipt
    {
        public string ReceiptNum { get; set; }
        public string CustomerName { get; set; }
        public string CasherName { get; set; }
        public string Date { get; set; }
        public string TotalCost { get; set; }
    }

    public class InventoryItem
    {
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Date { get; set; }
        public int Number { get; set; }
    }

    public class StockWarning
    {
        public string Name { get; set; }
        public string BrandName { get; set; }
        public int RemainingQuantity { get; set; }
    }
}
