using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        // ==========================================
        // BỘ LỌC

        private int _selectedReportTypeIndex = 0;
        public int SelectedReportTypeIndex
        {
            get => _selectedReportTypeIndex;
            set
            {
                _selectedReportTypeIndex = value;
                OnPropertyChanged(nameof(SelectedReportTypeIndex));

                // Cập nhật visibility cho toàn bộ UI dynamic
                OnPropertyChanged(nameof(RevenueSecondaryFilterVisibility));
                OnPropertyChanged(nameof(DebtSecondaryFilterVisibility));
                OnPropertyChanged(nameof(RevenueChartVisibility));
                OnPropertyChanged(nameof(InventoryChartVisibility));
                OnPropertyChanged(nameof(DebtChartVisibility));
                OnPropertyChanged(nameof(RevenueGridVisibility));
                OnPropertyChanged(nameof(InventoryGridVisibility));
                OnPropertyChanged(nameof(DebtGridVisibility));
                OnPropertyChanged(nameof(ChartTitle));
                OnPropertyChanged(nameof(ChartSubtitle));
                OnPropertyChanged(nameof(ChartIconKind));
                OnPropertyChanged(nameof(TableTitle));

                // Reset dữ liệu khi đổi loại báo cáo
                StatusText = "Nhấn 'Xem Báo Cáo' để tải dữ liệu.";
                TotalRows = 0;
            }
        }

        private DateTime _fromDate = DateTime.Today.AddMonths(-1);
        public DateTime FromDate
        {
            get => _fromDate;
            set { _fromDate = value; OnPropertyChanged(nameof(FromDate)); }
        }

        private DateTime _toDate = DateTime.Today;
        public DateTime ToDate
        {
            get => _toDate;
            set { _toDate = value; OnPropertyChanged(nameof(ToDate)); }
        }

        // Bộ lọc phụ - Loại sub-filter cho Doanh thu (0=Tất cả, 1=Nhân viên, 2=Thể loại)
        private int _selectedRevenueSubFilterType = 0;
        public int SelectedRevenueSubFilterType
        {
            get => _selectedRevenueSubFilterType;
            set
            {
                _selectedRevenueSubFilterType = value;
                OnPropertyChanged(nameof(SelectedRevenueSubFilterType));
                OnPropertyChanged(nameof(RevenueSubItemFilterVisibility));
                OnPropertyChanged(nameof(RevenueSubItemsSource));
            }
        }

        private string _selectedRevenueSubItem;
        public string SelectedRevenueSubItem
        {
            get => _selectedRevenueSubItem;
            set { _selectedRevenueSubItem = value; OnPropertyChanged(nameof(SelectedRevenueSubItem)); }
        }

        private string _selectedCustomer;
        public string SelectedCustomer
        {
            get => _selectedCustomer;
            set { _selectedCustomer = value; OnPropertyChanged(nameof(SelectedCustomer)); }
        }

        // Danh sách cho bộ lọc phụ
        public ObservableCollection<string> StaffList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CategoryList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CustomerList { get; set; } = new ObservableCollection<string>();

        // Source động cho sub-item combobox của Revenue
        public ObservableCollection<string> RevenueSubItemsSource =>
            SelectedRevenueSubFilterType == 1 ? StaffList : CategoryList;

        // ==========================================
        // TRẠNG THÁI

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
                OnPropertyChanged(nameof(LoadingVisibility));
                OnPropertyChanged(nameof(ContentVisibility));
            }
        }

        private string _statusText = "Chọn bộ lọc và nhấn 'Xem Báo Cáo' để tải dữ liệu.";
        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(nameof(StatusText)); }
        }

        private int _totalRows;
        public int TotalRows
        {
            get => _totalRows;
            set { _totalRows = value; OnPropertyChanged(nameof(TotalRows)); }
        }

        // ==========================================
        // VISIBILITY - Điều khiển giao diện động

        // Filter phụ
        public Visibility RevenueSecondaryFilterVisibility =>
            SelectedReportTypeIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DebtSecondaryFilterVisibility =>
            SelectedReportTypeIndex == 2 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility RevenueSubItemFilterVisibility =>
            SelectedRevenueSubFilterType != 0 ? Visibility.Visible : Visibility.Collapsed;

        // Biểu đồ
        public Visibility RevenueChartVisibility =>
            SelectedReportTypeIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility InventoryChartVisibility =>
            SelectedReportTypeIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DebtChartVisibility =>
            SelectedReportTypeIndex == 2 ? Visibility.Visible : Visibility.Collapsed;

        // DataGrid
        public Visibility RevenueGridVisibility =>
            SelectedReportTypeIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility InventoryGridVisibility =>
            SelectedReportTypeIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DebtGridVisibility =>
            SelectedReportTypeIndex == 2 ? Visibility.Visible : Visibility.Collapsed;

        // Loading overlay
        public Visibility LoadingVisibility =>
            IsLoading ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ContentVisibility =>
            IsLoading ? Visibility.Collapsed : Visibility.Visible;

        // ==========================================
        // TIÊU ĐỀ ĐỘNG (Chart & Table)

        public string ChartTitle => SelectedReportTypeIndex switch
        {
            0 => "DOANH THU & LỢI NHUẬN",
            1 => "GIÁ TRỊ TỒN KHO (TOP 10)",
            2 => "BIẾN ĐỘNG CÔNG NỢ",
            _ => ""
        };

        public string ChartSubtitle => SelectedReportTypeIndex switch
        {
            0 => "Biểu đồ cột chồng: Giá vốn vs Lợi nhuận theo ngày",
            1 => "Biểu đồ thanh ngang: Top 10 đầu sách có giá trị tồn kho cao nhất",
            2 => "Biểu đồ đường: Nợ phát sinh vs Nợ thu hồi theo ngày",
            _ => ""
        };

        public string ChartIconKind => SelectedReportTypeIndex switch
        {
            0 => "ChartBar",
            1 => "PackageVariant",
            2 => "AccountCash",
            _ => "ChartLine"
        };

        public string TableTitle => SelectedReportTypeIndex switch
        {
            0 => "CHI TIẾT DOANH THU",
            1 => "CHI TIẾT TỒN KHO",
            2 => "CHI TIẾT CÔNG NỢ KHÁCH HÀNG",
            _ => ""
        };

        // ==========================================
        // BIỂU ĐỒ 1 - Doanh thu (Stacked Column)

        private ObservableCollection<ISeries> _revenueChartSeries;
        public ObservableCollection<ISeries> RevenueChartSeries
        {
            get => _revenueChartSeries;
            set { _revenueChartSeries = value; OnPropertyChanged(nameof(RevenueChartSeries)); }
        }

        private IEnumerable<Axis> _revenueXAxes;
        public IEnumerable<Axis> RevenueXAxes
        {
            get => _revenueXAxes;
            set { _revenueXAxes = value; OnPropertyChanged(nameof(RevenueXAxes)); }
        }

        private IEnumerable<Axis> _revenueYAxes;
        public IEnumerable<Axis> RevenueYAxes
        {
            get => _revenueYAxes;
            set { _revenueYAxes = value; OnPropertyChanged(nameof(RevenueYAxes)); }
        }

        // ==========================================
        // BIỂU ĐỒ 2 - Tồn kho (Horizontal Bar)

        private ObservableCollection<ISeries> _inventoryChartSeries;
        public ObservableCollection<ISeries> InventoryChartSeries
        {
            get => _inventoryChartSeries;
            set { _inventoryChartSeries = value; OnPropertyChanged(nameof(InventoryChartSeries)); }
        }

        private IEnumerable<Axis> _inventoryXAxes;
        public IEnumerable<Axis> InventoryXAxes
        {
            get => _inventoryXAxes;
            set { _inventoryXAxes = value; OnPropertyChanged(nameof(InventoryXAxes)); }
        }

        private IEnumerable<Axis> _inventoryYAxes;
        public IEnumerable<Axis> InventoryYAxes
        {
            get => _inventoryYAxes;
            set { _inventoryYAxes = value; OnPropertyChanged(nameof(InventoryYAxes)); }
        }

        // ==========================================
        // BIỂU ĐỒ 3 - Công nợ (Line)

        private ObservableCollection<ISeries> _debtChartSeries;
        public ObservableCollection<ISeries> DebtChartSeries
        {
            get => _debtChartSeries;
            set { _debtChartSeries = value; OnPropertyChanged(nameof(DebtChartSeries)); }
        }

        private IEnumerable<Axis> _debtXAxes;
        public IEnumerable<Axis> DebtXAxes
        {
            get => _debtXAxes;
            set { _debtXAxes = value; OnPropertyChanged(nameof(DebtXAxes)); }
        }

        // ==========================================
        // DỮ LIỆU BẢNG

        public ObservableCollection<RevenueReportRowDto> RevenueRows { get; set; }
        public ObservableCollection<InventoryReportRowDto> InventoryRows { get; set; }
        public ObservableCollection<DebtReportRowDto> DebtRows { get; set; }

        // ==========================================
        // LỆNH

        public ICommand ApplyReportCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand ExportPdfCommand { get; }

        // ==========================================
        // CONSTRUCTOR

        public ReportViewModel()
        {
            RevenueRows = new ObservableCollection<RevenueReportRowDto>();
            InventoryRows = new ObservableCollection<InventoryReportRowDto>();
            DebtRows = new ObservableCollection<DebtReportRowDto>();

            ApplyReportCommand = new RelayCommand<object>(async (p) => await LoadReportDataAsync());
            //ExportExcelCommand = new RelayCommand(_ => ExportToExcel(), _ => TotalRows > 0);
            //ExportPdfCommand = new RelayCommand(_ => ExportToPdf(), _ => TotalRows > 0);

            FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ToDate = DateTime.Today;

            // Load danh sách filter phụ từ API
            _ = LoadFilterListsAsync();

            _ = LoadReportDataAsync();
        }

        // ==========================================
        // LOAD DỮ LIỆU

        /// <summary>
        /// Load danh sách nhân viên, thể loại, khách hàng cho bộ lọc phụ.
        /// </summary>
        private async Task LoadFilterListsAsync()
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7001/") };

                var staffs = await client.GetFromJsonAsync<List<string>>("api/report/filter/staffs");
                var categories = await client.GetFromJsonAsync<List<string>>("api/report/filter/categories");
                var customers = await client.GetFromJsonAsync<List<string>>("api/report/filter/customers");

                if (staffs != null) foreach (var s in staffs) StaffList.Add(s);
                if (categories != null) foreach (var c in categories) CategoryList.Add(c);
                if (customers != null) foreach (var c in customers) CustomerList.Add(c);
            }
            catch
            {
                // Lỗi load filter list thì bỏ qua, không hiện thông báo
            }
        }

        /// <summary>
        /// Gọi API lấy dữ liệu báo cáo theo bộ lọc đã chọn.
        /// </summary>
        private async Task LoadReportDataAsync()
        {
            if (FromDate > ToDate)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi bộ lọc",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;
            StatusText = "Đang tải dữ liệu...";

            try
            {
                using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7001/") };

                var filter = new ReportFilterDto
                {
                    ReportType = SelectedReportTypeIndex,
                    FromDate = FromDate,
                    ToDate = ToDate,
                    StaffName = SelectedRevenueSubFilterType == 1 ? SelectedRevenueSubItem : null,
                    CategoryName = SelectedRevenueSubFilterType == 2 ? SelectedRevenueSubItem : null,
                    CustomerName = SelectedCustomer
                };

                var result = await client.PostAsJsonAsync("api/report/generate", filter);
                result.EnsureSuccessStatusCode();
                var data = await result.Content.ReadFromJsonAsync<ReportResultDto>();

                if (data != null)
                {
                    // --- Cập nhật bảng dữ liệu ---
                    RevenueRows.Clear();
                    InventoryRows.Clear();
                    DebtRows.Clear();

                    if (data.RevenueRows != null)
                        foreach (var r in data.RevenueRows) RevenueRows.Add(r);

                    if (data.InventoryRows != null)
                        foreach (var r in data.InventoryRows) InventoryRows.Add(r);

                    if (data.DebtRows != null)
                        foreach (var r in data.DebtRows) DebtRows.Add(r);

                    // --- Cập nhật biểu đồ ---
                    SetupRevenueChart(data);
                    SetupInventoryChart(data);
                    SetupDebtChart(data);

                    // --- Cập nhật trạng thái ---
                    TotalRows = SelectedReportTypeIndex switch
                    {
                        0 => RevenueRows.Count,
                        1 => InventoryRows.Count,
                        2 => DebtRows.Count,
                        _ => 0
                    };

                    StatusText = $"Hiển thị {TotalRows} dòng — từ {FromDate:dd/MM/yyyy} đến {ToDate:dd/MM/yyyy}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                StatusText = "Không thể tải dữ liệu. Kiểm tra kết nối server.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ==========================================
        // THIẾT LẬP BIỂU ĐỒ

        /// <summary>
        /// Biểu đồ cột chồng: Giá vốn (đỏ) + Lợi nhuận (xanh) = Doanh thu.
        /// </summary>
        private void SetupRevenueChart(ReportResultDto data)
        {
            if (data.RevenueDateLabels == null) return;

            var costValues = data.RevenueCostSeries?.Select(x => (double)x).ToArray() ?? Array.Empty<double>();
            var profitValues = data.RevenueProfitSeries?.Select(x => (double)x).ToArray() ?? Array.Empty<double>();

            RevenueChartSeries = new ObservableCollection<ISeries>
            {
                new StackedColumnSeries<double>
                {
                    Name = "Giá vốn",
                    Values = costValues,
                    Fill = new SolidColorPaint(new SKColor(238, 93, 80)),       // #EE5D50 đỏ
                    Stroke = null,
                    DataLabelsPaint = null
                },
                new StackedColumnSeries<double>
                {
                    Name = "Lợi nhuận",
                    Values = profitValues,
                    Fill = new SolidColorPaint(new SKColor(5, 205, 153)),       // #05CD99 xanh lá
                    Stroke = null,
                    DataLabelsPaint = null
                }
            };

            RevenueXAxes = new[]
            {
                new Axis
                {
                    Labels = data.RevenueDateLabels,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(new SKColor(43, 54, 116))  // #2B3674
                }
            };

            RevenueYAxes = new[]
            {
                new Axis
                {
                    Labeler = value => $"{value / 1_000_000:N0}M",
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(new SKColor(163, 174, 208)) // #A3AED0
                }
            };
        }

        /// <summary>
        /// Biểu đồ thanh ngang: Top 10 sách có giá trị tồn kho cao nhất.
        /// </summary>
        private void SetupInventoryChart(ReportResultDto data)
        {
            if (data.InventoryBarLabels == null) return;

            InventoryChartSeries = new ObservableCollection<ISeries>
            {
                new RowSeries<double>
                {
                    Name = "Giá trị tồn kho",
                    Values = data.InventoryBarValues?.Select(x => (double)x).ToArray() ?? Array.Empty<double>(),
                    Fill = new SolidColorPaint(new SKColor(67, 24, 255)),       // #4318FF tím
                    MaxBarWidth = 20,
                    DataLabelsPaint = new SolidColorPaint(new SKColor(43, 54, 116)),
                    DataLabelsSize = 10,
                    DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue / 1_000_000:N0}M đ"
                }
            };

            InventoryYAxes = new[]
            {
                new Axis
                {
                    Labels = data.InventoryBarLabels,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(new SKColor(43, 54, 116))
                }
            };

            InventoryXAxes = new[]
            {
                new Axis
                {
                    Labeler = value => $"{value / 1_000_000:N0}M",
                    TextSize = 10,
                    LabelsPaint = new SolidColorPaint(new SKColor(163, 174, 208))
                }
            };
        }

        /// <summary>
        /// Biểu đồ đường: Nợ phát sinh (đỏ) và Nợ thu hồi (xanh).
        /// </summary>
        private void SetupDebtChart(ReportResultDto data)
        {
            if (data.DebtAxisLabels == null) return;

            DebtChartSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Name = "Nợ phát sinh",
                    Values = data.DebtNewSeries?.Select(x => (double)x).ToArray() ?? Array.Empty<double>(),
                    Stroke = new SolidColorPaint(new SKColor(238, 93, 80)) { StrokeThickness = 3 },
                    Fill = new SolidColorPaint(new SKColor(238, 93, 80, 30)),
                    GeometrySize = 8,
                    GeometryFill = new SolidColorPaint(SKColors.White),
                    GeometryStroke = new SolidColorPaint(new SKColor(238, 93, 80)) { StrokeThickness = 2 }
                },
                new LineSeries<double>
                {
                    Name = "Nợ thu hồi",
                    Values = data.DebtPaidSeries?.Select(x => (double)x).ToArray() ?? Array.Empty<double>(),
                    Stroke = new SolidColorPaint(new SKColor(5, 205, 153)) { StrokeThickness = 3 },
                    Fill = new SolidColorPaint(new SKColor(5, 205, 153, 30)),
                    GeometrySize = 8,
                    GeometryFill = new SolidColorPaint(SKColors.White),
                    GeometryStroke = new SolidColorPaint(new SKColor(5, 205, 153)) { StrokeThickness = 2 }
                }
            };

            DebtXAxes = new[]
            {
                new Axis
                {
                    Labels = data.DebtAxisLabels,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(new SKColor(43, 54, 116))
                }
            };
        }

        // ==========================================
        // XUẤT FILE

        private void ExportToExcel()
        {
            MessageBox.Show("Chức năng xuất Excel đang được phát triển.", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
            // TODO: Dùng ClosedXML
            //   var wb = new XLWorkbook();
            //   var ws = wb.Worksheets.Add("Báo cáo");
            //   if (SelectedReportTypeIndex == 0) { /* fill RevenueRows */ }
            //   var dlg = new SaveFileDialog { Filter = "Excel|*.xlsx", FileName = "BaoCao.xlsx" };
            //   if (dlg.ShowDialog() == true) wb.SaveAs(dlg.FileName);
        }

        private void ExportToPdf()
        {
            MessageBox.Show("Chức năng xuất PDF đang được phát triển.", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
            // TODO: Dùng iTextSharp hoặc PdfSharp
        }
    }
}
