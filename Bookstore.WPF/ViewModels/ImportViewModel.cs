using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class ImportViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ImportOrderModel> _allImportOrders;
        private Dictionary<string, ImportOrderDetailModel> _importOrderDetails;

        #region Constructor

        public ImportViewModel()
        {
            // Khởi tạo Commands
            OpenAddImportCommand = new RelayCommand(OpenAddImport);
            ClearFilterCommand = new RelayCommand(ClearFilter);
            ViewDetailCommand = new RelayCommand<ImportOrderModel>(ViewDetail);
            DeleteImportOrderCommand = new RelayCommand<ImportOrderModel>(DeleteImportOrder);

            // Commands cho popup thêm mới
            SearchBookCommand = new RelayCommand(SearchBook);
            AddBookToImportCommand = new RelayCommand(AddBookToImport);
            RemoveImportDetailCommand = new RelayCommand<ImportOrderAddDetailModel>(RemoveImportDetail);
            SaveImportOrderCommand = new RelayCommand(SaveImportOrder);

            PhanTrangCommand = new RelayCommand<string>(ExecutePhanTrang);

            LoadSampleData();
        }

        #endregion

        #region Properties - Data

        public ObservableCollection<ImportOrderModel> ImportOrders { get; set; } = new ObservableCollection<ImportOrderModel>();
        public ObservableCollection<SupplierModel> SupplierList { get; set; } = new ObservableCollection<SupplierModel>();

        private ImportOrderModel _selectedImportOrder;
        public ImportOrderModel SelectedImportOrder
        {
            get => _selectedImportOrder;
            set { _selectedImportOrder = value; OnPropertyChanged(); }
        }

        #endregion

        #region Properties - Filter

        private string _searchKeyword = string.Empty;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
                TrangHienTai = 1;
                FilterData();
            }
        }

        private DateTime? _filterFromDate;
        public DateTime? FilterFromDate
        {
            get => _filterFromDate;
            set
            {
                _filterFromDate = value;
                OnPropertyChanged();
                TrangHienTai = 1;
                FilterData();
            }
        }

        private DateTime? _filterToDate;
        public DateTime? FilterToDate
        {
            get => _filterToDate;
            set
            {
                _filterToDate = value;
                OnPropertyChanged();
                TrangHienTai = 1;
                FilterData();
            }
        }

        private SupplierModel _selectedSupplier;
        public SupplierModel SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged();
                TrangHienTai = 1;
                FilterData();
            }
        }

        #endregion

        #region Properties - Phân Trang
        private int _pageSize = 10; // Giữ nguyên kích thước trang của bạn

        private int _tongBanGhi = 0;
        public int TongBanGhi
        {
            get => _tongBanGhi;
            set { _tongBanGhi = value; OnPropertyChanged(); }
        }

        private int _trangHienTai = 1;
        public int TrangHienTai
        {
            get => _trangHienTai;
            set { _trangHienTai = value; OnPropertyChanged(); }
        }

        private int _tongSoTrang = 1;
        public int TongSoTrang
        {
            get => _tongSoTrang;
            set { _tongSoTrang = value; OnPropertyChanged(); }
        }
        #endregion

        #region Properties - Detail Popup

        private bool _isDetailPopupOpen;
        public bool IsDetailPopupOpen
        {
            get => _isDetailPopupOpen;
            set { _isDetailPopupOpen = value; OnPropertyChanged(); }
        }

        private ImportOrderDetailModel _detailImportOrder;
        public ImportOrderDetailModel DetailImportOrder
        {
            get => _detailImportOrder;
            set { _detailImportOrder = value; OnPropertyChanged(); }
        }

        #endregion

        #region Properties - Add Popup

        private bool _isAddPopupOpen;
        public bool IsAddPopupOpen
        {
            get => _isAddPopupOpen;
            set { _isAddPopupOpen = value; OnPropertyChanged(); }
        }

        private ImportOrderAddModel _newImportOrder;
        public ImportOrderAddModel NewImportOrder
        {
            get => _newImportOrder;
            set { _newImportOrder = value; OnPropertyChanged(); }
        }

        private string _searchBookKeyword = string.Empty;
        public string SearchBookKeyword
        {
            get => _searchBookKeyword;
            set { _searchBookKeyword = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BookModel> _searchBookResults;
        public ObservableCollection<BookModel> SearchBookResults
        {
            get => _searchBookResults;
            set { _searchBookResults = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands

        public ICommand OpenAddImportCommand { get; }
        public ICommand ClearFilterCommand { get; }
        public ICommand ViewDetailCommand { get; }
        public ICommand DeleteImportOrderCommand { get; }
        public ICommand PhanTrangCommand { get; }

        // Commands cho popup thêm mới
        public ICommand SearchBookCommand { get; }
        public ICommand AddBookToImportCommand { get; }
        public ICommand RemoveImportDetailCommand { get; }
        public ICommand SaveImportOrderCommand { get; }

        #endregion

        #region Public Methods

        public void CloseDetailPopup()
        {
            IsDetailPopupOpen = false;
            DetailImportOrder = null;
        }

        public void CloseAddPopup()
        {
            IsAddPopupOpen = false;
            NewImportOrder = null;
            SearchBookKeyword = string.Empty;
            SearchBookResults = null;
        }

        #endregion

        #region Private Methods - Main

        private void ViewDetail(ImportOrderModel order)
        {
            if (order != null && _importOrderDetails.ContainsKey(order.MaPhieuNhap))
            {
                DetailImportOrder = _importOrderDetails[order.MaPhieuNhap];
                IsDetailPopupOpen = true;
            }
            else if (order != null)
            {
                MessageBox.Show("Không tìm thấy chi tiết phiếu nhập này.", "Thông báo",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteImportOrder(ImportOrderModel order)
        {
            if (order == null) return;

            var result = MessageBox.Show(
                $"⚠️ XÁC NHẬN XÓA\n\nBạn có chắc chắn muốn xóa phiếu nhập {order.MaPhieuNhap}?\nHành động này không thể hoàn tác.",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _allImportOrders.Remove(order);
                _importOrderDetails?.Remove(order.MaPhieuNhap);
                FilterData();
                MessageBox.Show($"✅ Đã xóa phiếu nhập {order.MaPhieuNhap} thành công!", "Thành công",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OpenAddImport()
        {
            // Tạo mã phiếu tự động
            string newMaPhieu = $"PN{DateTime.Now:yyMMdd}{_allImportOrders.Count + 1:D3}";

            NewImportOrder = new ImportOrderAddModel
            {
                MaPhieuNhap = newMaPhieu,
                NgayNhap = DateTime.Now,
                GhiChu = string.Empty,
                SelectedSupplier = null,
                ImportDetailItems = new ObservableCollection<ImportOrderAddDetailModel>()
            };

            IsAddPopupOpen = true;
        }

        private void ClearFilter()
        {
            SearchKeyword = string.Empty;
            FilterFromDate = null;
            FilterToDate = null;
            SelectedSupplier = null;
            TrangHienTai = 1;
        }
        #endregion

        #region Logic Xử Lý Dữ Liệu & Phân Trang

        private void FilterData()
        {
            var filtered = _allImportOrders.AsEnumerable();

            // Lọc theo từ khóa
            if (!string.IsNullOrWhiteSpace(SearchKeyword))
            {
                string keyword = SearchKeyword.ToLower();
                filtered = filtered.Where(o =>
                    o.MaPhieuNhap.ToLower().Contains(keyword) ||
                    o.TenNhaCungCap.ToLower().Contains(keyword) ||
                    o.TenNguoiTao.ToLower().Contains(keyword) ||
                    (o.GhiChu != null && o.GhiChu.ToLower().Contains(keyword))
                );
            }

            // Lọc theo nhà cung cấp
            if (SelectedSupplier != null)
                filtered = filtered.Where(o => o.MaNhaCungCap == SelectedSupplier.MaNhaCungCap);

            // Lọc theo ngày
            if (FilterFromDate.HasValue)
                filtered = filtered.Where(o => o.NgayNhap.Date >= FilterFromDate.Value.Date);
            if (FilterToDate.HasValue)
                filtered = filtered.Where(o => o.NgayNhap.Date <= FilterToDate.Value.Date);

            // Sắp xếp
            filtered = filtered.OrderByDescending(o => o.NgayNhap);

            // CẬP NHẬT THÔNG TIN PHÂN TRANG
            TongBanGhi = filtered.Count();
            TongSoTrang = (int)Math.Ceiling((double)TongBanGhi / _pageSize);
            if (TongSoTrang == 0) TongSoTrang = 1;

            // Giới hạn trang hiện tại
            if (TrangHienTai > TongSoTrang) TrangHienTai = TongSoTrang;
            if (TrangHienTai < 1) TrangHienTai = 1;

            // Cắt dữ liệu đưa ra màn hình
            var pageData = filtered.Skip((TrangHienTai - 1) * _pageSize).Take(_pageSize).ToList();

            ImportOrders.Clear();
            foreach (var item in pageData)
            {
                ImportOrders.Add(item);
            }
        }

        private void ExecutePhanTrang(string parameter)
        {
            int targetPage = TrangHienTai;

            switch (parameter)
            {
                case "First":
                    targetPage = 1;
                    break;
                case "Prev":
                    if (TrangHienTai > 1) targetPage = TrangHienTai - 1;
                    break;
                case "Next":
                    if (TrangHienTai < TongSoTrang) targetPage = TrangHienTai + 1;
                    break;
                case "Last":
                    targetPage = TongSoTrang;
                    break;
            }

            if (targetPage != TrangHienTai)
            {
                GoToPage(targetPage);
            }
        }

        private void GoToPage(int page)
        {
            if (page >= 1 && page <= TongSoTrang)
            {
                TrangHienTai = page;
                FilterData(); // Gọi lại hàm FilterData để nó cắt đúng đoạn danh sách mới
            }
        }
        #endregion

        #region Private Methods - Add Popup

        private void SearchBook()
        {
            if (string.IsNullOrWhiteSpace(SearchBookKeyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.", "Thông báo",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var random = new Random();
            var results = new ObservableCollection<BookModel>();
            string[] tenSach = { "Doraemon", "Conan", "One Piece", "Harry Potter", "Sherlock Holmes", "Nhà giả kim", "Đắc nhân tâm", "Tôi tài giỏi bạn cũng thế", "Không gia đình", "Tiếng chim hót" };
            string[] tacGia = { "Fujiko Fujio", "Gosho Aoyama", "Oda Eiichiro", "J.K. Rowling", "Conan Doyle", "Paulo Coelho", "Dale Carnegie", "Adam Khoo", "Hector Malot", "Colin McCulough" };

            int count = random.Next(3, 7);
            for (int i = 0; i < count; i++)
            {
                results.Add(new BookModel
                {
                    MaSach = $"S{random.Next(100, 999):D3}",
                    TenSach = tenSach[random.Next(tenSach.Length)],
                    ISBN = $"978-604-{random.Next(1000, 9999)}-{random.Next(10, 99)}-{random.Next(0, 9)}",
                    TacGia = tacGia[random.Next(tacGia.Length)]
                });
            }

            SearchBookResults = results;
            MessageBox.Show($"Tìm thấy {results.Count} sách với từ khóa \"{SearchBookKeyword}\"",
                          "Kết quả tìm kiếm", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddBookToImport()
        {
            if (SearchBookResults == null || SearchBookResults.Count == 0)
            {
                MessageBox.Show("Chưa có kết quả tìm kiếm. Vui lòng tìm sách trước.",
                              "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NewImportOrder == null) return;

            // Lấy sách đầu tiên trong kết quả tìm kiếm
            var selectedBook = SearchBookResults.FirstOrDefault();
            if (selectedBook != null)
            {
                // Kiểm tra sách đã tồn tại chưa
                var existing = NewImportOrder.ImportDetailItems.FirstOrDefault(x => x.MaSach == selectedBook.MaSach);
                if (existing != null)
                {
                    MessageBox.Show($"Sách \"{selectedBook.TenSach}\" đã có trong danh sách.",
                                  "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NewImportOrder.ImportDetailItems.Add(new ImportOrderAddDetailModel
                {
                    STT = NewImportOrder.ImportDetailItems.Count + 1,
                    MaSach = selectedBook.MaSach,
                    TenSach = selectedBook.TenSach,
                    ISBN = selectedBook.ISBN,
                    TacGia = selectedBook.TacGia,
                    SoLuong = 0,
                    DonGia = 0
                });

                UpdateTotalAmount();
            }
        }

        private void RemoveImportDetail(ImportOrderAddDetailModel item)
        {
            if (item != null && NewImportOrder != null)
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa dòng này?",
                                           "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    NewImportOrder.ImportDetailItems.Remove(item);

                    // Cập nhật lại STT
                    for (int i = 0; i < NewImportOrder.ImportDetailItems.Count; i++)
                    {
                        NewImportOrder.ImportDetailItems[i].STT = i + 1;
                    }

                    UpdateTotalAmount();
                }
            }
        }

        private void SaveImportOrder()
        {
            if (NewImportOrder == null) return;

            // Validation
            if (NewImportOrder.SelectedSupplier == null)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp.", "Lỗi Validation",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewImportOrder.ImportDetailItems.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất 1 sách vào phiếu nhập.", "Lỗi Validation",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra số lượng và đơn giá
            foreach (var item in NewImportOrder.ImportDetailItems)
            {
                if (item.SoLuong <= 0)
                {
                    MessageBox.Show($"Sách \"{item.TenSach}\" có số lượng không hợp lệ. Vui lòng nhập số lượng > 0.",
                                  "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (item.DonGia <= 0)
                {
                    MessageBox.Show($"Sách \"{item.TenSach}\" có đơn giá không hợp lệ. Vui lòng nhập đơn giá > 0.",
                                  "Lỗi Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Xác nhận tạo phiếu
            var confirm = MessageBox.Show(
                $"Xác nhận tạo phiếu nhập {NewImportOrder.MaPhieuNhap}?\n" +
                $"Nhà cung cấp: {NewImportOrder.SelectedSupplier.TenNhaCungCap}\n" +
                $"Tổng tiền: {NewImportOrder.TongTien:N0} VNĐ",
                "Xác nhận tạo phiếu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            // Tạo phiếu nhập mới
            var newOrder = new ImportOrderModel
            {
                MaPhieuNhap = NewImportOrder.MaPhieuNhap,
                NgayNhap = NewImportOrder.NgayNhap,
                MaNhaCungCap = NewImportOrder.SelectedSupplier.MaNhaCungCap,
                TenNhaCungCap = NewImportOrder.SelectedSupplier.TenNhaCungCap,
                TenNguoiTao = "Admin",
                TongTien = NewImportOrder.TongTien,
                GhiChu = NewImportOrder.GhiChu
            };

            // Tạo chi tiết
            var details = new ObservableCollection<ImportOrderDetailItemModel>();
            foreach (var item in NewImportOrder.ImportDetailItems)
            {
                details.Add(new ImportOrderDetailItemModel
                {
                    STT = item.STT,
                    MaSach = item.MaSach,
                    TenSach = item.TenSach,
                    ISBN = item.ISBN,
                    TacGia = item.TacGia,
                    SoLuong = item.SoLuong,
                    DonGia = item.DonGia,
                    ThanhTien = item.SoLuong * item.DonGia
                });
            }

            // Lưu vào danh sách
            _allImportOrders.Insert(0, newOrder);
            _importOrderDetails[newOrder.MaPhieuNhap] = new ImportOrderDetailModel
            {
                MaPhieuNhap = newOrder.MaPhieuNhap,
                NgayNhap = newOrder.NgayNhap,
                TenNhaCungCap = newOrder.TenNhaCungCap,
                TenNguoiTao = newOrder.TenNguoiTao,
                TongTien = newOrder.TongTien,
                GhiChu = newOrder.GhiChu,
                ChiTietSach = details
            };

            // Đóng popup và refresh
            CloseAddPopup();
            FilterData();

            MessageBox.Show($"✅ Tạo phiếu nhập {newOrder.MaPhieuNhap} thành công!",
                          "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateTotalAmount()
        {
            if (NewImportOrder != null)
            {
                NewImportOrder.TongTien = NewImportOrder.ImportDetailItems.Sum(x => x.SoLuong * x.DonGia);
            }
        }

        #endregion

        #region Private Methods - Data

        private void LoadSampleData()
        {
            // Suppliers
            SupplierList = new ObservableCollection<SupplierModel>
            {
                new SupplierModel { MaNhaCungCap = "NCC001", TenNhaCungCap = "NXB Kim Đồng", SoDienThoai = "0281234567", Email = "kimdong@nxb.com", DiaChi = "TP.HCM" },
                new SupplierModel { MaNhaCungCap = "NCC002", TenNhaCungCap = "NXB Trẻ", SoDienThoai = "0287654321", Email = "nxbtre@nxb.com", DiaChi = "TP.HCM" },
                new SupplierModel { MaNhaCungCap = "NCC003", TenNhaCungCap = "NXB Giáo Dục", SoDienThoai = "0241234567", Email = "giaoduc@nxb.com", DiaChi = "Hà Nội" },
                new SupplierModel { MaNhaCungCap = "NCC004", TenNhaCungCap = "Fahasa", SoDienThoai = "0281122334", Email = "fahasa@book.com", DiaChi = "TP.HCM" },
                new SupplierModel { MaNhaCungCap = "NCC005", TenNhaCungCap = "Phương Nam Book", SoDienThoai = "0285566778", Email = "pnbook@book.com", DiaChi = "TP.HCM" }
            };

            // Import Orders
            _allImportOrders = new ObservableCollection<ImportOrderModel>
            {
                new ImportOrderModel { MaPhieuNhap = "PN001", NgayNhap = new DateTime(2026, 1, 15, 9, 30, 0), MaNhaCungCap = "NCC001", TenNhaCungCap = "NXB Kim Đồng", TenNguoiTao = "Admin", TongTien = 0, GhiChu = "Nhập sách thiếu nhi tháng 1" },
                new ImportOrderModel { MaPhieuNhap = "PN002", NgayNhap = new DateTime(2026, 2, 20, 14, 15, 0), MaNhaCungCap = "NCC002", TenNhaCungCap = "NXB Trẻ", TenNguoiTao = "Admin", TongTien = 0, GhiChu = "Nhập sách văn học" },
                new ImportOrderModel { MaPhieuNhap = "PN003", NgayNhap = new DateTime(2026, 3, 10, 10, 0, 0), MaNhaCungCap = "NCC003", TenNhaCungCap = "NXB Giáo Dục", TenNguoiTao = "Admin", TongTien = 0, GhiChu = "Nhập sách giáo khoa quý 2" },
                new ImportOrderModel { MaPhieuNhap = "PN004", NgayNhap = new DateTime(2026, 4, 5, 8, 45, 0), MaNhaCungCap = "NCC001", TenNhaCungCap = "NXB Kim Đồng", TenNguoiTao = "User1", TongTien = 0, GhiChu = "Nhập bổ sung truyện tranh" },
                new ImportOrderModel { MaPhieuNhap = "PN005", NgayNhap = new DateTime(2026, 5, 1, 11, 20, 0), MaNhaCungCap = "NCC004", TenNhaCungCap = "Fahasa", TenNguoiTao = "Admin", TongTien = 0, GhiChu = "Nhập sách tổng hợp tháng 5" }
            };

            var random = new Random();
            for (int i = 6; i <= 50; i++)
            {
                var supplier = SupplierList[random.Next(SupplierList.Count)];
                _allImportOrders.Add(new ImportOrderModel
                {
                    MaPhieuNhap = $"PN{i:D3}",
                    NgayNhap = new DateTime(2026, 1, 1, random.Next(8, 18), random.Next(0, 60), 0).AddDays(i * 3),
                    MaNhaCungCap = supplier.MaNhaCungCap,
                    TenNhaCungCap = supplier.TenNhaCungCap,
                    TenNguoiTao = i % 3 == 0 ? "Admin" : "User1",
                    TongTien = 0,
                    GhiChu = $"Phiếu nhập số {i}"
                });
            }

            GenerateDetailsData(random);
            FilterData();
        }

        private void GenerateDetailsData(Random random)
        {
            _importOrderDetails = new Dictionary<string, ImportOrderDetailModel>();
            string[] sachMau = { "Doraemon", "Conan", "One Piece", "Harry Potter", "Sherlock Holmes", "Nhà giả kim", "Đắc nhân tâm", "Tôi tài giỏi bạn cũng thế", "Không gia đình", "Tiếng chim hót" };
            string[] tacGia = { "Fujiko Fujio", "Gosho Aoyama", "Oda Eiichiro", "J.K. Rowling", "Conan Doyle", "Paulo Coelho", "Dale Carnegie", "Adam Khoo", "Hector Malot", "Colin McCulough" };

            foreach (var order in _allImportOrders)
            {
                int itemCount = random.Next(3, 8);
                var details = new ObservableCollection<ImportOrderDetailItemModel>();
                decimal tongTien = 0;

                for (int j = 0; j < itemCount; j++)
                {
                    int soLuong = random.Next(10, 100);
                    decimal donGia = random.Next(50000, 300000);
                    decimal thanhTien = soLuong * donGia;

                    details.Add(new ImportOrderDetailItemModel
                    {
                        STT = j + 1,
                        MaSach = $"S{random.Next(100, 999):D3}",
                        TenSach = sachMau[random.Next(sachMau.Length)],
                        ISBN = $"978-604-{random.Next(1000, 9999)}-{random.Next(10, 99)}-{random.Next(0, 9)}",
                        TacGia = tacGia[random.Next(tacGia.Length)],
                        SoLuong = soLuong,
                        DonGia = donGia,
                        ThanhTien = thanhTien
                    });
                    tongTien += thanhTien;
                }

                order.TongTien = tongTien;
                _importOrderDetails[order.MaPhieuNhap] = new ImportOrderDetailModel
                {
                    MaPhieuNhap = order.MaPhieuNhap,
                    NgayNhap = order.NgayNhap,
                    TenNhaCungCap = order.TenNhaCungCap,
                    TenNguoiTao = order.TenNguoiTao,
                    TongTien = tongTien,
                    GhiChu = order.GhiChu,
                    ChiTietSach = details
                };
            }
        }

  

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    #region Models

    public class ImportOrderModel : INotifyPropertyChanged
    {
        private string _maPhieuNhap;
        public string MaPhieuNhap { get => _maPhieuNhap; set { _maPhieuNhap = value; OnPropertyChanged(); } }

        private DateTime _ngayNhap;
        public DateTime NgayNhap { get => _ngayNhap; set { _ngayNhap = value; OnPropertyChanged(); } }

        private string _maNhaCungCap;
        public string MaNhaCungCap { get => _maNhaCungCap; set { _maNhaCungCap = value; OnPropertyChanged(); } }

        private string _tenNhaCungCap;
        public string TenNhaCungCap { get => _tenNhaCungCap; set { _tenNhaCungCap = value; OnPropertyChanged(); } }

        private string _tenNguoiTao;
        public string TenNguoiTao { get => _tenNguoiTao; set { _tenNguoiTao = value; OnPropertyChanged(); } }

        private decimal _tongTien;
        public decimal TongTien { get => _tongTien; set { _tongTien = value; OnPropertyChanged(); } }

        private string _ghiChu;
        public string GhiChu { get => _ghiChu; set { _ghiChu = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class SupplierModel : INotifyPropertyChanged
    {
        private string _maNhaCungCap;
        public string MaNhaCungCap { get => _maNhaCungCap; set { _maNhaCungCap = value; OnPropertyChanged(); } }

        private string _tenNhaCungCap;
        public string TenNhaCungCap { get => _tenNhaCungCap; set { _tenNhaCungCap = value; OnPropertyChanged(); } }

        private string _soDienThoai;
        public string SoDienThoai { get => _soDienThoai; set { _soDienThoai = value; OnPropertyChanged(); } }

        private string _email;
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        private string _diaChi;
        public string DiaChi { get => _diaChi; set { _diaChi = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class ImportOrderDetailModel : INotifyPropertyChanged
    {
        private string _maPhieuNhap;
        public string MaPhieuNhap { get => _maPhieuNhap; set { _maPhieuNhap = value; OnPropertyChanged(); } }

        private DateTime _ngayNhap;
        public DateTime NgayNhap { get => _ngayNhap; set { _ngayNhap = value; OnPropertyChanged(); } }

        private string _tenNhaCungCap;
        public string TenNhaCungCap { get => _tenNhaCungCap; set { _tenNhaCungCap = value; OnPropertyChanged(); } }

        private string _tenNguoiTao;
        public string TenNguoiTao { get => _tenNguoiTao; set { _tenNguoiTao = value; OnPropertyChanged(); } }

        private decimal _tongTien;
        public decimal TongTien { get => _tongTien; set { _tongTien = value; OnPropertyChanged(); } }

        private string _ghiChu;
        public string GhiChu { get => _ghiChu; set { _ghiChu = value; OnPropertyChanged(); } }

        private ObservableCollection<ImportOrderDetailItemModel> _chiTietSach;
        public ObservableCollection<ImportOrderDetailItemModel> ChiTietSach { get => _chiTietSach; set { _chiTietSach = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class ImportOrderDetailItemModel : INotifyPropertyChanged
    {
        private int _stt;
        public int STT { get => _stt; set { _stt = value; OnPropertyChanged(); } }

        private string _maSach;
        public string MaSach { get => _maSach; set { _maSach = value; OnPropertyChanged(); } }

        private string _tenSach;
        public string TenSach { get => _tenSach; set { _tenSach = value; OnPropertyChanged(); } }

        private string _isbn;
        public string ISBN { get => _isbn; set { _isbn = value; OnPropertyChanged(); } }

        private string _tacGia;
        public string TacGia { get => _tacGia; set { _tacGia = value; OnPropertyChanged(); } }

        private int _soLuong;
        public int SoLuong { get => _soLuong; set { _soLuong = value; OnPropertyChanged(); } }

        private decimal _donGia;
        public decimal DonGia { get => _donGia; set { _donGia = value; OnPropertyChanged(); } }

        private decimal _thanhTien;
        public decimal ThanhTien { get => _thanhTien; set { _thanhTien = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Models cho popup Thêm mới
    public class ImportOrderAddModel : INotifyPropertyChanged
    {
        private string _maPhieuNhap;
        public string MaPhieuNhap { get => _maPhieuNhap; set { _maPhieuNhap = value; OnPropertyChanged(); } }

        private DateTime _ngayNhap = DateTime.Now;
        public DateTime NgayNhap { get => _ngayNhap; set { _ngayNhap = value; OnPropertyChanged(); } }

        private SupplierModel _selectedSupplier;
        public SupplierModel SelectedSupplier { get => _selectedSupplier; set { _selectedSupplier = value; OnPropertyChanged(); } }

        private string _ghiChu;
        public string GhiChu { get => _ghiChu; set { _ghiChu = value; OnPropertyChanged(); } }

        private decimal _tongTien;
        public decimal TongTien { get => _tongTien; set { _tongTien = value; OnPropertyChanged(); } }

        private ObservableCollection<ImportOrderAddDetailModel> _importDetailItems;
        public ObservableCollection<ImportOrderAddDetailModel> ImportDetailItems { get => _importDetailItems; set { _importDetailItems = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class ImportOrderAddDetailModel : INotifyPropertyChanged
    {
        private int _stt;
        public int STT { get => _stt; set { _stt = value; OnPropertyChanged(); } }

        private string _maSach;
        public string MaSach { get => _maSach; set { _maSach = value; OnPropertyChanged(); } }

        private string _tenSach;
        public string TenSach { get => _tenSach; set { _tenSach = value; OnPropertyChanged(); } }

        private string _isbn;
        public string ISBN { get => _isbn; set { _isbn = value; OnPropertyChanged(); } }

        private string _tacGia;
        public string TacGia { get => _tacGia; set { _tacGia = value; OnPropertyChanged(); } }

        private int _soLuong;
        public int SoLuong { get => _soLuong; set { _soLuong = value; OnPropertyChanged(); OnPropertyChanged(nameof(ThanhTien)); } }

        private decimal _donGia;
        public decimal DonGia { get => _donGia; set { _donGia = value; OnPropertyChanged(); OnPropertyChanged(nameof(ThanhTien)); } }

        public decimal ThanhTien => SoLuong * DonGia;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class BookModel : INotifyPropertyChanged
    {
        private string _maSach;
        public string MaSach { get => _maSach; set { _maSach = value; OnPropertyChanged(); } }

        private string _tenSach;
        public string TenSach { get => _tenSach; set { _tenSach = value; OnPropertyChanged(); } }

        private string _isbn;
        public string ISBN { get => _isbn; set { _isbn = value; OnPropertyChanged(); } }

        private string _tacGia;
        public string TacGia { get => _tacGia; set { _tacGia = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region RelayCommand

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
        public void Execute(object parameter) => _execute();
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return parameter is T t && (_canExecute?.Invoke(t) ?? true);
        }

        public void Execute(object parameter)
        {
            if (parameter is T t)
                _execute(t);
        }
    }

    #endregion
}