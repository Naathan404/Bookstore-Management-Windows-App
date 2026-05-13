using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class CategoryItem : BaseViewModel
    {
        public int Id { get; set; }
        private int _stt; 
        public int Stt { get => _stt; set { _stt = value; OnPropertyChanged(); } }
        private string _name = "";
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string Type { get; set; }
    }

    public class DauSachItem : BaseViewModel
    {
        public int Id { get; set; }
        private int _stt; 
        public int Stt { get => _stt; set { _stt = value; OnPropertyChanged(); } }
        private string _name = "";
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        private string _tenTheLoai = "";
        public string TenTheLoai { get => _tenTheLoai; set { _tenTheLoai = value; OnPropertyChanged(); } }
        private string _moTa = "";
        public string MoTa { get => _moTa; set { _moTa = value; OnPropertyChanged(); } }
        private string _imageUrl = "/Resources/Images/Books/default_book_cover.jpg";
        public string ImageUrl { get => _imageUrl; set { _imageUrl = value; OnPropertyChanged(); } }
        public ObservableCollection<TacGiaDTO> DanhSachTacGia { get; set; } = new ObservableCollection<TacGiaDTO>();
    }

    public class CategoryViewModel : BaseViewModel
    {
        #region Collections
        private List<DauSachItem> _allDauSachs = new List<DauSachItem>();
        private List<CategoryItem> _allTacGias = new List<CategoryItem>();
        private List<CategoryItem> _allTheLoais = new List<CategoryItem>();
        private List<CategoryItem> _allNXBs = new List<CategoryItem>();

        public ObservableCollection<DauSachItem> FilteredDauSachs { get; set; } = new ObservableCollection<DauSachItem>();
        public ObservableCollection<CategoryItem> FilteredTacGias { get; set; } = new ObservableCollection<CategoryItem>();
        public ObservableCollection<CategoryItem> FilteredTheLoais { get; set; } = new ObservableCollection<CategoryItem>();
        public ObservableCollection<CategoryItem> FilteredNXBs { get; set; } = new ObservableCollection<CategoryItem>();
        #endregion

        #region Search Properties
        public string SearchDauSach { get => _searchDauSach; set { _searchDauSach = value; OnPropertyChanged(); ApplyFilter("DauSach"); } }
        private string _searchDauSach = "";
        public string SearchTacGia { get => _searchTacGia; set { _searchTacGia = value; OnPropertyChanged(); ApplyFilter("TacGia"); } }
        private string _searchTacGia = "";
        public string SearchTheLoai { get => _searchTheLoai; set { _searchTheLoai = value; OnPropertyChanged(); ApplyFilter("TheLoai"); } }
        private string _searchTheLoai = "";
        public string SearchNXB { get => _searchNXB; set { _searchNXB = value; OnPropertyChanged(); ApplyFilter("NXB"); } }
        private string _searchNXB = "";
        #endregion

        #region Popup Căn bản (Thể loại, Tác giả, NXB)
        public Visibility IsPopupVisible { get => _isPopupVisible; set { _isPopupVisible = value; OnPropertyChanged(); } }
        private Visibility _isPopupVisible = Visibility.Hidden;
        public string PopupTitle { get => _popupTitle; set { _popupTitle = value; OnPropertyChanged(); } }
        private string _popupTitle = "";
        public string EditingName { get => _editingName; set { _editingName = value; OnPropertyChanged(); } }
        private string _editingName = "";
        private int _editingId = 0;
        private bool _isAddMode = true;
        private string _currentEditType = "";
        #endregion

        #region Popup ĐẦU SÁCH (Đặc thù)
        public Visibility IsDauSachPopupVisible { get => _isDauSachPopupVisible; set { _isDauSachPopupVisible = value; OnPropertyChanged(); } }
        private Visibility _isDauSachPopupVisible = Visibility.Hidden;
        public string DauSachPopupTitle { get => _dauSachPopupTitle; set { _dauSachPopupTitle = value; OnPropertyChanged(); } }
        private string _dauSachPopupTitle = "";

        public DauSachItem EditingDauSach { get => _editingDauSach; set { _editingDauSach = value; OnPropertyChanged(); } }
        private DauSachItem _editingDauSach;
        private bool _isAddDauSachMode = true;

        public ObservableCollection<string> ListTheLoai { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<TacGiaDTO> ListTatCaTacGia { get; set; } = new ObservableCollection<TacGiaDTO>();

        private TacGiaDTO _selectedTacGiaToAdd;
        public TacGiaDTO SelectedTacGiaToAdd
        {
            get => _selectedTacGiaToAdd;
            set
            {
                _selectedTacGiaToAdd = value;
                OnPropertyChanged();
                if (value != null && EditingDauSach != null)
                {
                    if (!EditingDauSach.DanhSachTacGia.Any(t => t.Id == value.Id))
                        EditingDauSach.DanhSachTacGia.Add(value);
                    Application.Current.Dispatcher.InvokeAsync(() => SelectedTacGiaToAdd = null); // Reset ô chọn
                }
            }
        }
        #endregion

        #region Commands
        // Commands Cũ
        public ICommand OpenAddTacGiaCommand { get; set; }
        public ICommand OpenAddTheLoaiCommand { get; set; }
        public ICommand OpenAddNXBCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }

        // Commands ĐẦU SÁCH
        public ICommand OpenAddDauSachCommand { get; set; }
        public ICommand EditDauSachCommand { get; set; }
        public ICommand DeleteDauSachCommand { get; set; }
        public ICommand SaveDauSachCommand { get; set; }
        public ICommand CloseDauSachPopupCommand { get; set; }
        public ICommand RemoveTacGiaCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        #endregion

        public CategoryViewModel()
        {
            InitCommands();
            _ = LoadAllDataAsync();
        }

        private void InitCommands()
        {
            #region Init Lệnh Căn Bản (Tác giả, Thể loại, NXB)
            OpenAddTacGiaCommand = new RelayCommand<object>((p) => OpenPopup(true, "TacGia", 0, "", "THÊM TÁC GIẢ"));
            OpenAddTheLoaiCommand = new RelayCommand<object>((p) => OpenPopup(true, "TheLoai", 0, "", "THÊM THỂ LOẠI"));
            OpenAddNXBCommand = new RelayCommand<object>((p) => OpenPopup(true, "NhaXuatBan", 0, "", "THÊM NHÀ XUẤT BẢN"));

            EditCommand = new RelayCommand<CategoryItem>((item) => {
                if (item == null) return;
                OpenPopup(false, item.Type, item.Id, item.Name, "CHỈNH SỬA DANH MỤC");
            });

            ClosePopupCommand = new RelayCommand<object>((p) => IsPopupVisible = Visibility.Hidden);

            SaveCommand = new RelayCommand<object>(async (p) => {
                if (string.IsNullOrWhiteSpace(EditingName))
                {
                    MessageBox.Show("Vui lòng điền thông tin");
                    return;
                }    
                string path = $"api/{_currentEditType}";
                object payload = _currentEditType switch
                {
                    "TacGia" => _isAddMode ? new TacGiaDTO { TenTacGia = EditingName } : new TacGiaDTO { Id = _editingId, TenTacGia = EditingName },
                    "TheLoai" => _isAddMode ? new { TenTheLoai = EditingName } : (object)new { MaTheLoai = _editingId, TenTheLoai = EditingName },
                    _ => _isAddMode ? new { TenNhaXuatBan = EditingName } : (object)new { MaNhaXuatBan = _editingId, TenNhaXuatBan = EditingName }
                };

                bool success = _isAddMode ? await ApiClient.PostAndCheckSuccessAsync(path, payload) : await ApiClient.PutAndCheckSuccessAsync($"{path}/{_editingId}", payload);
                if (success) MessageBox.Show("Cập nhật thành công!");
                if (success) { IsPopupVisible = Visibility.Hidden; await LoadAllDataAsync(); }
            });

            DeleteCommand = new RelayCommand<CategoryItem>(async (item) => {
                if (item == null) return;
                if (MessageBox.Show($"Xóa '{item.Name}'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bool isSucces = (await ApiClient.DeleteAndCheckSuccessAsync($"api/{item.Type}/{item.Id}"));
                    if (isSucces)
                    {
                        MessageBox.Show("Xóa thành công!");
                        await LoadAllDataAsync();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại. Vui lòng thử lại sau.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }    
                }
            });
            #endregion

            #region Init Lệnh ĐẦU SÁCH
            OpenAddDauSachCommand = new RelayCommand<object>((p) => {
                EditingDauSach = new DauSachItem();
                _isAddDauSachMode = true;
                DauSachPopupTitle = "THÊM ĐẦU SÁCH MỚI";
                IsDauSachPopupVisible = Visibility.Visible;
            });

            EditDauSachCommand = new RelayCommand<DauSachItem>((item) => {
                if (item == null) return;

                EditingDauSach = new DauSachItem { Id = item.Id, Name = item.Name, TenTheLoai = item.TenTheLoai, MoTa = item.MoTa, ImageUrl = item.ImageUrl };
                foreach (var tg in item.DanhSachTacGia) EditingDauSach.DanhSachTacGia.Add(new TacGiaDTO { Id = tg.Id, TenTacGia = tg.TenTacGia });

                _isAddDauSachMode = false;
                DauSachPopupTitle = "CHỈNH SỬA ĐẦU SÁCH";
                IsDauSachPopupVisible = Visibility.Visible;
            });

            SelectImageCommand = new RelayCommand<object>((p) => {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (ofd.ShowDialog() == true)
                {
                    EditingDauSach.ImageUrl = ofd.FileName;
                }
            });

            CloseDauSachPopupCommand = new RelayCommand<object>((p) => IsDauSachPopupVisible = Visibility.Hidden);

            RemoveTacGiaCommand = new RelayCommand<TacGiaDTO>((tg) => {
                if (tg != null && EditingDauSach.DanhSachTacGia.Contains(tg))
                    EditingDauSach.DanhSachTacGia.Remove(tg);
            });

            SaveDauSachCommand = new RelayCommand<object>(async (p) => {
                if (string.IsNullOrWhiteSpace(EditingDauSach.Name) || string.IsNullOrWhiteSpace(EditingDauSach.TenTheLoai))
                {
                    MessageBox.Show("Vui lòng nhập Tên đầu sách và Thể loại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning); return;
                }


                object payload = new
                {
                    Id = EditingDauSach.Id,
                    TenSach = EditingDauSach.Name,
                    TenTheLoai = EditingDauSach.TenTheLoai,
                    MoTa = EditingDauSach.MoTa,
                    ImageUrl = EditingDauSach.ImageUrl,
                    DanhSachTacGia = EditingDauSach.DanhSachTacGia.Select(t => t.Id).ToList()
                };

                bool success = _isAddDauSachMode
                    ? await ApiClient.PostAndCheckSuccessAsync("api/Sach", payload)
                    : await ApiClient.PutAndCheckSuccessAsync($"api/Sach/{EditingDauSach.Id}", payload);

                if (success)
                {
                    MessageBox.Show("Lưu Đầu sách thành công!");
                    IsDauSachPopupVisible = Visibility.Hidden;
                    await LoadAllDataAsync();
                }
            });

            DeleteDauSachCommand = new RelayCommand<DauSachItem>(async (item) => {
                if (item == null) return;
                if (MessageBox.Show($"Xóa Đầu sách: {item.Name}?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var isSuccess = await ApiClient.DeleteAndCheckSuccessAsync($"api/Sach/{item.Id}");
                    if (isSuccess)
                    {
                        MessageBox.Show("Xóa Đầu sách thành công!");
                        await LoadAllDataAsync();
                    }
                    else
                    {
                        MessageBox.Show("Xóa Đầu sách thất bại. Vui lòng thử lại sau.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }    
                }
            });
            #endregion
        }

        private void OpenPopup(bool isAdd, string type, int id, string name, string title)
        {
            _isAddMode = isAdd; _currentEditType = type; _editingId = id; EditingName = name; PopupTitle = title; IsPopupVisible = Visibility.Visible;
        }

        public async Task LoadAllDataAsync()
        {
            try
            {
                var resTG = await ApiClient.GetAsync<List<TacGiaDTO>>("api/TacGia");
                var resTL = await ApiClient.GetAsync<List<CategoryItem>>("api/TheLoai");
                var resNXB = await ApiClient.GetAsync<List<CategoryItem>>("api/NhaXuatBan");

                // ĐÃ SỬA: Dùng class đàng hoàng thay vì List<dynamic>
                var resDS = await ApiClient.GetAsync<List<DauSachResponseDTO>>("api/Sach");

                Application.Current.Dispatcher.Invoke(() => {
                    _allTacGias.Clear(); _allTheLoais.Clear(); _allNXBs.Clear(); _allDauSachs.Clear();
                    ListTheLoai.Clear(); ListTatCaTacGia.Clear();

                    if (resTG != null)
                    {
                        foreach (var x in resTG)
                        {
                            _allTacGias.Add(new CategoryItem { Id = x.Id, Name = x.TenTacGia, Type = "TacGia" });
                            ListTatCaTacGia.Add(x); // Đổ vào ComboBox
                        }
                    }

                    if (resTL != null)
                    {
                        foreach (var x in resTL)
                        {
                            x.Type = "TheLoai"; _allTheLoais.Add(x);
                            ListTheLoai.Add(x.Name); // Đổ vào ComboBox
                        }
                    }

                    if (resNXB != null) resNXB.ForEach(x => { x.Type = "NhaXuatBan"; _allNXBs.Add(x); });

                    // Map Đầu sách (Đã sửa lại gọi đúng Tên Thuộc Tính viết hoa)
                    if (resDS != null)
                    {
                        foreach (var x in resDS)
                        {
                            var dsItem = new DauSachItem
                            {
                                Id = x.Id,
                                Name = x.TenSach,
                                TenTheLoai = x.TenTheLoai, // Gọi chính xác tên biến
                                MoTa = x.MoTa ?? "",
                                ImageUrl = x.ImageUrl
                            };

                            if (x.DanhSachTacGia != null)
                            {
                                foreach (var tg in x.DanhSachTacGia)
                                    dsItem.DanhSachTacGia.Add(tg);
                            }
                            _allDauSachs.Add(dsItem);
                        }
                    }

                    ApplyFilter("TacGia");
                    ApplyFilter("TheLoai");
                    ApplyFilter("NXB");
                    ApplyFilter("DauSach");
                });
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, nó sẽ hiện thông báo thay vì im im làm trống DataGrid
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi API", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilter(string type)
        {
            if (type == "DauSach")
            {
                FilteredDauSachs.Clear();
                int stt = 1; // Khởi tạo STT = 1
                foreach (var item in _allDauSachs.Where(x => x.Name.ToLower().Contains(SearchDauSach.ToLower())))
                {
                    item.Stt = stt++; // Gán STT và cộng dần lên
                    FilteredDauSachs.Add(item);
                }
            }
            else if (type == "TacGia")
            {
                FilteredTacGias.Clear();
                int stt = 1;
                foreach (var item in _allTacGias.Where(x => x.Name.ToLower().Contains(SearchTacGia.ToLower())))
                {
                    item.Stt = stt++;
                    FilteredTacGias.Add(item);
                }
            }
            else if (type == "TheLoai")
            {
                FilteredTheLoais.Clear();
                int stt = 1;
                foreach (var item in _allTheLoais.Where(x => x.Name.ToLower().Contains(SearchTheLoai.ToLower())))
                {
                    item.Stt = stt++;
                    FilteredTheLoais.Add(item);
                }
            }
            else if (type == "NXB")
            {
                FilteredNXBs.Clear();
                int stt = 1;
                foreach (var item in _allNXBs.Where(x => x.Name.ToLower().Contains(SearchNXB.ToLower())))
                {
                    item.Stt = stt++;
                    FilteredNXBs.Add(item);
                }
            }
        }

        public class DauSachResponseDTO
        {
            public int Id { get; set; }
            public string TenSach { get; set; }
            public string TenTheLoai { get; set; }
            public string MoTa { get; set; }
            public string ImageUrl { get; set; }
            public List<TacGiaDTO> DanhSachTacGia { get; set; } = new List<TacGiaDTO>();
        }
    }
}