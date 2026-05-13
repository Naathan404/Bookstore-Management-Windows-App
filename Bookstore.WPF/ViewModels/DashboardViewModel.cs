using Bookstore.WPF.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        // ==========================================
        // Tab 1
        private decimal _sale;
        public decimal Sale // Doanh thu 
        {
            get => _sale;
            set { _sale = value; OnPropertyChanged(nameof(Sale)); }
        }

        private decimal _profit;
        public decimal Profit // Lợi nhuận 
        {
            get => _profit;
            set { _profit = value; OnPropertyChanged(nameof(Profit)); }
        }

        private int _custNum;
        
        public int CustNum // Khách mới 
        {
            get => _custNum;
            set { _custNum = value; OnPropertyChanged(nameof(CustNum)); }
        }

        private int _receiptNum;
        
        public int ReceiptNum // Đơn hàng 
        {
            get => _receiptNum;
            set { _receiptNum = value; OnPropertyChanged(nameof(ReceiptNum)); }
        }

        // ==========================================
        // BIỂU ĐỒ 

       // Biểu đồ xu hướng doanh thu 
        private ISeries[] _revenueSeries;
        public ISeries[] RevenueSeries
        {
            get => _revenueSeries;
            set { _revenueSeries = value; OnPropertyChanged(nameof(RevenueSeries)); }
        }

        // Biểu đồ tỷ trọng thể loại 
        //private ISeries[] _data;
        //public ISeries[] Data
        //{
        //    get => _data;
        //    set { _data = value; OnPropertyChanged(nameof(Data)); }
        //}

        private ObservableCollection<ISeries> _data;
        public ObservableCollection<ISeries> Data
        {
            get => _data;
            set { _data = value; OnPropertyChanged(nameof(Data)); }
        }

        // ==========================================
        // DANH SÁCH & BẢNG BIỂU 

        // Tab 1: Top 5 sách bán chạy
        public ObservableCollection<TopBookModel> TopBooks { get; set; }

        // Tab 1: Top Khách hàng VIP
        public ObservableCollection<CustomerRankingModel> TopCustomers { get; set; }

        // Tab 1: Doanh số nhân viên 
        public ObservableCollection<StaffRankingModel> TopStaffs { get; set; }

        // Tab 2: Đơn hàng trong ngày 
        public ObservableCollection<OrderModel> Items { get; set; }

        // Tab 2: Nhập kho trong ngày 
        public ObservableCollection<ImportModel> ImportItems { get; set; }

        // Tab 2: Phiếu thu tiền trong ngày
        public ObservableCollection<PaymentModel> PaymentReceipts { get; set; }

        // Tab 2: Cảnh báo tồn kho 
        public ObservableCollection<StockWarningModel> StockWarnings { get; set; }


        public DashboardViewModel()
        {
            // Khởi tạo các List
            TopBooks = new ObservableCollection<TopBookModel>();
            TopCustomers = new ObservableCollection<CustomerRankingModel>();
            TopStaffs = new ObservableCollection<StaffRankingModel>();
            Items = new ObservableCollection<OrderModel>();
            ImportItems = new ObservableCollection<ImportModel>();
            PaymentReceipts = new ObservableCollection<PaymentModel>();
            StockWarnings = new ObservableCollection<StockWarningModel>();

            LoadMockData();
        }

        /// <summary>
        /// Hàm load dữ liệu giả lập để test UI. 
        /// Sau này ông thay code query Entity Framework / API vào đây nhé!
        /// </summary>
        private void LoadMockData()
        {
            //  Chỉ số tổng quan
            Sale = 25450000;
            Profit = 12500000;
            CustNum = 45;
            ReceiptNum = 128;

            // Data Biểu đồ Xu hướng doanh thu (Cartesian Chart)
            RevenueSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = new double[] { 15, 20, 18, 25, 22, 30, 28 },
                    Name = "Doanh thu (Triệu VNĐ)",
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                    Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(50)),
                    GeometrySize = 10
                }
            };

            // (Pie Chart) Thể loại
            //Data = new ISeries[]
            //{
            //    new PieSeries<double> { Values = new double[] { 45 }, Name = "Công nghệ thông tin" },
            //    new PieSeries<double> { Values = new double[] { 25 }, Name = "Kinh tế - Quản trị" },
            //    new PieSeries<double> { Values = new double[] { 20 }, Name = "Văn học" },
            //    new PieSeries<double> { Values = new double[] { 10 }, Name = "Tâm lý - Kỹ năng" }
            //};
            Data = new ObservableCollection<ISeries>
            {
                new PieSeries<double> { Values = new double[] { 45 }, Name = "Công nghệ thông tin" },
                new PieSeries<double> { Values = new double[] { 25 }, Name = "Kinh tế - Quản trị" },
                new PieSeries<double> { Values = new double[] { 20 }, Name = "Văn học" },
                new PieSeries<double> { Values = new double[] { 10 }, Name = "Tâm lý - Kỹ năng" }
            };

            // Tab 1: Danh sách Top
            TopBooks.Add(new TopBookModel { Rank = 1, BookImage = "/Resources/Images/Books/matbiec.jpg" });
            TopBooks.Add(new TopBookModel { Rank = 2, BookImage = "/Resources/Images/Books/default_book_cover.jpg" });
            TopBooks.Add(new TopBookModel { Rank = 3, BookImage = "/Resources/Images/Books/default_book_cover.jpg" });
            TopBooks.Add(new TopBookModel { Rank = 4, BookImage = "/Resources/Images/Books/default_book_cover.jpg" });
            TopBooks.Add(new TopBookModel { Rank = 5, BookImage = "/Resources/Images/Books/default_book_cover.jpg" });

            TopCustomers.Add(new CustomerRankingModel { Name = "Nguyễn Văn A", TotalSpent = 15500000 });
            TopCustomers.Add(new CustomerRankingModel { Name = "Trần Thị B", TotalSpent = 12200000 });
            TopCustomers.Add(new CustomerRankingModel { Name = "Lê Hoàng C", TotalSpent = 9800000 });

            TopStaffs.Add(new StaffRankingModel { Name = "Phạm Nhân Viên 1", SalesAmount = 45000000 });
            TopStaffs.Add(new StaffRankingModel { Name = "Hoàng Nhân Viên 2", SalesAmount = 38500000 });

            //  Tab 2: Vận hành
            Items.Add(new OrderModel { ReceiptNum = "HD001", CustomerName = "Khách Lẻ", TotalCost = 150000 });
            Items.Add(new OrderModel { ReceiptNum = "HD002", CustomerName = "Nguyễn Văn A", TotalCost = 1250000 });

            ImportItems.Add(new ImportModel { ImportId = "NK001", SupplierName = "NXB Trẻ", TotalQuantity = 500 });
            ImportItems.Add(new ImportModel { ImportId = "NK002", SupplierName = "NXB Kim Đồng", TotalQuantity = 300 });

            PaymentReceipts.Add(new PaymentModel { PaymentId = "PT001", Reason = "Thu tiền nợ KH", Amount = 5000000 });

            StockWarnings.Add(new StockWarningModel { Name = "C# căn bản tới nâng cao", RemainingQuantity = 5 });
            StockWarnings.Add(new StockWarningModel { Name = "Đắc Nhân Tâm", RemainingQuantity = 2 });
        }
    }

    // ==========================================
    // CÁC LỚP MODEL DÙNG ĐỂ BINDING CHO DATAGRID
    // ==========================================
    public class TopBookModel
    {
        public int Rank { get; set; }
        public string BookImage { get; set; }
    }

    public class CustomerRankingModel
    {
        public string Name { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class StaffRankingModel
    {
        public string Name { get; set; }
        public decimal SalesAmount { get; set; }
    }

    public class OrderModel
    {
        public string ReceiptNum { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalCost { get; set; }
    }

    public class ImportModel
    {
        public string ImportId { get; set; }
        public string SupplierName { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class PaymentModel
    {
        public string PaymentId { get; set; }
        public string Reason { get; set; }
        public decimal Amount { get; set; }
    }

    public class StockWarningModel
    {
        public string Name { get; set; }
        public int RemainingQuantity { get; set; }
    }
}