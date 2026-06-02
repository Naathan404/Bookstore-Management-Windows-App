using Bookstore.Share.DTOs;
using Bookstore.WPF.Models;
using Bookstore.WPF.Services;
using Bookstore.WPF.Views.Components;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        #region Collections
        private ObservableCollection<Models.BookItem> _allBooks;
        private ObservableCollection<Models.BookItem> _filteredBooks;
        public ObservableCollection<string> ListNhaCungCap { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ListNhaXuatBan { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Models.BookItem> ListTacPhamGoc { get; set; } = new ObservableCollection<Models.BookItem>();
        public ObservableCollection<TacGiaDTO> ListTatCaTacGia { get; set; } = new ObservableCollection<TacGiaDTO>();
        private TacGiaDTO _selectedTacGiaToAdd;
        public TacGiaDTO SelectedTacGiaToAdd
        {
            get => _selectedTacGiaToAdd;
            set
            {
                _selectedTacGiaToAdd = value;
                OnPropertyChanged();

                if (value != null && EditingBook != null)
                {
                    if (!EditingBook.DanhSachTacGia.Any(tg => tg.Id == value.Id))
                    {
                        EditingBook.DanhSachTacGia.Add(value);
                    }

                    Application.Current.Dispatcher.InvokeAsync(() => {
                        SelectedTacGiaToAdd = null;
                        InputTacGiaText = string.Empty;
                    });
                }
            }
        }

        public ObservableCollection<string> ListHinhThucBia { get; set; }

        // Danh sách sách hiển thị trên 1 trang
        public ObservableCollection<Models.BookItem> PagedBooks { get; set; }

        // Danh sách Thể loại cho ComboBox
        public ObservableCollection<string> ListTheLoai { get; set; }
        public ObservableCollection<string> ListTheLoaiTaoSach { get; set; }
        public ObservableCollection<int> PageNumbers { get; set; }

        public List<string> ListPriceRangeIndex { get; set; } = new List<string>
        {
            "Tất cả mức giá",
            "Dưới 50.000đ",
            "50.000đ - 100.000đ",
            "100.000đ - 200.000đ",
            "Trên 200.000đ"
        };
        #endregion

        #region Properties - Tìm Kiếm
        private string _searchTenSach = string.Empty;
        public string SearchTenSach 
        { 
            get => _searchTenSach; 
            set 
            { 
                _searchTenSach = value; 
                OnPropertyChanged(); 
                PerformSearch(); 
            } 
        }

        private string _searchTacGia = string.Empty;
        public string SearchTacGia 
        { 
            get => _searchTacGia; 
            set 
            { _searchTacGia = value; 
                OnPropertyChanged(); 
                PerformSearch(); 
            } 
        }

        private string _selectedTheLoai = string.Empty;
        public string SelectedTheLoai 
        { 
            get => _selectedTheLoai; 
            set 
            { 
                _selectedTheLoai = value; 
                OnPropertyChanged();
                PerformSearch(); 
            } 
        }

        private string _searchTonKho;
        public string SearchTonKho
        {
            get => _searchTonKho;
            set 
            { 
                _searchTonKho = value; 
                OnPropertyChanged();
                PerformSearch();
            }
        }

        // filter lọc theo mức giá
        private int _selectedPriceRangeIndex = 0; // Mặc định là 0 (Tất cả mức giá)
        public int SelectedPriceRangeIndex
        {
            get => _selectedPriceRangeIndex;
            set
            {
                _selectedPriceRangeIndex = value;
                OnPropertyChanged();
                PerformSearch();
            }
        }

        private decimal _tiLeGiaBan = 1.0m;

        // NOTE Để bổ sung các properties còn thíu

        #endregion

        #region Properties - Phân Trang
        private int _trangHienTai = 1;
        public int TrangHienTai { get => _trangHienTai; set { _trangHienTai = value; OnPropertyChanged(); } }

        private int _tongSoTrang = 1;
        public int TongSoTrang { get => _tongSoTrang; set { _tongSoTrang = value; OnPropertyChanged(); } }

        private int _tongBanGhi = 0; // Thêm biến này để hiển thị "(Tổng: ...)" trên UI
        public int TongBanGhi { get => _tongBanGhi; set { _tongBanGhi = value; OnPropertyChanged(); } }

        private int _pageSize = 10; // Biến này giữ nguyên để làm tham số gọi API/Database
        #endregion

        // Khai báo 1 Command duy nhất thay vì 5 cái như trước

        #region Properties - Popup Thêm/Sửa
        //private Visibility _isPopupVisible = Visibility.Collapsed;
        //public Visibility IsPopupVisible { get => _isPopupVisible; set { _isPopupVisible = value; OnPropertyChanged(); } }

        private string _popupTitle;
        public string PopupTitle { get => _popupTitle; set { _popupTitle = value; OnPropertyChanged(); } }

        private PackIconKind _popupIcon;
        public PackIconKind PopupIcon { get => _popupIcon; set { _popupIcon = value; OnPropertyChanged(); } }

        private Visibility _isAddPopupVisible = Visibility.Collapsed;
        public Visibility IsAddPopupVisible { get => _isAddPopupVisible; set { _isAddPopupVisible = value; OnPropertyChanged(); } }

        private Visibility _isEditPopupVisible = Visibility.Collapsed;
        public Visibility IsEditPopupVisible { get => _isEditPopupVisible; set { _isEditPopupVisible = value; OnPropertyChanged(); } }

        private bool _isEditMasterEnabled = false;
        public bool IsEditMasterEnabled
        {
            get => _isEditMasterEnabled;
            set
            {
                _isEditMasterEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isNewProduct = true;
        public bool IsNewProduct
        {
            get => _isNewProduct;
            set
            {
                _isNewProduct = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOldProduct));

                if (value)
                {
                    SelectedTacPhamGoc = null;
                }
            }
        }

        public bool IsOldProduct
        {
            get => !_isNewProduct;
            set
            {
                IsNewProduct = !value;
            }
        }
        private Models.BookItem _selectedTacPhamGoc;
        public Models.BookItem SelectedTacPhamGoc
        {
            get => _selectedTacPhamGoc;
            set
            {
                _selectedTacPhamGoc = value;
                OnPropertyChanged();

                if (value != null && EditingBook != null)
                {
                    EditingBook.TenSach = value.TenSach;
                    EditingBook.DanhSachTacGia = value.DanhSachTacGia;
                    EditingBook.TheLoai = value.TheLoai;
                    EditingBook.MoTa = value.MoTa;
                    EditingBook.HinhAnh = value.HinhAnh;
                }
            }
        }

        private string _inputTacGiaText;
        public string InputTacGiaText
        {
            get => _inputTacGiaText;
            set
            {
                _inputTacGiaText = value;
                OnPropertyChanged();

                var view = System.Windows.Data.CollectionViewSource.GetDefaultView(ListTatCaTacGia);
                view.Filter = (obj) =>
                {
                    if (string.IsNullOrWhiteSpace(value)) return true;
                    var tg = obj as TacGiaDTO;
                    if (tg == null) return false;
                    return tg.TenTacGia.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
                };
            }
        }

        private int _totalRecords = 0;
        public int TotalRecords
        {
            get => _totalRecords;
            set
            {
                _totalRecords = value;
                OnPropertyChanged();
            }
        }

        // Biến chứa dữ liệu sách đang được thêm hoặc sửa
        private Models.BookItem _editingBook;
        public Models.BookItem EditingBook { get => _editingBook; set { _editingBook = value; OnPropertyChanged(); } }

        private bool _isAddingNew;
        public bool IsAddingNew
        {
            get { return _isAddingNew; }
            set
            {
                _isAddingNew = value;
                OnPropertyChanged(nameof(IsAddingNew));
            }
        }

        private bool _isViewByVersion = true;
        public bool IsViewByVersion
        {
            get => _isViewByVersion;
            set
            {
                _isViewByVersion = value;
                OnPropertyChanged();
                _ = LoadDataAsync();
            }
        }

        private float _sellingPriceRatio = 1f;
        public float SellingPriceRatio
        {
            get => _sellingPriceRatio;
            set
            {
                _sellingPriceRatio = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Commands
        // mở đóng pop up
        public ICommand OpenAddPopupCommand { get; set; }
        public ICommand OpenEditPopupCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }

        // các thao tác cập nhật
        public ICommand SaveNewBookCommand { get; set; }
        public ICommand SaveEditBookCommand { get; set; }
        public ICommand DeleteBookCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand RemoveTacGiaCommand { get; set; }

        // nút thêm nhanh các dannh mục
        public ICommand AddNewTacGiaCommand { get; set; }
        public ICommand AddNewTheLoaiCommand { get; set; }
        public ICommand AddNewNXBCommand { get; set; }

        // xuất excel
        public ICommand ExportExcelCommand { get; set; }


        // Phân trang Commands
        public ICommand PhanTrangCommand { get; set; }
        #endregion

        // ===========================================================================================
        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ProductViewModel()
        {
            _allBooks = new ObservableCollection<Models.BookItem>();
            _filteredBooks = new ObservableCollection<Models.BookItem>();
            PagedBooks = new ObservableCollection<Models.BookItem>();
            PageNumbers = new ObservableCollection<int>();
            ListTheLoai = new ObservableCollection<string>();
            ListTheLoaiTaoSach = new ObservableCollection<string>();

            ListHinhThucBia = new ObservableCollection<string>
            {
                "Bìa mềm",
                "Bìa cứng",
                "Bìa gập",
                "Bìa rời",
                "Bìa da",
                "Khác"
            };

            IsAddPopupVisible = IsEditPopupVisible = Visibility.Hidden;
            EditingBook = new Models.BookItem();
            InitCommands();

            _ = LoadTheLoaiAsync();
            _ = LoadNhaXuatBanAsync();
            _ = LoadTacGiaAsync();
            _ = LoadDataAsync();
        }

        public async Task LoadMasterData()
        {
            _ = LoadTheLoaiAsync();
            _ = LoadNhaXuatBanAsync();
            _ = LoadTiLeGiaBanAsync();
            _ = LoadTacGiaAsync();
            _ = LoadDataAsync();
        }

        private void InitCommands()
        {
            OpenAddPopupCommand = new RelayCommand<object>((p) => {
                IsNewProduct = true; // Mặc định là đầu sách mới
                EditingBook = new Models.BookItem
                {
                    HinhAnh = "/Resources/Images/Books/default_book_cover.jpg",
                    SoLuongTonKho = 0,
                    TiLeGiaBan = _tiLeGiaBan
                };
                IsAddPopupVisible = Visibility.Visible;
                PopupIcon = PackIconKind.BookPlus;
                PopupTitle = "THÊM SÁCH MỚI";
            });

            // COMMAND MỞ POPUP SỬA
            OpenEditPopupCommand = new RelayCommand<Models.BookItem>((book) => 
            {
                if (book == null) return;
                EditingBook = new Models.BookItem
                {
                    Id = book.Id,
                    STT = book.STT,
                    TenSach = book.TenSach,
                    TheLoai = book.TheLoai,
                    MoTa = book.MoTa,
                    HinhAnh = book.HinhAnh,
                    ISBN = book.ISBN,
                    NamXuatBan = book.NamXuatBan,
                    LanTaiBan = book.LanTaiBan,
                    NhaXuatBan = book.NhaXuatBan,
                    HinhThucBia = book.HinhThucBia,
                    SoLuongTonKho = book.SoLuongTonKho,
                    TongDaBan = book.TongDaBan,
                    TiLeGiaBan = _tiLeGiaBan,
                    GiaNiemYet = book.GiaNiemYet,
                    DonGiaBan = book.DonGiaBan,
                };

                EditingBook.ResetManualFlag();

                for (int i = 0; i < book.DanhSachTacGia.Count; i++)
                {
                    var tg = book.DanhSachTacGia[i];
                    EditingBook.DanhSachTacGia.Add(new TacGiaDTO
                    { 
                        Id = tg.Id, 
                        TenTacGia = tg.TenTacGia 
                    });
                }

                IsEditMasterEnabled = false;
                IsEditPopupVisible = Visibility.Visible;
                PopupIcon = PackIconKind.BookEdit;
                PopupTitle = "ĐIỀU CHỈNH SÁCH";
            });

            ClosePopupCommand = new RelayCommand<object>((p) =>
            {
                IsAddPopupVisible = Visibility.Hidden;
                IsEditPopupVisible = Visibility.Hidden;
            });

            /// Lưu sách mới
            SaveNewBookCommand = new RelayCommand<object>(async (p) => {
                if (!ValidateInput(isAdding: true)) return;

                if (IsOldProduct && SelectedTacPhamGoc == null)
                {
                    MessageBox.Show("Vui lòng chọn một đầu sách có sẵn!"); return;
                }
                if (string.IsNullOrWhiteSpace(EditingBook.TenSach) || string.IsNullOrWhiteSpace(EditingBook.ISBN))
                {
                    MessageBox.Show("Tên sách và ISBN không được để trống!"); return;
                }

                var dto = new SachDTO
                {
                    IsTacPhamMoi = IsNewProduct,
                    MaSachGoc = IsOldProduct ? SelectedTacPhamGoc.Id : 0,
                    TenSach = EditingBook.TenSach,
                    ISBN = EditingBook.ISBN,
                    TheLoai = EditingBook.TheLoai,
                    NhaXuatBan = EditingBook.NhaXuatBan,
                    MoTa = EditingBook.MoTa,
                    HinhAnh = EditingBook.HinhAnh,
                    NamXuatBan = EditingBook.NamXuatBan,
                    LanTaiBan = EditingBook.LanTaiBan,
                    HinhThucBia = EditingBook.HinhThucBia,
                    GiaNiemYet = EditingBook.GiaNiemYet,
                    DonGiaBan = EditingBook.DonGiaBan,
                    SoLuongTonKho = EditingBook.SoLuongTonKho,
                    DanhSachTacGia = EditingBook.DanhSachTacGia.ToList()
                };

                // Gọi API
                bool success = await ApiClient.PostAndCheckSuccessAsync("api/PhienBanSach", dto);
                if (success)
                {
                    MessageBox.Show("Thêm sách thành công!");
                    IsAddPopupVisible = Visibility.Hidden;
                    _ = LoadDataAsync(); // refresh trang
                }
            });

            // Lưu thông tin khui điều hcinhr sách
            SaveEditBookCommand = new RelayCommand<object>(async (p) => {

                if (!ValidateInput(isAdding: false)) return;

                var dto = new SachDTO
                {
                    Id = EditingBook.Id, 
                    ISBN = EditingBook.ISBN,
                    TenSach = EditingBook.TenSach,
                    TheLoai = EditingBook.TheLoai,
                    MoTa = EditingBook.MoTa,
                    HinhAnh = EditingBook.HinhAnh,
                    NhaXuatBan = EditingBook.NhaXuatBan,
                    NamXuatBan = EditingBook.NamXuatBan,
                    LanTaiBan = EditingBook.LanTaiBan,
                    HinhThucBia = EditingBook.HinhThucBia,
                    GiaNiemYet = EditingBook.GiaNiemYet,
                    DonGiaBan = EditingBook.DonGiaBan,

                    IsTacPhamMoi = IsEditMasterEnabled
                };
                dto.DanhSachTacGia = EditingBook.DanhSachTacGia.ToList();

                bool success = await ApiClient.PutAndCheckSuccessAsync($"api/PhienBanSach/{dto.ISBN}", dto);
                if (success)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    IsEditPopupVisible = Visibility.Hidden;
                    _ = LoadDataAsync();
                }
            });

            // --- xóa sách---
            DeleteBookCommand = new RelayCommand<Models.BookItem>(async (book) =>
            {
                if (book == null) return;

                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sách: {book.TenSach}\n(Mã ISBN: {book.ISBN})?\n\nLưu ý: Hành động này không thể hoàn tác!",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // call api
                    bool isSuccess = await ApiClient.DeleteAndCheckSuccessAsync($"api/PhienBanSach/{book.ISBN}");

                    if (isSuccess)
                    {
                        // xóa ở db ok thì xóa trên ui
                        Application.Current.Dispatcher.Invoke(() => {
                            var itemInList = _allBooks.FirstOrDefault(b => b.ISBN == book.ISBN);
                            if (itemInList != null) _allBooks.Remove(itemInList);

                            PerformSearch();
                        });

                        MessageBox.Show("Đã xóa sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // nneeus api lỗi
                        MessageBox.Show("Không thể xóa sách này vì đã có dữ liệu liên quan (Hóa đơn hoặc Phiếu nhập)!", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });

            ChangeImageCommand = new RelayCommand<object>((p) =>
            {
                var openFileDialog = new OpenFileDialog { Filter = "Image files (*.png;*.jpg)|*.png;*.jpg" };
                if (openFileDialog.ShowDialog() == true)
                {
                    EditingBook.HinhAnh = openFileDialog.FileName;
                }
            });

            RemoveTacGiaCommand = new RelayCommand<TacGiaDTO>((tacGiaXoa) =>
            {
                if (tacGiaXoa != null && EditingBook != null)
                {
                    EditingBook.DanhSachTacGia.Remove(tacGiaXoa);
                }
            });

            // xuất excel
            ExportExcelCommand = new RelayCommand<object>((p) =>
            {
                MessageBox.Show("Chức năng đang trong quá trình phát triển. Vui lòng quay lại sau!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            });


            // thêm tác giả
            AddNewTacGiaCommand = new RelayCommand<string>(async (tenTacGiaMoi) =>
            {
                if (string.IsNullOrWhiteSpace(tenTacGiaMoi))
                {
                    MessageBox.Show("Vui lòng nhập tên tác giả!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }    

                MessageBox.Show($"Bạn có muốn thêm tác giả '{tenTacGiaMoi}' không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                var tacGiaMoiTao = new TacGiaDTO { TenTacGia = tenTacGiaMoi };
                    bool isSuccess = await ApiClient.PostAndCheckSuccessAsync("api/TacGia", tacGiaMoiTao);

                if (isSuccess)
                {
                    await LoadTacGiaAsync();

                    var tacGiaTuDB = ListTatCaTacGia.FirstOrDefault(t => t.TenTacGia.ToLower() == tenTacGiaMoi.ToLower());
        
                    if (tacGiaTuDB != null && EditingBook != null)
                    {
                        EditingBook.DanhSachTacGia.Add(tacGiaTuDB);
                    }

                    InputTacGiaText = string.Empty;
                }
                else
                {
                    MessageBox.Show("Thêm tác giả thất bại!");
                }
            });

            // thêm thể loại
            AddNewTheLoaiCommand = new RelayCommand<string>(async (tenTheLoai) =>
            {
                if (string.IsNullOrWhiteSpace(tenTheLoai))
                {
                    MessageBox.Show("Vui lòng nhập tên thể loại cần thêm!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Thêm thể loại mới: '{tenTheLoai}'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool isSuccess = await ApiClient.PostAndCheckSuccessAsync("api/TheLoai", new { TenTheLoai = tenTheLoai });
                    if (isSuccess)
                    {
                        await LoadTheLoaiAsync();
                        EditingBook.TheLoai = tenTheLoai;
                        MessageBox.Show("Thêm thành công!");
                    }
                    else MessageBox.Show("Lỗi khi thêm Thể loại!");
                }
            });

            // thêm nxb
            AddNewNXBCommand = new RelayCommand<string>(async (tenNXB) =>
            {
                if (string.IsNullOrWhiteSpace(tenNXB))
                {
                    MessageBox.Show("Vui lòng nhập tên Nhà xuất bản cần thêm!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Thêm Nhà xuất bản mới: '{tenNXB}'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool isSuccess = await ApiClient.PostAndCheckSuccessAsync("api/NhaXuatBan", new { TenNhaXuatBan = tenNXB });
                    if (isSuccess)
                    {
                        await LoadNhaXuatBanAsync(); 
                        EditingBook.NhaXuatBan = tenNXB;
                        MessageBox.Show("Thêm thành công!");
                    }
                    else MessageBox.Show("Lỗi khi thêm Nhà xuất bản!");
                }
            });

            ClearFilterCommand = new RelayCommand<object>((p) => {
                SearchTenSach = string.Empty;
                SearchTacGia = string.Empty;
                SelectedTheLoai = null;
                SelectedPriceRangeIndex = 0;
                SearchTonKho = string.Empty;

                PerformSearch();
            });

            RefreshCommand = new RelayCommand<object>(async p => await LoadMasterData());

            // Phân trang commands
            PhanTrangCommand = new RelayCommand<string>(ExecutePhanTrang);
        }

        // ===========================================================================================
        /// <summary>
        /// Xử lý tìm kiếm
        /// </summary>
        private void PerformSearch()
        {
            var result = _allBooks.AsEnumerable();

            // lọc tên sách
            if (!string.IsNullOrWhiteSpace(SearchTenSach))
                result = result.Where(b => b.TenSach.ToLower().Contains(SearchTenSach.ToLower()));

            // lọc tên tác giả
            if (!string.IsNullOrWhiteSpace(SearchTacGia))
            {
                result = result.Where(b => b.DanhSachTacGia.Any(t => t.TenTacGia.ToLower().Contains(SearchTacGia.ToLower())));
            }

            // lọc theo thể loại
            if (SelectedTheLoai != "Tất cả thể loại" && !string.IsNullOrEmpty(SelectedTheLoai))
                result = result.Where(b => b.TheLoai == SelectedTheLoai);

            // lọc theo khoảng giá
            switch (SelectedPriceRangeIndex)
            {
                case 1: // Dưới 50.000đ
                    result = result.Where(x => x.DonGiaBan < 50000);
                    break;
                case 2: // 50.000đ - 100.000đ
                    result = result.Where(x => x.DonGiaBan >= 50000 && x.DonGiaBan <= 100000);
                    break;
                case 3: // 100.000đ - 200.000đ
                    result = result.Where(x => x.DonGiaBan > 100000 && x.DonGiaBan <= 200000);
                    break;
                case 4: // Trên 200.000đ
                    result = result.Where(x => x.DonGiaBan > 200000);
                    break;
                case 0: // Tất cả mức giá -> Không làm gì cả
                default:
                    break;
            }

            // lọc theo tồn kho
            if (!string.IsNullOrWhiteSpace(SearchTonKho) && int.TryParse(SearchTonKho, out int tonKho))
            {
                result = result.Where(x => x.SoLuongTonKho <= tonKho);
            }

            _filteredBooks.Clear();
            int stt = 1;
            foreach (var b in result)
            {
                b.STT = stt++;
                _filteredBooks.Add(b);
            }

            // ĐỒNG NHẤT BIẾN THEO CHUẨN MỚI
            TongBanGhi = _filteredBooks.Count;

            // Cực kỳ quan trọng: Khi có kết quả tìm kiếm mới, LUÔN LUÔN phải reset về trang 1
            TrangHienTai = 1;
            UpdatePagination();
        }

        private void UpdatePagination()
        {
            // Tính tổng số trang (Đã đổi TotalPages -> TongSoTrang)
            TongSoTrang = (int)Math.Ceiling((double)_filteredBooks.Count / _pageSize);
            if (TongSoTrang < 1) TongSoTrang = 1;

            // Cắt dữ liệu đưa ra Grid (Đã đổi CurrentPage -> TrangHienTai)
            PagedBooks.Clear();
            var pagedData = _filteredBooks.Skip((TrangHienTai - 1) * _pageSize).Take(_pageSize);
            foreach (var b in pagedData)
            {
                PagedBooks.Add(b);
            }

            // GHI CHÚ: Mình đã xóa toàn bộ đoạn code "PageNumbers.Clear();..." cũ 
            // vì UI mới không còn dùng danh sách nút số nữa, giúp code nhẹ đi rất nhiều!
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

            // Nếu thực sự có sự thay đổi trang thì mới gọi hàm Load
            if (targetPage != TrangHienTai)
            {
                GoToPage(targetPage);
            }
        }
        private void GoToPage(int page)
        {
            // Kiểm tra an toàn để không bao giờ bị lỗi index
            if (page >= 1 && page <= TongSoTrang)
            {
                TrangHienTai = page;
                UpdatePagination(); // Cập nhật lại danh sách sách hiển thị trên Grid
            }
        }
        // ===========================================================================================


        // ===========================================================================================
        /// <summary>
        /// LOAD DATA ASYNC TỪ API
        /// </summary>
        /// <returns></returns>
        /// 
        private async Task LoadTiLeGiaBanAsync()
        {
            try
            {
                // ThamSo API trả về object { TenThamSo, GiaTri } — đọc GiaTri
                var thamSo = await ApiClient.GetAsync<ThamSoDTO>("api/ThamSo/TiLeDonGiaBan");
                if (thamSo != null && thamSo.GiaTri > 0)
                {
                    _tiLeGiaBan = thamSo.GiaTri;
                }
            }
            catch
            {
                // Nếu API lỗi thì giữ mặc định 1.0, không crash app
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var danhSachTuApi = await ApiClient.GetAsync<List<SachDTO>>("api/PhienBanSach");
                var listTacPhamGoc = await ApiClient.GetAsync<List<DauSachResponseDTO>>("api/Sach");
                var tonKhoToiThieu = await ApiClient.GetAsync<ThamSoDTO>("api/ThamSo/SoLuongTonToiThieu");
                if (danhSachTuApi != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _allBooks.Clear();
                        int stt = 1;
                        foreach (var item in danhSachTuApi)
                        {
                            var newBook = new Models.BookItem
                            {
                                Id = item.Id,
                                STT = stt++,
                                TenSach = item.TenSach,
                                TheLoai = item.TheLoai,
                                MoTa = item.MoTa,
                                SoLuongTonKho = item.SoLuongTonKho,
                                TongDaBan = item.TongDaBan,
                                GiaNiemYet = item.GiaNiemYet,
                                DonGiaBan = item.DonGiaBan,
                                HinhAnh = string.IsNullOrEmpty(item.HinhAnh) ? "/Resources/Images/Books/default_book_cover.jpg" : item.HinhAnh,
                                ISBN = item.ISBN,
                                NamXuatBan = item.NamXuatBan,
                                LanTaiBan = item.LanTaiBan,
                                NhaXuatBan = item.NhaXuatBan,
                                HinhThucBia = item.HinhThucBia,
                                IsCanhBaoTonKho = item.SoLuongTonKho <= tonKhoToiThieu.GiaTri
                            };
                            if (item.DanhSachTacGia != null)
                            {
                                foreach (var tg in item.DanhSachTacGia)
                                    newBook.DanhSachTacGia.Add(tg);
                            }

                            _allBooks.Add(newBook);
                        }
                        PerformSearch();

                        ListTacPhamGoc.Clear();
                        foreach (var b in listTacPhamGoc)
                        {
                            var newBook = new Models.BookItem
                            {
                                TenSach = b.TenSach,
                                TheLoai = b.TenTheLoai,
                                HinhAnh = b.ImageUrl,
                                MoTa = b.MoTa
                            };

                            if(b.DanhSachTacGia != null)
                            {
                                foreach(var tg in b.DanhSachTacGia)
                                {
                                    newBook.DanhSachTacGia.Add(tg);                                    
                                }    
                            }

                            ListTacPhamGoc.Add(newBook);
                        }    
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadTheLoaiAsync()
        {
            try
            {
                var danhSachTheLoai = await ApiClient.GetAsync<List<string>>("api/TheLoai/names");

                if (danhSachTheLoai != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ListTheLoai.Clear();
                        ListTheLoaiTaoSach.Clear();
                        ListTheLoai.Add("Tất cả thể loại");

                        foreach (var tl in danhSachTheLoai)
                        {
                            ListTheLoai.Add(tl);
                            ListTheLoaiTaoSach.Add(tl);
                        }
                        SelectedTheLoai = "Tất cả thể loại";
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi tải thể loại: {ex.Message}");
            }
        }

        private async Task LoadTacGiaAsync()
        {
            try
            {
                var data = await ApiClient.GetAsync<List<TacGiaDTO>>("api/TacGia");
                if (data != null)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        ListTatCaTacGia.Clear();
                        foreach (var item in data) ListTatCaTacGia.Add(item);
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi tải danh sách tác giả: {ex.Message}");
            }
        }

        private async Task LoadNhaXuatBanAsync()
        {
            var data = await ApiClient.GetAsync<List<string>>("api/NhaXuatBan/names");
            if (data != null)
            {
                Application.Current.Dispatcher.Invoke(() => {
                    ListNhaXuatBan.Clear();
                    foreach (var item in data) ListNhaXuatBan.Add(item);
                });
            }
        }
        // ===========================================================================================

        // ================================ VALIDATION ===========================================
        private bool ValidateInput(bool isAdding)
        {
            // Nếu đang Thêm Mới mà chọn chế độ "Đầu sách cũ"
            if (isAdding && IsOldProduct)
            {
                if (SelectedTacPhamGoc == null)
                {
                    MessageBox.Show("Vui lòng tìm và chọn một đầu sách gốc!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            // Nếu đang Thêm "Đầu sách mới" || đang Sửa mà "Mở khóa Đầu sách"
            if ((isAdding && IsNewProduct) || (!isAdding && IsEditMasterEnabled))
            {
                if (string.IsNullOrWhiteSpace(EditingBook.TenSach))
                {
                    MessageBox.Show("Vui lòng nhập Tên đầu sách!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                if (EditingBook.DanhSachTacGia == null || EditingBook.DanhSachTacGia.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất 1 Tác giả!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(EditingBook.TheLoai))
                {
                    MessageBox.Show("Vui lòng chọn Thể loại!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            // 3kiểm tra thông tin Phiên bản
            if (string.IsNullOrWhiteSpace(EditingBook.ISBN))
            {
                MessageBox.Show("Vui lòng nhập Mã ISBN!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(EditingBook.NhaXuatBan))
            {
                MessageBox.Show("Vui lòng chọn Nhà xuất bản!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            //  Kiểm tra logic giá và tồn kho
            if (EditingBook.GiaNiemYet < 0 || EditingBook.DonGiaBan < 0)
            {
                MessageBox.Show("Giá tiền không được để số âm!", "Lỗi logic", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if(EditingBook.SoLuongTonKho < 0)
            {
                MessageBox.Show("Số lượng tồn kho không được để số âm!", "Lỗi logic", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (EditingBook.LanTaiBan < 0)
            {
                MessageBox.Show("Số lượng tồn kho không được để số âm!", "Lỗi logic", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true; // Nếu qua hết các ải trên thì cho phép Lưu
        }
    }
}
