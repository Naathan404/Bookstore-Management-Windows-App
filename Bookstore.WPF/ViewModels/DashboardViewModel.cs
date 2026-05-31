using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MaterialDesignThemes.Wpf;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }
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

        private string _saleChangeIcon = "TreiangleNeutral";
        public string SaleChangeIcon
        {
            get => _saleChangeIcon;
            set
            {
                _saleChangeIcon = value;
                OnPropertyChanged(nameof(SaleChangeIcon));
            }
        }

        private string _saleChangeText = "0%";
        public string SaleChangeText
        {
            get => _saleChangeText;
            set
            {
                _saleChangeText = value;
                OnPropertyChanged(nameof(SaleChangeText));
            }
        }

        private string _saleChangeColor = "#FFB547";
        public string SaleChangeColor
        {
            get => _saleChangeColor;
            set
            {
                _saleChangeColor = value;
                OnPropertyChanged(nameof(SaleChangeColor));
            }
        }

        // ==========================================
        // BIỂU ĐỒ 

        // Biểu đồ xu hướng doanh thu 
        //private ISeries[] _revenueSeries;
        //public ISeries[] RevenueSeries
        //{
        //    get => _revenueSeries;
        //    set { _revenueSeries = value; OnPropertyChanged(nameof(RevenueSeries)); }
        //}

        private ObservableCollection<ISeries> _revenueSeries;
        public ObservableCollection<ISeries> RevenueSeries
        {
            get => _revenueSeries;
            set { _revenueSeries = value; OnPropertyChanged(nameof(RevenueSeries)); }
        }

        private ObservableCollection<ISeries> _data;
        public ObservableCollection<ISeries> Data
        {
            get => _data;
            set { _data = value; OnPropertyChanged(nameof(Data)); }
        }

        private ObservableCollection<ISeries> _comparisonSeries;
        public ObservableCollection<ISeries> ComparisonSeries
        {
            get => _comparisonSeries;
            set { _comparisonSeries = value; OnPropertyChanged(nameof(ComparisonSeries)); }
        }

        // ==========================================
        // DANH SÁCH

        // Tab 1: Top 5 sách bán chạy
        public ObservableCollection<TopBookDto> TopBooks { get; set; }

        // Tab 1: Top Khách hàng VIP
        public ObservableCollection<CustomerRankingDto> TopCustomers { get; set; }

        // Tab 1: Doanh số nhân viên 
        public ObservableCollection<StaffRankingDto> TopStaffs { get; set; }

        // Tab 2: Đơn hàng trong ngày 
        public ObservableCollection<OrderDto> Items { get; set; }

        // Tab 2: Nhập kho trong ngày 
        public ObservableCollection<ImportDto> ImportItems { get; set; }

        // Tab 2: Phiếu thu tiền trong ngày
        public ObservableCollection<PaymentDto> PaymentReceipts { get; set; }

        // Tab 2: Cảnh báo tồn kho 
        public ObservableCollection<StockWarningDto> StockWarnings { get; set; }


        public DashboardViewModel()
        {
            // Khởi tạo các List
            TopBooks = new ObservableCollection<TopBookDto>();
            TopCustomers = new ObservableCollection<CustomerRankingDto>();
            TopStaffs = new ObservableCollection<StaffRankingDto>();
            Items = new ObservableCollection<OrderDto>();
            ImportItems = new ObservableCollection<ImportDto>();
            PaymentReceipts = new ObservableCollection<PaymentDto>();
            StockWarnings = new ObservableCollection<StockWarningDto>();

            LoadAllDataAsync();
        }

        /// <summary>
        /// This hàm để load dữ liệu cho trang Dashboard. 
        /// </summary>
        public async Task LoadAllDataAsync()
        {
            string[] last7Days = new string[7];
            for (int i = 6; i >= 0; i--)
            {
                last7Days[6 - i] = DateTime.Now.AddDays(-i).ToString("dd/MM");
            }

            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = last7Days,
                    LabelsRotation = 0, 
                    TextSize = 13,
                    LabelsPaint = new SolidColorPaint(SKColors.Gray)
                }
            };


            YAxes = new Axis[]
            {
                new Axis
                {
                    MinLimit = 0,
                    Labeler = value => value.ToString("N0") 
                }
            };

            IsLoading = true;
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7001/") };

                // call api get dashboard overview
                var data = await client.GetFromJsonAsync<DashboardOverviewDto>("api/dashboard/overview");

                if (data != null)
                {
                    // số liệu tổng quan
                    Sale = data.Sale;
                    Profit = data.Profit;
                    CustNum = data.CustNum;
                    ReceiptNum = data.ReceiptNum;
                    SaleChangeText = data.SaleChangeText;
                    SaleChangeIcon = data.SaleChangeIcon;
                    SaleChangeColor = data.SaleChangeColor;

                    //  dữ liệu cho Bảng lưới
                    TopBooks.Clear();
                    foreach (var item in data.TopBooks)
                        TopBooks.Add(item);
                    TopCustomers.Clear();
                    foreach (var item in data.TopCustomers)
                        TopCustomers.Add(item);
                    TopStaffs.Clear();
                    foreach (var item in data.TopStaffs)
                        TopStaffs.Add(item);
                    Items.Clear();
                    foreach (var item in data.RecentOrders)
                        Items.Add(item);
                    ImportItems.Clear();
                    foreach (var item in data.RecentImports)
                        ImportItems.Add(item);
                    PaymentReceipts.Clear();
                    foreach (var item in data.RecentPayments)
                        PaymentReceipts.Add(item);
                    StockWarnings.Clear();
                    foreach (var item in data.StockWarnings)
                        StockWarnings.Add(item);


                    // gọi hàm xử lý biểu đồ 
                    SetupLiveCharts(data);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SetupLiveCharts(DashboardOverviewDto data)
        {
            // --- Biểu đồ Đường (Doanh thu) ---
            var valuesArray = data.RevenueSeries.Select(x => x.Value).ToArray();
            RevenueSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = valuesArray,
                    Name = "Doanh thu",
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                    Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(50)),
                    GeometrySize = 10
                }
            };

            // --- Biểu đồ Tròn (Tỷ trọng thể loại) ---
            var pieSeriesList = new ObservableCollection<ISeries>();
            foreach (var item in data.CategoryShares)
            {
                pieSeriesList.Add(new PieSeries<double>
                {
                    Values = new double[] { item.Percentage },
                    Name = item.CategoryName,

                    // GỌI THẲNG POINT.MODEL
                    ToolTipLabelFormatter = point => $"{point.Model}%"
                });
            }
            Data = pieSeriesList;

            // -- Biểu đồ cột (so sánh doanh thuvà chi phí s) ---
            var dates = data.ComparisonSeries.Select(x => x.Date).ToArray();
            var revenueValues = data.ComparisonSeries.Select(x => x.Revenue).ToArray();
            var importValues = data.ComparisonSeries.Select(x => x.ImportCost).ToArray();
            var profitValues = data.ComparisonSeries.Select(x => x.Profit).ToArray();

            ComparisonSeries = new ObservableCollection<ISeries>
            {
                new ColumnSeries<double> 
                { 
                    Name = "Doanh thu", 
                    Values = revenueValues, 
                    Fill = new SolidColorPaint(SKColors.CornflowerBlue) 
                },
                new ColumnSeries<double> 
                { 
                    Name = "Chi phí nhập", 
                    Values = importValues, 
                    Fill = new SolidColorPaint(SKColors.Tomato) 
                },
                new LineSeries<double> 
                { 
                    Name = "Lợi nhuận",
                    Values = profitValues,
                    Stroke = new SolidColorPaint(SKColors.Gold) { StrokeThickness = 4 }, 
                    Fill = null 
                }
            };
        }
    }
}