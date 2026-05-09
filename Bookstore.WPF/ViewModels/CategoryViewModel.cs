using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using Bookstore.Share.DTOs; // Nhớ thêm using này để dùng TacGiaDTO
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace Bookstore.WPF.ViewModels
{
    public class CategoryItem : BaseViewModel
    {
        public int Id { get; set; }
        private string _name = "";
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string Type { get; set; }
    }

    public class CategoryViewModel : BaseViewModel
    {
        #region Collections
        private List<CategoryItem> _allTacGias = new List<CategoryItem>();
        private List<CategoryItem> _allTheLoais = new List<CategoryItem>();
        private List<CategoryItem> _allNXBs = new List<CategoryItem>();

        public ObservableCollection<CategoryItem> FilteredTacGias { get; set; } = new ObservableCollection<CategoryItem>();
        public ObservableCollection<CategoryItem> FilteredTheLoais { get; set; } = new ObservableCollection<CategoryItem>();
        public ObservableCollection<CategoryItem> FilteredNXBs { get; set; } = new ObservableCollection<CategoryItem>();
        #endregion

        #region Properties
        public string SearchTacGia { get => _searchTacGia; set { _searchTacGia = value; OnPropertyChanged(); ApplyFilter("TacGia"); } }
        private string _searchTacGia = "";

        public string SearchTheLoai { get => _searchTheLoai; set { _searchTheLoai = value; OnPropertyChanged(); ApplyFilter("TheLoai"); } }
        private string _searchTheLoai = "";

        public string SearchNXB { get => _searchNXB; set { _searchNXB = value; OnPropertyChanged(); ApplyFilter("NXB"); } }
        private string _searchNXB = "";

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

        #region Commands
        public ICommand OpenAddTacGiaCommand { get; set; }
        public ICommand OpenAddTheLoaiCommand { get; set; }
        public ICommand OpenAddNXBCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }
        #endregion

        public CategoryViewModel()
        {
            InitCommands();
            _ = LoadAllDataAsync();
        }

        private void InitCommands()
        {
            OpenAddTacGiaCommand = new RelayCommand<object>((p) => OpenPopup(true, "TacGia", 0, "", "THÊM TÁC GIẢ"));
            OpenAddTheLoaiCommand = new RelayCommand<object>((p) => OpenPopup(true, "TheLoai", 0, "", "THÊM THỂ LOẠI"));
            OpenAddNXBCommand = new RelayCommand<object>((p) => OpenPopup(true, "NhaXuatBan", 0, "", "THÊM NHÀ XUẤT BẢN"));

            EditCommand = new RelayCommand<CategoryItem>((item) => {
                if (item == null) return;
                OpenPopup(false, item.Type, item.Id, item.Name, "CHỈNH SỬA DANH MỤC");
            });

            ClosePopupCommand = new RelayCommand<object>((p) => IsPopupVisible = Visibility.Hidden);

            SaveCommand = new RelayCommand<object>(async (p) => {
                if (string.IsNullOrWhiteSpace(EditingName)) return;

                string path = $"api/{_currentEditType}";
                bool success = false;

                // Gửi payload khớp với từng Controller của ông
                if (_isAddMode)
                {
                    object payload = _currentEditType switch
                    {
                        "TacGia" => new TacGiaDTO { TenTacGia = EditingName },
                        "TheLoai" => new { TenTheLoai = EditingName },
                        _ => new { TenNhaXuatBan = EditingName }
                    };
                    success = await ApiClient.PostAndCheckSuccessAsync(path, payload);
                }
                else
                {
                    object payload = _currentEditType switch
                    {
                        "TacGia" => new TacGiaDTO { Id = _editingId, TenTacGia = EditingName },
                        "TheLoai" => new { MaTheLoai = _editingId, TenTheLoai = EditingName },
                        _ => new { MaNhaXuatBan = _editingId, TenNhaXuatBan = EditingName }
                    };
                    success = await ApiClient.PutAndCheckSuccessAsync($"{path}/{_editingId}", payload);
                }

                if (success)
                {
                    IsPopupVisible = Visibility.Hidden;
                    await LoadAllDataAsync();
                }
            });

            DeleteCommand = new RelayCommand<CategoryItem>(async (item) => {
                //if (item == null) return;
                //if (MessageBox.Show($"Xóa '{item.Name}'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                //{
                //    if (await ApiClient.DeleteAndCheckSuccessAsync($"api/{item.Type}/{item.Id}"))
                //        await LoadAllDataAsync();
                //}
            });
        }

        private void OpenPopup(bool isAdd, string type, int id, string name, string title)
        {
            _isAddMode = isAdd; _currentEditType = type; _editingId = id;
            EditingName = name; PopupTitle = title; IsPopupVisible = Visibility.Visible;
        }

        public async Task LoadAllDataAsync()
        {
            var resTG = await ApiClient.GetAsync<List<TacGiaDTO>>("api/TacGia");
            var resTL = await ApiClient.GetAsync<List<CategoryItem>>("api/TheLoai");
            var resNXB = await ApiClient.GetAsync<List<CategoryItem>>("api/NhaXuatBan");

            Application.Current.Dispatcher.Invoke(() => {
                _allTacGias.Clear(); _allTheLoais.Clear(); _allNXBs.Clear();

                if (resTG != null)
                {
                    foreach (var x in resTG)
                        _allTacGias.Add(new CategoryItem { Id = x.Id, Name = x.TenTacGia, Type = "TacGia" });
                }

                if (resTL != null) resTL.ForEach(x => { x.Type = "TheLoai"; _allTheLoais.Add(x); });
                if (resNXB != null) resNXB.ForEach(x => { x.Type = "NhaXuatBan"; _allNXBs.Add(x); });

                ApplyFilter("TacGia"); ApplyFilter("TheLoai"); ApplyFilter("NXB");
            });
        }

        private void ApplyFilter(string type)
        {
            if (type == "TacGia")
            {
                FilteredTacGias.Clear();
                _allTacGias.Where(x => x.Name.ToLower().Contains(SearchTacGia.ToLower())).ToList().ForEach(FilteredTacGias.Add);
            }
            else if (type == "TheLoai")
            {
                FilteredTheLoais.Clear();
                _allTheLoais.Where(x => x.Name.ToLower().Contains(SearchTheLoai.ToLower())).ToList().ForEach(FilteredTheLoais.Add);
            }
            else if (type == "NXB")
            {
                FilteredNXBs.Clear();
                _allNXBs.Where(x => x.Name.ToLower().Contains(SearchNXB.ToLower())).ToList().ForEach(FilteredNXBs.Add);
            }
        }
    }
}