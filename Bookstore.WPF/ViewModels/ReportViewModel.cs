using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
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

        private string _selectedCustomerType = "Tất cả khách hàng";
        public string SelectedCustomerType
        {
            get => _selectedCustomerType;
            set { _selectedCustomerType = value; OnPropertyChanged(nameof(SelectedCustomerType)); }
        }

        // Danh sách cho bộ lọc phụ
        public ObservableCollection<string> StaffList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CategoryList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CustomerTypeList { get; set; } = new ObservableCollection<string>();

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

        private string _analysisMessage = "Chưa có phân tích dữ liệu.";
        public string AnalysisMessage
        {
            get => _analysisMessage;
            set { _analysisMessage = value; OnPropertyChanged(nameof(AnalysisMessage)); }
        }

        private int _totalRows;
        public int TotalRows
        {
            get => _totalRows;
            set { _totalRows = value; OnPropertyChanged(nameof(TotalRows)); }
        }

        // --- PROPERTY PHÂN TRANG ---
        private int _pageSize = 10;

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
        }

        private int _totalPages = 1;
        public int TotalPages
        {
            get => _totalPages;
            set { _totalPages = value; OnPropertyChanged(nameof(TotalPages)); }
        }

        public ObservableCollection<int> PageNumbers { get; set; } = new ObservableCollection<int>();

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
            0 => "THỐNG KÊ DOANH THU & LỢI NHUẬN",
            1 => "THỐNG KÊ TỒN KHO",
            2 => "THỐNG KÊ CÔNG NỢ",
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

        private IEnumerable<Axis> _debtYAxes;
        public IEnumerable<Axis> DebtYAxes
        {
            get => _debtYAxes;
            set
            {
                _debtYAxes = value;
                OnPropertyChanged(nameof(DebtYAxes));
            }
        }

        // ==========================================
        // DỮ LIỆU BẢNG

        private List<RevenueReportRowDto> _allRevenueRows = new();
        private List<InventoryReportRowDto> _allInventoryRows = new();
        private List<DebtReportRowDto> _allDebtRows = new();

        public ObservableCollection<RevenueReportRowDto> RevenueRows { get; set; }
        public ObservableCollection<InventoryReportRowDto> InventoryRows { get; set; }
        public ObservableCollection<DebtReportRowDto> DebtRows { get; set; }

        // ==========================================
        // LỆNH

        public ICommand ApplyReportCommand { get; set; }
        public ICommand ExportExcelCommand { get; set;  }
        public ICommand ExportPdfCommand { get; set; }

        // Commands Phân trang
        public ICommand FirstPageCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand LastPageCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }

        // ==========================================
        // CONSTRUCTOR

        public ReportViewModel()
        {
            RevenueRows = new ObservableCollection<RevenueReportRowDto>();
            InventoryRows = new ObservableCollection<InventoryReportRowDto>();
            DebtRows = new ObservableCollection<DebtReportRowDto>();

            ApplyReportCommand = new RelayCommand<object>(async (p) =>
            {
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
                OnPropertyChanged(nameof(ChartIconKind));
                OnPropertyChanged(nameof(TableTitle));

                // Reset dữ liệu khi đổi loại báo cáo
                CurrentPage = 1;
                StatusText = "Nhấn 'Xem Báo Cáo' để tải dữ liệu.";

                UpdatePagedData();

                //UpdatePagedData();
                await LoadReportDataAsync();
            });

            ExportExcelCommand = new RelayCommand<object>(async (p) =>
            {
                ExportToExcel();
            });
            ExportPdfCommand = new RelayCommand<object>(async (p) =>
            {
                ExportToPdf();
            });

            FirstPageCommand = new RelayCommand<object>(p => { if (CurrentPage > 1) { CurrentPage = 1; UpdatePagedData(); } });
            PrevPageCommand = new RelayCommand<object>(p => { if (CurrentPage > 1) { CurrentPage--; UpdatePagedData(); } });
            NextPageCommand = new RelayCommand<object>(p => { if (CurrentPage < TotalPages) { CurrentPage++; UpdatePagedData(); } });
            LastPageCommand = new RelayCommand<object>(p => { if (CurrentPage < TotalPages) { CurrentPage = TotalPages; UpdatePagedData(); } });
            GoToPageCommand = new RelayCommand<object>(p =>
            {
                if (p is int pageNum && pageNum != CurrentPage)
                {
                    CurrentPage = pageNum;
                    UpdatePagedData();
                }
            });

            FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ToDate = DateTime.Today;

            // Load danh sách filter phụ từ API
            _ = LoadFilterListsAsync();

            _ = LoadReportDataAsync();
        }

        // ==========================================
        // LOAD DỮ LIỆU

        private void UpdatePagedData()
        {
            if (TotalPages == 0) CurrentPage = 1;

            RevenueRows.Clear();
            InventoryRows.Clear();
            DebtRows.Clear();

            int skip = (CurrentPage - 1) * _pageSize;

            // Lấy 10 dòng từ danh sách gốc tùy theo loại báo cáo
            if (SelectedReportTypeIndex == 0)
            {
                foreach (var item in _allRevenueRows.Skip(skip).Take(_pageSize)) RevenueRows.Add(item);
            }
            else if (SelectedReportTypeIndex == 1)
            {
                foreach (var item in _allInventoryRows.Skip(skip).Take(_pageSize)) InventoryRows.Add(item);
            }
            else if (SelectedReportTypeIndex == 2)
            {
                foreach (var item in _allDebtRows.Skip(skip).Take(_pageSize)) DebtRows.Add(item);
            }

            UpdatePageNumbers();
        }

        private void UpdatePageNumbers()
        {
            PageNumbers.Clear();
            if (TotalPages <= 0) return;

            int start = Math.Max(1, CurrentPage - 2);
            int end = Math.Min(TotalPages, start + 4);

            if (end - start < 4)
            {
                start = Math.Max(1, end - 4);
            }

            for (int i = start; i <= end; i++)
            {
                PageNumbers.Add(i);
            }
        }

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
                CustomerTypeList.Add("Tất cả khách hàng");
                if (customers != null) foreach (var c in customers) CustomerTypeList.Add(c);
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
                    // --- Cập nhật bảng dữ liệu ---
                    _allRevenueRows.Clear();
                    _allInventoryRows.Clear();
                    _allDebtRows.Clear();

                    if (data.RevenueRows != null) _allRevenueRows.AddRange(data.RevenueRows);
                    if (data.InventoryRows != null) _allInventoryRows.AddRange(data.InventoryRows);
                    if (data.DebtRows != null) _allDebtRows.AddRange(data.DebtRows);

                    //RevenueRows.Clear();
                    //InventoryRows.Clear();
                    //DebtRows.Clear();

                    //if (data.RevenueRows != null)
                    //    foreach (var r in data.RevenueRows) RevenueRows.Add(r);

                    //if (data.InventoryRows != null)
                    //    foreach (var r in data.InventoryRows) InventoryRows.Add(r);

                    //if (data.DebtRows != null)
                    //    foreach (var r in data.DebtRows) DebtRows.Add(r);


                    // --- Cập nhật trạng thái ---
                    int count = 0;
                    if (SelectedReportTypeIndex == 0) count = _allRevenueRows.Count;
                    else if (SelectedReportTypeIndex == 1) count = _allInventoryRows.Count;
                    else if (SelectedReportTypeIndex == 2) count = _allDebtRows.Count;
                    TotalRows = count;

                    // tính tổng trang
                    TotalPages = (int)Math.Ceiling((double)TotalRows / _pageSize);
                    if (TotalPages == 0) TotalPages = 1;
                    CurrentPage = 1;

                    if (TotalRows == 0)
                    {
                        AnalysisMessage = "Hệ thống chưa ghi nhận bất kỳ phát sinh nào trong khoảng thời gian này.";
                    }
                    else
                    {
                        if (SelectedReportTypeIndex == 0) // Báo cáo Doanh thu & Lợi nhuận
                        {
                            decimal totalNetRevenue = _allRevenueRows.Sum(x => x.NetRevenue);
                            decimal totalGrossProfit = _allRevenueRows.Sum(x => x.GrossProfit);
                            // Tính tỷ suất lợi nhuận gộp trung bình
                            decimal profitMargin = totalNetRevenue > 0 ? (totalGrossProfit / totalNetRevenue) * 100 : 0;

                            if (profitMargin >= 25)
                            {
                                AnalysisMessage = $"🔥 Tình hình kinh doanh xuất sắc! Tổng doanh thu đạt {totalNetRevenue:#,0} đ với biên lợi nhuận gộp rất cao ({profitMargin:N1}%), hoạt động kinh doanh đang tối ưu hiệu quả tốt.";
                            }
                            else if (profitMargin > 0 && profitMargin < 15)
                            {
                                AnalysisMessage = $"⚠️ Mặc dù doanh thu đạt {totalNetRevenue:#,0} đ nhưng biên lợi nhuận gộp khá mỏng ({profitMargin:N1}%), hãy kiểm tra lại giá vốn đầu vào hoặc giảm tần suất chương trình giảm giá.";
                            }
                            else
                            {
                                AnalysisMessage = $"✨ Kinh doanh ổn định. Tổng lợi nhuận gộp đạt {totalGrossProfit:#,0} đ (Hiệu suất đạt {profitMargin:N1}% trên tổng doanh thu thuần).";
                            }
                        }
                        else if (SelectedReportTypeIndex == 1) // Báo cáo Tồn kho
                        {
                            decimal totalStockValue = _allInventoryRows.Sum(x => x.StockValue);
                            var topSpamBook = _allInventoryRows.OrderByDescending(x => x.StockValue).FirstOrDefault();

                            AnalysisMessage = $"📦 Tổng giá trị hàng hóa đang lưu kho đạt {totalStockValue:#,0} đ. Trong đó, đầu sách '{topSpamBook?.BookName}' đang chiếm tỷ trọng đọng vốn cao nhất, cần cân nhắc đẩy mạnh khuyến mãi.";
                        }
                        else if (SelectedReportTypeIndex == 2) // Báo cáo Công nợ
                        {
                            decimal totalClosingDebt = _allDebtRows.Sum(x => x.ClosingDebt);
                            int customerInDebtCount = _allDebtRows.Count(x => x.HasDebt);

                            if (totalClosingDebt > 20_000_000) 
                            {
                                AnalysisMessage = $"🚨 Cảnh báo rủi ro! Hiện có {customerInDebtCount} khách hàng đang mua chịu với tổng công nợ đạt {totalClosingDebt:#,0} đ. Đề xuất siết chặt hạn mức bán nợ và ưu tiên phiếu thu tiền mặt.";
                            }
                            else
                            {
                                AnalysisMessage = $"✅ Chỉ số công nợ an toàn. Hệ thống đang kiểm soát tốt các khoản nợ phải thu với tổng số dư công nợ khách hàng là {totalClosingDebt:#,0} đ.";
                            }
                        }
                    }

                    UpdatePagedData();

                    await Task.Delay(400);

                    // --- Cập nhật biểu đồ ---
                    SetupRevenueChart(data);
                    SetupInventoryChart(data);
                    SetupDebtChart(data);


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
            var totalValues = new double[costValues.Length];
            for (int i = 0; i < costValues.Length; i++)
            {
                totalValues[i] = costValues[i] + profitValues[i];
            }

            RevenueChartSeries = new ObservableCollection<ISeries>
            {
                new StackedColumnSeries<double>
                {
                    Name = "Giá vốn",
                    Values = costValues,
                    Fill = new SolidColorPaint(SKColors.Tomato),       // #EE5D50 đỏ
                    Rx = 1, Ry = 1,
                    Stroke = null,
                    DataLabelsPaint = null
                },
                new StackedColumnSeries<double>
                {
                    Name = "Lợi nhuận",
                    Values = profitValues,
                    Fill = new SolidColorPaint(SKColors.MediumSpringGreen),       // #05CD99 xanh lá
                    Rx = 1, Ry = 1,
                    Stroke = null,
                    DataLabelsPaint = null
                }
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
                DataLabelsFormatter = point =>
                {
                    double val = point.Coordinate.PrimaryValue;
                    if (val >= 1_000_000) return $"{val / 1_000_000:N1}M"; // Hiện chữ M nếu > 1 triệu
                    if (val >= 1_000) return $"{val / 1_000:N0}K"; // Hiện chữ K nếu > 1 ngàn
                    return $"{val:N0}";
                },

                YToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:#,0} đ"
            });

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
                    //Labeler = value => $"{value / 1_000:N0}K đ",
                    Labeler = value => $"{value:N0}" + " đ",
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

            var invenValue = data.InventoryBarValues?.Select(x => (double)x).ToArray() ?? Array.Empty<double>();

            InventoryChartSeries = new ObservableCollection<ISeries>
            {
                new RowSeries<double>
                {
                    Name = "Giá trị tồn kho",
                    Values = invenValue,
                    Fill = new SolidColorPaint(new SKColor(67, 24, 255)),       // #4318FF tím
                    MaxBarWidth = 20,

                    XToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue / 1_000:N0}K đ",
            
                    // Giữ nguyên các dòng định dạng nhãn hiển thị trực tiếp trên thanh
                    DataLabelsPaint = new SolidColorPaint(new SKColor(43, 54, 116)),
                    DataLabelsSize = 10,
                    DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue / 1_000:N0}K đ"
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
                    Labeler = value => $"{value / 1_000:N0}K đ",
                    //Labeler = value => $"{value:N0}" + " đ",
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

            DebtYAxes = new[]
            {
                new Axis
                {
                    //Labeler = value => $"{value / 1_000:N0}K đ",
                    Labeler = value => $"{value:N0} đ",
                    TextSize = 11,
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
