using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels.Base;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace Bookstore.WPF.ViewModels
{
    // KẾ THỪA TỪ BASE LIST
    public class ReportViewModel : BaseListViewModel
    {
        // ==========================================
        // BỘ LỌC TÙY CHỈNH CỦA BÁO CÁO

        private int _selectedReportTypeIndex = 0;
        public int SelectedReportTypeIndex
        {
            get => _selectedReportTypeIndex;
            set
            {
                _selectedReportTypeIndex = value;
                OnPropertyChanged(nameof(SelectedReportTypeIndex));
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

        private string _selectedCustomerType = "Tất cả khách hàng";
        public string SelectedCustomerType
        {
            get => _selectedCustomerType;
            set { _selectedCustomerType = value; OnPropertyChanged(nameof(SelectedCustomerType)); }
        }

        public ObservableCollection<string> StaffList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CategoryList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CustomerTypeList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ReportTypeList { get; set; } = new ObservableCollection<string> {
            "Doanh thu & Lợi nhuận",
            "Tồn kho",
            "Công nợ"
        };

        public ObservableCollection<string> RevenueSubFilterTypeList { get; set; } = new ObservableCollection<string>
        {
            "Tất cả",
            "Nhân viên",
            "Thể loại"
        };

        public ObservableCollection<string> RevenueSubItemsSource =>
            SelectedRevenueSubFilterType == 1 ? StaffList : CategoryList;

        // ==========================================
        // TRẠNG THÁI & HIỂN THỊ

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

        private string _analysisMessage = "Chưa có phân tích dữ liệu.";
        public string AnalysisMessage
        {
            get => _analysisMessage;
            set { _analysisMessage = value; OnPropertyChanged(nameof(AnalysisMessage)); }
        }

        // ==========================================
        // VISIBILITY - Điều khiển giao diện động

        public Visibility RevenueSecondaryFilterVisibility => SelectedReportTypeIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DebtSecondaryFilterVisibility => SelectedReportTypeIndex == 2 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility RevenueSubItemFilterVisibility => SelectedRevenueSubFilterType != 0 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility RevenueChartVisibility => SelectedReportTypeIndex == 0 ? Visibility.Visible : Visibility.Hidden;
        public Visibility InventoryChartVisibility => SelectedReportTypeIndex == 1 ? Visibility.Visible : Visibility.Hidden;
        public Visibility DebtChartVisibility => SelectedReportTypeIndex == 2 ? Visibility.Visible : Visibility.Hidden;

        public Visibility RevenueGridVisibility => SelectedReportTypeIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility InventoryGridVisibility => SelectedReportTypeIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DebtGridVisibility => SelectedReportTypeIndex == 2 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility LoadingVisibility => IsLoading ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ContentVisibility => IsLoading ? Visibility.Collapsed : Visibility.Visible;

        // ==========================================
        // TIÊU ĐỀ ĐỘNG

        public string ChartTitle => SelectedReportTypeIndex switch { 0 => "THỐNG KÊ DOANH THU & LỢI NHUẬN", 1 => "THỐNG KÊ TỒN KHO", 2 => "THỐNG KÊ CÔNG NỢ", _ => "" };
        public string ChartIconKind => SelectedReportTypeIndex switch { 0 => "ChartBar", 1 => "PackageVariant", 2 => "AccountCash", _ => "ChartLine" };
        public string TableTitle => SelectedReportTypeIndex switch { 0 => "CHI TIẾT DOANH THU", 1 => "CHI TIẾT TỒN KHO", 2 => "CHI TIẾT CÔNG NỢ KHÁCH HÀNG", _ => "" };

        // ==========================================
        // BIỂU ĐỒ SERIES (Giữ nguyên)
        private ObservableCollection<ISeries> _revenueChartSeries;
        public ObservableCollection<ISeries> RevenueChartSeries { get => _revenueChartSeries; set { _revenueChartSeries = value; OnPropertyChanged(nameof(RevenueChartSeries)); } }
        private IEnumerable<Axis> _revenueXAxes;
        public IEnumerable<Axis> RevenueXAxes { get => _revenueXAxes; set { _revenueXAxes = value; OnPropertyChanged(nameof(RevenueXAxes)); } }
        private IEnumerable<Axis> _revenueYAxes;
        public IEnumerable<Axis> RevenueYAxes { get => _revenueYAxes; set { _revenueYAxes = value; OnPropertyChanged(nameof(RevenueYAxes)); } }

        private ObservableCollection<ISeries> _inventoryChartSeries;
        public ObservableCollection<ISeries> InventoryChartSeries { get => _inventoryChartSeries; set { _inventoryChartSeries = value; OnPropertyChanged(nameof(InventoryChartSeries)); } }
        private IEnumerable<Axis> _inventoryXAxes;
        public IEnumerable<Axis> InventoryXAxes { get => _inventoryXAxes; set { _inventoryXAxes = value; OnPropertyChanged(nameof(InventoryXAxes)); } }
        private IEnumerable<Axis> _inventoryYAxes;
        public IEnumerable<Axis> InventoryYAxes { get => _inventoryYAxes; set { _inventoryYAxes = value; OnPropertyChanged(nameof(InventoryYAxes)); } }

        private ObservableCollection<ISeries> _debtChartSeries;
        public ObservableCollection<ISeries> DebtChartSeries { get => _debtChartSeries; set { _debtChartSeries = value; OnPropertyChanged(nameof(DebtChartSeries)); } }
        private IEnumerable<Axis> _debtXAxes;
        public IEnumerable<Axis> DebtXAxes { get => _debtXAxes; set { _debtXAxes = value; OnPropertyChanged(nameof(DebtXAxes)); } }
        private IEnumerable<Axis> _debtYAxes;
        public IEnumerable<Axis> DebtYAxes { get => _debtYAxes; set { _debtYAxes = value; OnPropertyChanged(nameof(DebtYAxes)); } }

        // ==========================================
        // DỮ LIỆU BẢNG

        private List<RevenueReportRowDto> _allRevenueRows = new();
        private List<InventoryReportRowDto> _allInventoryRows = new();
        private List<DebtReportRowDto> _allDebtRows = new();

        public ObservableCollection<RevenueReportRowDto> RevenueRows { get; set; } = new();
        public ObservableCollection<InventoryReportRowDto> InventoryRows { get; set; } = new();
        public ObservableCollection<DebtReportRowDto> DebtRows { get; set; } = new();

        // ==========================================
        // LỆNH

        public ICommand ApplyReportCommand { get; set; }
        public ICommand ExportExcelCommand { get; set; }
        public ICommand ExportPdfCommand { get; set; }

        // ==========================================
        // CONSTRUCTOR

        public ReportViewModel()
        {
            ApplyReportCommand = new RelayCommand<object>(async (p) =>
            {
                OnPropertyChanged(nameof(SelectedReportTypeIndex));
                OnPropertyChanged(nameof(RevenueSecondaryFilterVisibility));
                OnPropertyChanged(nameof(DebtSecondaryFilterVisibility));
                OnPropertyChanged(nameof(RevenueChartVisibility));
                OnPropertyChanged(nameof(InventoryChartVisibility));
                OnPropertyChanged(nameof(DebtChartVisibility));
                OnPropertyChanged(nameof(RevenueGridVisibility));
                OnPropertyChanged(nameof(InventoryGridVisibility));
                OnPropertyChanged(nameof(DebtGridVisibility));
                OnPropertyChanged(nameof(ChartTitle));
                OnPropertyChanged(nameof(ChartIconKind));
                OnPropertyChanged(nameof(TableTitle));

                TrangHienTai = 1;
                StatusText = "Nhấn 'Xem Báo Cáo' để tải dữ liệu.";

                await LoadReportDataAsync();
            });

            ExportExcelCommand = new RelayCommand<object>(p => ExportToExcel());
            ExportPdfCommand = new RelayCommand<object>(p => ExportToPdf());

            FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ToDate = DateTime.Today;

            _ = LoadFilterListsAsync();
            _ = LoadReportDataAsync();
        }

        // ==========================================
        // GHI ĐÈ HÀM TỪ BASE LIST VIEW MODEL
        // ==========================================
        protected override void ApplyFilterAndPagination()
        {
            // Xóa dữ liệu hiển thị cũ
            RevenueRows.Clear();
            InventoryRows.Clear();
            DebtRows.Clear();

            int skip = (TrangHienTai - 1) * PageSize;

            // API Đã lọc sẵn, ta chỉ cần gắn Tổng bản ghi và Cắt dữ liệu (Skip, Take)
            if (SelectedReportTypeIndex == 0)
            {
                TongBanGhi = _allRevenueRows.Count;
                foreach (var item in _allRevenueRows.Skip(skip).Take(PageSize)) RevenueRows.Add(item);
            }
            else if (SelectedReportTypeIndex == 1)
            {
                TongBanGhi = _allInventoryRows.Count;
                foreach (var item in _allInventoryRows.Skip(skip).Take(PageSize)) InventoryRows.Add(item);
            }
            else if (SelectedReportTypeIndex == 2)
            {
                TongBanGhi = _allDebtRows.Count;
                foreach (var item in _allDebtRows.Skip(skip).Take(PageSize)) DebtRows.Add(item);
            }

            // Tính tổng số trang
            TongSoTrang = (int)Math.Ceiling((double)TongBanGhi / PageSize);
            if (TongSoTrang == 0) TongSoTrang = 1;

            if (TrangHienTai > TongSoTrang) TrangHienTai = TongSoTrang;
            if (TrangHienTai < 1) TrangHienTai = 1;
        }

        // ==========================================
        // API & LOGIC TẢI DỮ LIỆU

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
                CustomerTypeList.Add("Tất cả khách hàng");
                if (customers != null) foreach (var c in customers) CustomerTypeList.Add(c);
            }
            catch { /* Bỏ qua lỗi load filter */ }
        }

        private async Task LoadReportDataAsync()
        {
            if (FromDate > ToDate)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi bộ lọc", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    CustomerType = SelectedCustomerType
                };

                var result = await client.PostAsJsonAsync("api/report/generate", filter);
                if (!result.IsSuccessStatusCode)
                {
                    var error = await result.Content.ReadAsStringAsync();
                    MessageBox.Show(error, "API ERROR");
                    return;
                }

                var data = await result.Content.ReadFromJsonAsync<ReportResultDto>();

                if (data != null)
                {
                    // Lưu dữ liệu gốc
                    _allRevenueRows.Clear();
                    _allInventoryRows.Clear();
                    _allDebtRows.Clear();

                    if (data.RevenueRows != null) _allRevenueRows.AddRange(data.RevenueRows);
                    if (data.InventoryRows != null) _allInventoryRows.AddRange(data.InventoryRows);
                    if (data.DebtRows != null) _allDebtRows.AddRange(data.DebtRows);

                    // CHẠY HÀM PHÂN TRANG (sẽ tự động tính TongBanGhi, TongSoTrang và cắt dữ liệu cho Grid)
                    TrangHienTai = 1;
                    ApplyFilterAndPagination();

                    // Sinh câu phân tích
                    if (TongBanGhi == 0)
                    {
                        AnalysisMessage = "Hệ thống chưa ghi nhận bất kỳ phát sinh nào trong khoảng thời gian này.";
                    }
                    else
                    {
                        if (SelectedReportTypeIndex == 0)
                        {
                            decimal totalNetRevenue = _allRevenueRows.Sum(x => x.NetRevenue);
                            decimal totalGrossProfit = _allRevenueRows.Sum(x => x.GrossProfit);
                            decimal profitMargin = totalNetRevenue > 0 ? (totalGrossProfit / totalNetRevenue) * 100 : 0;

                            if (profitMargin >= 25)
                                AnalysisMessage = $"🔥 Tình hình kinh doanh xuất sắc! Tổng doanh thu đạt {totalNetRevenue:#,0} đ với biên lợi nhuận gộp rất cao ({profitMargin:N1}%), hoạt động kinh doanh đang tối ưu hiệu quả tốt.";
                            else if (profitMargin > 0 && profitMargin < 15)
                                AnalysisMessage = $"⚠️ Mặc dù doanh thu đạt {totalNetRevenue:#,0} đ nhưng biên lợi nhuận gộp khá mỏng ({profitMargin:N1}%), hãy kiểm tra lại giá vốn đầu vào hoặc giảm tần suất chương trình giảm giá.";
                            else
                                AnalysisMessage = $"✨ Kinh doanh ổn định. Tổng lợi nhuận gộp đạt {totalGrossProfit:#,0} đ (Hiệu suất đạt {profitMargin:N1}% trên tổng doanh thu thuần).";
                        }
                        else if (SelectedReportTypeIndex == 1)
                        {
                            decimal totalStockValue = _allInventoryRows.Sum(x => x.StockValue);
                            var topSpamBook = _allInventoryRows.OrderByDescending(x => x.StockValue).FirstOrDefault();
                            AnalysisMessage = $"📦 Tổng giá trị hàng hóa đang lưu kho đạt {totalStockValue:#,0} đ. Trong đó, đầu sách '{topSpamBook?.BookName}' đang chiếm tỷ trọng đọng vốn cao nhất, cần cân nhắc đẩy mạnh khuyến mãi.";
                        }
                        else if (SelectedReportTypeIndex == 2)
                        {
                            decimal totalClosingDebt = _allDebtRows.Sum(x => x.ClosingDebt);
                            int customerInDebtCount = _allDebtRows.Count(x => x.HasDebt);

                            if (totalClosingDebt > 20_000_000)
                                AnalysisMessage = $"🚨 Cảnh báo rủi ro! Hiện có {customerInDebtCount} khách hàng đang mua chịu với tổng công nợ đạt {totalClosingDebt:#,0} đ. Đề xuất siết chặt hạn mức bán nợ và ưu tiên phiếu thu tiền mặt.";
                            else
                                AnalysisMessage = $"✅ Chỉ số công nợ an toàn. Hệ thống đang kiểm soát tốt các khoản nợ phải thu với tổng số dư công nợ khách hàng là {totalClosingDebt:#,0} đ.";
                        }
                    }

                    await Task.Delay(new Random().Next(400, 800));

                    // Vẽ biểu đồ
                    SetupRevenueChart(data);
                    SetupInventoryChart(data);
                    SetupDebtChart(data);

                    // Cập nhật lại Status Text bằng biến TongBanGhi chuẩn
                    StatusText = $"Hiển thị {TongBanGhi} dòng — từ {FromDate:dd/MM/yyyy} đến {ToDate:dd/MM/yyyy}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusText = "Không thể tải dữ liệu. Kiểm tra kết nối server.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ==========================================
        // THIẾT LẬP BIỂU ĐỒ (Giữ nguyên hoàn toàn logic biểu đồ cũ)

        private void SetupRevenueChart(ReportResultDto data)
        {
            if (data.RevenueDateLabels == null) return;
            var costValues = data.RevenueCostSeries?.Select(x => (double)x).ToArray() ?? Array.Empty<double>();
            var profitValues = data.RevenueProfitSeries?.Select(x => (double)x).ToArray() ?? Array.Empty<double>();
            var totalValues = new double[costValues.Length];
            for (int i = 0; i < costValues.Length; i++) totalValues[i] = costValues[i] + profitValues[i];

            RevenueChartSeries = new ObservableCollection<ISeries>
            {
                new StackedColumnSeries<double> { Name = "Giá vốn", Values = costValues, Fill = new SolidColorPaint(SKColors.Tomato), Rx = 1, Ry = 1, Stroke = null, DataLabelsPaint = null },
                new StackedColumnSeries<double> { Name = "Lợi nhuận", Values = profitValues, Fill = new SolidColorPaint(SKColors.MediumSpringGreen), Rx = 1, Ry = 1, Stroke = null, DataLabelsPaint = null }
            };

            RevenueChartSeries.Add(new LineSeries<double>
            {
                Name = "TỔNG DOANH THU",
                Values = totalValues,
                Stroke = null,
                Fill = null,
                GeometrySize = 0,
                DataLabelsPaint = new SolidColorPaint(new SKColor(43, 54, 116)),
                DataLabelsSize = 15,
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                DataLabelsFormatter = point => { double val = point.Coordinate.PrimaryValue; return val >= 1_000_000 ? $"{val / 1_000_000:N1}M" : val >= 1_000 ? $"{val / 1_000:N0}K" : $"{val:N0}"; },
                YToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:#,0} đ"
            });

            RevenueXAxes = new[] { new Axis { Labels = data.RevenueDateLabels, TextSize = 11, LabelsPaint = new SolidColorPaint(new SKColor(43, 54, 116)) } };
            RevenueYAxes = new[] { new Axis { Labeler = value => $"{value:N0} đ", TextSize = 11, LabelsPaint = new SolidColorPaint(new SKColor(163, 174, 208)) } };
        }

        private void SetupInventoryChart(ReportResultDto data)
        {
            if (data.InventoryBarLabels == null) return;
            var invenValue = data.InventoryBarValues?.Select(x => (double)x).ToArray() ?? Array.Empty<double>();

            InventoryChartSeries = new ObservableCollection<ISeries>
            {
                new RowSeries<double>
                {
                    Name = "Giá trị tồn kho", Values = invenValue, Fill = new SolidColorPaint(new SKColor(67, 24, 255)), MaxBarWidth = 20,
                    XToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:N0} đ",
                    DataLabelsPaint = new SolidColorPaint(new SKColor(43, 54, 116)), DataLabelsSize = 11,
                    DataLabelsFormatter = point => { double val = point.Coordinate.PrimaryValue; return val >= 1_000_000 ? $"{val / 1_000_000:N1}M" : val >= 1_000 ? $"{val / 1_000:N0}K" : $"{val:N0}"; },
                }
            };

            InventoryYAxes = new[] { new Axis { Labels = data.InventoryBarLabels, TextSize = 11, LabelsPaint = new SolidColorPaint(new SKColor(43, 54, 116)) } };
            InventoryXAxes = new[] { new Axis { Labeler = value => { double val = value; return val >= 1_000_000 ? $"{val / 1_000_000:N1}M" : val >= 1_000 ? $"{val / 1_000:N0}K" : $"{val:N0}"; }, TextSize = 10, LabelsPaint = new SolidColorPaint(new SKColor(163, 174, 208)) } };
        }

        private void SetupDebtChart(ReportResultDto data)
        {
            if (data.DebtAxisLabels == null) return;

            DebtChartSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<double> { Name = "Nợ phát sinh", Values = data.DebtNewSeries?.Select(x => (double)x).ToArray() ?? Array.Empty<double>(), Stroke = new SolidColorPaint(new SKColor(238, 93, 80)) { StrokeThickness = 3 }, Fill = new SolidColorPaint(new SKColor(238, 93, 80, 30)), GeometrySize = 8, GeometryFill = new SolidColorPaint(SKColors.White), GeometryStroke = new SolidColorPaint(new SKColor(238, 93, 80)) { StrokeThickness = 2 } },
                new LineSeries<double> { Name = "Nợ thu hồi", Values = data.DebtPaidSeries?.Select(x => (double)x).ToArray() ?? Array.Empty<double>(), Stroke = new SolidColorPaint(new SKColor(5, 205, 153)) { StrokeThickness = 3 }, Fill = new SolidColorPaint(new SKColor(5, 205, 153, 30)), GeometrySize = 8, GeometryFill = new SolidColorPaint(SKColors.White), GeometryStroke = new SolidColorPaint(new SKColor(5, 205, 153)) { StrokeThickness = 2 } }
            };

            DebtXAxes = new[] { new Axis { Labels = data.DebtAxisLabels, TextSize = 11, LabelsPaint = new SolidColorPaint(new SKColor(43, 54, 116)) } };
            DebtYAxes = new[] { new Axis { Labeler = value => $"{value:N0} đ", TextSize = 11, } };
        }

        // ==========================================
        // XUẤT FILE

        private void ExportToExcel()
        {
            var FilterType = _selectedReportTypeIndex;
            switch (FilterType)
            {
                case 0: // Revenue
                    {
                        try
                        {
                            SaveFileDialog sfd = new SaveFileDialog()
                            {
                                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                                FileName = $"ChiTietDoanhThu_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                            };

                            if (sfd.ShowDialog() == true)
                            {
                                ExcelPackage.License.SetNonCommercialPersonal("Quan");

                                using (var package = new ExcelPackage())
                                {
                                    var sheet = package.Workbook.Worksheets.Add("Chi tiết Doanh Thu");

                                    string[] headers = { "Ngày", "Số Hoá Đơn", "Số Sách Bán", "Doanh thu gộp", "Giảm Giá", "Doanh Thu Thuần", "Tổng Vốn", "Lợi Nhuận Gộp" };
                                    for (int i = 0; i < headers.Length; i++)
                                    {
                                        var cell = sheet.Cells[1, i + 1];
                                        cell.Value = headers[i];
                                        cell.Style.Font.Bold = true;
                                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    }

                                    var dataToExport = _allRevenueRows.ToList();
                                    for (int i = 0; i < dataToExport.Count; i++)
                                    {
                                        var revenue = dataToExport[i];
                                        sheet.Cells[i + 2, 1].Value = $"{revenue.Date:dd/MM/yyyy}";
                                        sheet.Cells[i + 2, 2].Value = revenue.InvoiceCount;
                                        sheet.Cells[i + 2, 3].Value = revenue.BooksSold;
                                        sheet.Cells[i + 2, 4].Value = revenue.GrossProfit;
                                        sheet.Cells[i + 2, 5].Value = revenue.Discount;
                                        sheet.Cells[i + 2, 6].Value = revenue.NetRevenue;
                                        sheet.Cells[i + 2, 7].Value = revenue.TotalCost;
                                        sheet.Cells[i + 2, 8].Value = revenue.TotalAmount;

                                        sheet.Cells[i + 2, 2].Style.Numberformat.Format = "#,##0";
                                        sheet.Cells[i + 2, 3].Style.Numberformat.Format = "#,##0";
                                        sheet.Cells[i + 2, 4].Style.Numberformat.Format = "#,##0\" đ\"";
                                        sheet.Cells[i + 2, 5].Style.Numberformat.Format = "#,##0\" đ\"";
                                        sheet.Cells[i + 2, 6].Style.Numberformat.Format = "#,##0\" đ\"";
                                        sheet.Cells[i + 2, 7].Style.Numberformat.Format = "#,##0\" đ\"";
                                        sheet.Cells[i + 2, 8].Style.Numberformat.Format = "#,##0\" đ\"";

                                        sheet.Cells[i + 2, 6].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                                        sheet.Cells[i + 2, 6].Style.Font.Bold = true;

                                        sheet.Cells[i + 2, 7].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                                        sheet.Cells[i + 2, 7].Style.Font.Bold = true;

                                        sheet.Cells[i + 2, 8].Style.Font.Color.SetColor(revenue.TotalAmount >= 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red);
                                        sheet.Cells[i + 2, 8].Style.Font.Bold = true;
                                    }

                                    sheet.Cells.AutoFitColumns();

                                    File.WriteAllBytes(sfd.FileName, package.GetAsByteArray());

                                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    }
                case 1: // Inventory
                    {
                        try
                        {
                            SaveFileDialog sfd = new SaveFileDialog()
                            {
                                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                                FileName = $"ChiTietTonKho_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                            };

                            if (sfd.ShowDialog() == true)
                            {
                                ExcelPackage.License.SetNonCommercialPersonal("Quan");

                                using (var package = new ExcelPackage())
                                {
                                    var sheet = package.Workbook.Worksheets.Add("Chi tiết Tồn Kho");

                                    string[] headers = { "Mã ISBN", "Tựa Sách", "Thể Loại", "Tồn đầu", "Nhập", "Xuất", "Tồn Cuối", "Giá Trị Tồn" };
                                    for (int i = 0; i < headers.Length; i++)
                                    {
                                        var cell = sheet.Cells[1, i + 1];
                                        cell.Value = headers[i];
                                        cell.Style.Font.Bold = true;
                                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    }

                                    var dataToExport = _allInventoryRows.ToList();
                                    for (int i = 0; i < dataToExport.Count; i++)
                                    {
                                        var inventory = dataToExport[i];
                                        sheet.Cells[i + 2, 1].Value = i + 1;
                                        sheet.Cells[i + 2, 2].Value = inventory.BookId;
                                        sheet.Cells[i + 2, 3].Value = inventory.BookName;
                                        sheet.Cells[i + 2, 4].Value = inventory.CategoryName;
                                        sheet.Cells[i + 2, 5].Value = inventory.OpeningQty;
                                        sheet.Cells[i + 2, 6].Value = inventory.ImportedQty;
                                        sheet.Cells[i + 2, 7].Value = inventory.SoldQty;
                                        sheet.Cells[i + 2, 8].Value = inventory.ClosingQty;
                                        sheet.Cells[i + 2, 9].Value = inventory.StockValue;

                                        sheet.Cells[i + 2, 5].Style.Numberformat.Format = "#,##0";
                                        sheet.Cells[i + 2, 6].Style.Numberformat.Format = "#,##0";
                                        sheet.Cells[i + 2, 7].Style.Numberformat.Format = "#,##0";
                                        sheet.Cells[i + 2, 8].Style.Numberformat.Format = "#,##0";
                                        sheet.Cells[i + 2, 9].Style.Numberformat.Format = "#,##0\" đ\"";

                                        sheet.Cells[i + 2, 6].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                                        sheet.Cells[i + 2, 6].Style.Font.Bold = true;

                                        sheet.Cells[i + 2, 7].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                                        sheet.Cells[i + 2, 7].Style.Font.Bold = true;

                                        sheet.Cells[i + 2, 9].Style.Font.Color.SetColor(inventory.StockValue >= 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red);
                                        sheet.Cells[i + 2, 9].Style.Font.Bold = true;
                                    }

                                    if (dataToExport.Count > 0)
                                    {
                                        sheet.Cells[2, 1, dataToExport.Count + 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }

                                    sheet.Cells.AutoFitColumns();

                                    File.WriteAllBytes(sfd.FileName, package.GetAsByteArray());

                                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    }

                case 2: // Debt
                    {
                        try
                        {
                            SaveFileDialog sfd = new SaveFileDialog()
                            {
                                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                                FileName = $"ChiTietCongNo_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                            };

                            if (sfd.ShowDialog() == true)
                            {
                                ExcelPackage.License.SetNonCommercialPersonal("Quan");

                                using (var package = new ExcelPackage())
                                {
                                    var sheet = package.Workbook.Worksheets.Add("Chi tiết Công Nợ");

                                    string[] headers = { "STT", "Tên Khách Hàng", "Nợ Đầu", "Phát Sinh", "Thu Hồi", "Nợ Cuối" };
                                    for (int i = 0; i < headers.Length; i++)
                                    {
                                        var cell = sheet.Cells[1, i + 1];
                                        cell.Value = headers[i];
                                        cell.Style.Font.Bold = true;
                                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    }

                                    var dataToExport = _allDebtRows.ToList();
                                    for (int i = 0; i < dataToExport.Count; i++)
                                    {
                                        var debt = dataToExport[i];
                                        sheet.Cells[i + 2, 1].Value = i + 1;
                                        sheet.Cells[i + 2, 2].Value = debt.CustomerName;
                                        sheet.Cells[i + 2, 3].Value = debt.OpeningDebt;
                                        sheet.Cells[i + 2, 4].Value = debt.NewDebt;
                                        sheet.Cells[i + 2, 5].Value = debt.PaidDebt;
                                        sheet.Cells[i + 2, 6].Value = debt.ClosingDebt;

                                        sheet.Cells[i + 2, 3].Style.Numberformat.Format = "#,##0\" đ\"";
                                        sheet.Cells[i + 2, 4].Style.Numberformat.Format = "#,##0\" đ\"";
                                        sheet.Cells[i + 2, 5].Style.Numberformat.Format = "#,##0\" đ\"";
                                        sheet.Cells[i + 2, 6].Style.Numberformat.Format = "#,##0\" đ\"";

                                        sheet.Cells[i + 2, 3].Style.Font.Bold = true;
                                        sheet.Cells[i + 2, 4].Style.Font.Bold = true;
                                        sheet.Cells[i + 2, 5].Style.Font.Bold = true;
                                        sheet.Cells[i + 2, 6].Style.Font.Bold = true;

                                        sheet.Cells[i + 2, 5].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                                        sheet.Cells[i + 2, 4].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                                        sheet.Cells[i + 2, 6].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                                    }

                                    sheet.Cells.AutoFitColumns();

                                    if (dataToExport.Count > 0)
                                    {
                                        sheet.Cells[2, 1, dataToExport.Count + 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }

                                    File.WriteAllBytes(sfd.FileName, package.GetAsByteArray());

                                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Vui lòng chọn danh mục báo cáo hiển thị để xuất file Excel!", "Nhắc nhở", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
            }
        }

        private void ExportToPdf()
        {
            MessageBox.Show("Chức năng xuất PDF đang được phát triển.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}