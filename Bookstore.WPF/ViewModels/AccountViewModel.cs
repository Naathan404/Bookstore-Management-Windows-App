using Bookstore.Share.DTOs;
using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    // KẾ THỪA TỪ BASE LIST ĐỂ QUẢN LÝ TAB TÀI KHOẢN
    public class AccountViewModel : BaseListViewModel
    {
        // ==========================================
        // TAB 1: TÀI KHOẢN (Được quản lý bởi BaseListViewModel)
        // ==========================================

        private List<AccountDto> _allAccounts = new();
        public ObservableCollection<AccountDto> PagedAccounts { get; set; } = new();

        // Các biến phân trang (CurrentPage, TotalPages, PageSize...) 
        // và biến SearchKeyword đã được tự động xử lý bên trong BaseListViewModel!

        // ComboBox lọc phụ trên toolbar
        public ObservableCollection<NhomNguoiDungDto> RoleFilterList { get; set; } = new();

        private NhomNguoiDungDto _selectedRoleFilter;
        public NhomNguoiDungDto SelectedRoleFilter
        {
            get => _selectedRoleFilter;
            set { _selectedRoleFilter = value; OnPropertyChanged(); TrangHienTai = 1; ApplyFilterAndPagination(); }
        }

        public ObservableCollection<string> StatusFilterList { get; set; } = new ObservableCollection<string> { "Tất cả trạng thái", "Đang làm việc", "Đã nghỉ việc" };

        private string _selectedStatusFilter = "Tất cả trạng thái";
        public string SelectedStatusFilter
        {
            get => _selectedStatusFilter;
            set { _selectedStatusFilter = value; OnPropertyChanged(); TrangHienTai = 1; ApplyFilterAndPagination(); }
        }

        // ==========================================
        // TAB 2: NHÓM NGƯỜI DÙNG & PHÂN QUYỀN
        // ==========================================

        public ObservableCollection<NhomNguoiDungDto> RolesList { get; set; } = new();

        private NhomNguoiDungDto _selectedRole;
        public NhomNguoiDungDto SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
                ScreenPermissions.Clear();
                if (value != null)
                    _ = LoadPermissionsForRoleAsync(value.MaNhomNguoiDung);
            }
        }

        public ObservableCollection<ScreenPermissionDto> ScreenPermissions { get; set; } = new();

        private string _newRoleName = "";
        public string NewRoleName { get => _newRoleName; set { _newRoleName = value; OnPropertyChanged(); } }

        // ==========================================
        // TRẠNG THÁI POPUP 
        // ==========================================

        private bool _isAccountPopupVisible;
        public bool IsAccountPopupVisible { get => _isAccountPopupVisible; set { _isAccountPopupVisible = value; OnPropertyChanged(); } }

        private bool _isRolePopupVisible;
        public bool IsRolePopupVisible { get => _isRolePopupVisible; set { _isRolePopupVisible = value; OnPropertyChanged(); } }

        private string _accountPopupTitle;
        public string AccountPopupTitle { get => _accountPopupTitle; set { _accountPopupTitle = value; OnPropertyChanged(); } }

        private bool _isEditMode;
        public bool IsEditMode { get => _isEditMode; set { _isEditMode = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsAddMode)); } }
        public bool IsAddMode => !_isEditMode;

        private AccountDto _editingAccount = new();
        public AccountDto EditingAccount { get => _editingAccount; set { _editingAccount = value; OnPropertyChanged(); } }


        // ==========================================
        // COMMANDS
        // ==========================================
        public ICommand ClearFilterCommand { get; private set; }

        public ICommand OpenAddAccountPopupCommand { get; private set; }
        public ICommand OpenEditAccountPopupCommand { get; private set; }
        public ICommand ClosePopupCommand { get; private set; }
        public ICommand SaveAccountCommand { get; private set; }
        public ICommand ResetPasswordCommand { get; private set; }
        public ICommand DeleteAccountCommand { get; private set; }

        public ICommand OpenAddRolePopupCommand { get; private set; }
        public ICommand SaveNewRoleCommand { get; private set; }
        public ICommand SavePermissionsCommand { get; private set; }
        public ICommand DeleteRoleCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }


        // CONSTRUCTOR
        public AccountViewModel()
        {
            InitCommands();
            _ = LoadInitialDataAsync();
        }

        private void InitCommands()
        {
            // --- CÁC COMMAND CỦA TAB 1 (Tài khoản) ---
            ClearFilterCommand = new RelayCommand<object>(_ =>
            {
                SearchKeyword = "";
                SelectedRoleFilter = RoleFilterList.FirstOrDefault();
                SelectedStatusFilter = "Tất cả trạng thái";
                TrangHienTai = 1;
                ApplyFilterAndPagination();
            });

            OpenAddAccountPopupCommand = new RelayCommand<object>(_ =>
            {
                AccountPopupTitle = "THÊM TÀI KHOẢN MỚI";
                IsEditMode = false;
                EditingAccount = new AccountDto
                {
                    GioiTinh = "Nam",
                    DangLamViec = true,
                    NgaySinh = new DateOnly(2000, 1, 1),
                    NgayVaoLam = DateOnly.FromDateTime(DateTime.Today)
                };

                EditingAccount.SelectedRole = RolesList.FirstOrDefault();
                IsAccountPopupVisible = true;
                IsRolePopupVisible = false;
            });

            OpenEditAccountPopupCommand = new RelayCommand<object>(p =>
            {
                if (p is not AccountDto acc) return;
                AccountPopupTitle = "CHỈNH SỬA TÀI KHOẢN";
                IsEditMode = true;

                EditingAccount = new AccountDto
                {
                    Username = acc.Username,
                    HoTen = acc.HoTen,
                    Email = acc.Email,
                    GioiTinh = acc.GioiTinh,
                    ChucVu = acc.ChucVu,
                    DangLamViec = acc.DangLamViec,
                    NgaySinh = acc.NgaySinh,
                    NgayVaoLam = acc.NgayVaoLam,
                    RoleName = acc.RoleName,
                    SelectedRole = RolesList.FirstOrDefault(r => r.TenNhomNguoiDung == acc.RoleName)
                };
                IsAccountPopupVisible = true;
                IsRolePopupVisible = false;
            });

            ClosePopupCommand = new RelayCommand<object>(_ =>
            {
                IsAccountPopupVisible = false;
                IsRolePopupVisible = false;
            });

            SaveAccountCommand = new RelayCommand<object>(async _ => await SaveAccountAsync());
            ResetPasswordCommand = new RelayCommand<object>(async p => await ResetPasswordAsync(p as AccountDto));
            DeleteAccountCommand = new RelayCommand<object>(async p => await DeleteAccountAsync(p as AccountDto));

            RefreshCommand = new RelayCommand<object>(async p => await LoadAccountsAsync());

            // --- CÁC COMMAND CỦA TAB 2 (Quyền) ---
            OpenAddRolePopupCommand = new RelayCommand<object>(_ =>
            {
                NewRoleName = "";
                IsRolePopupVisible = true;
                IsAccountPopupVisible = false;
            });

            SaveNewRoleCommand = new RelayCommand<object>(async _ => await SaveNewRoleAsync());
            SavePermissionsCommand = new RelayCommand<object>(async _ => await SavePermissionsAsync());
            DeleteRoleCommand = new RelayCommand<object>(async p => await DeleteRoleAsync(p as NhomNguoiDungDto));
        }

        // ==========================================
        // DATA LOADING & LỌC TÀI KHOẢN (GHI ĐÈ BASE)
        // ==========================================

        private async Task LoadInitialDataAsync()
        {
            await LoadRolesAsync();
            await LoadAccountsAsync();
        }

        private async Task LoadRolesAsync()
        {
            var data = await ApiClient.GetAsync<List<NhomNguoiDungDto>>("api/NhomNguoiDung");
            if (data == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                RolesList.Clear();
                RoleFilterList.Clear();

                RoleFilterList.Add(new NhomNguoiDungDto { MaNhomNguoiDung = 0, TenNhomNguoiDung = "Tất cả" });

                foreach (var r in data)
                {
                    RolesList.Add(r);
                    RoleFilterList.Add(r);
                }

                SelectedRoleFilter = RoleFilterList.First();
            });
        }

        private async Task LoadAccountsAsync()
        {
            var data = await ApiClient.GetAsync<List<AccountDto>>("api/NguoiDung");
            if (data == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                _allAccounts = data;
                TrangHienTai = 1;
                ApplyFilterAndPagination();
            });
        }

        // --- GHI ĐÈ HÀM PHÂN TRANG VÀ LỌC CỦA BASELISTVIEWMODEL ---
        protected override void ApplyFilterAndPagination()
        {
            var query = _allAccounts.AsQueryable();

            // Lọc theo SearchKeyword (Biến này tự động lấy từ Base)
            if (!string.IsNullOrWhiteSpace(SearchKeyword))
            {
                query = query.Where(x =>
                    (x.Username ?? "").Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase) ||
                    (x.HoTen ?? "").Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase) ||
                    (x.Email ?? "").Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase));
            }

            // Lọc theo nhóm
            if (SelectedRoleFilter != null && SelectedRoleFilter.MaNhomNguoiDung != 0)
                query = query.Where(x => x.RoleName == SelectedRoleFilter.TenNhomNguoiDung);

            // Lọc theo trạng thái
            if (SelectedStatusFilter != "Tất cả trạng thái")
            {
                if (SelectedStatusFilter == "Đang làm việc")
                    query = query.Where(x => x.DangLamViec == true);
                else if (SelectedStatusFilter == "Đã nghỉ việc")
                    query = query.Where(x => x.DangLamViec == false);
            }

            var filtered = query.ToList();

            // Tính toán tổng số đẩy xuống Base
            TongBanGhi = filtered.Count;
            TongSoTrang = Math.Max(1, (int)Math.Ceiling((double)TongBanGhi / PageSize));

            if (TrangHienTai > TongSoTrang) TrangHienTai = TongSoTrang;
            if (TrangHienTai < 1) TrangHienTai = 1;

            // Cắt dữ liệu
            var paged = filtered
                .Skip((TrangHienTai - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            PagedAccounts.Clear();
            int stt = (TrangHienTai - 1) * PageSize + 1;
            foreach (var acc in paged)
            {
                acc.STT = stt++;
                PagedAccounts.Add(acc);
            }
        }


        // ==========================================
        // CÁC HÀM XỬ LÝ (CRUD)
        // ==========================================

        private async Task SaveAccountAsync()
        {
            if (string.IsNullOrWhiteSpace(EditingAccount.Username))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(EditingAccount.HoTen))
            {
                MessageBox.Show("Vui lòng nhập Họ tên!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EditingAccount.SelectedRole == null)
            {
                MessageBox.Show("Vui lòng chọn Nhóm người dùng!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!string.IsNullOrWhiteSpace(EditingAccount.Email) && !EditingAccount.Email.Contains('@'))
            {
                MessageBox.Show("Email không hợp lệ!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EditingAccount.NgayVaoLam <= EditingAccount.NgaySinh)
            {
                MessageBox.Show("Ngày vào làm phải lớn hơn ngày sinh.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (EditingAccount.Username.Equals("admin", StringComparison.OrdinalIgnoreCase) &&
                !EditingAccount.SelectedRole.TenNhomNguoiDung.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Tài khoản 'admin' bắt buộc phải thuộc nhóm quyền 'ADMIN' để đảm bảo an toàn hệ thống!", "Cảnh báo bảo mật", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            EditingAccount.MaNhomNguoiDung = EditingAccount.SelectedRole.MaNhomNguoiDung;

            try
            {
                bool success;
                if (IsAddMode)
                    success = await ApiClient.PostAndCheckSuccessAsync("api/NguoiDung", EditingAccount);
                else
                    success = await ApiClient.PutAndCheckSuccessAsync($"api/NguoiDung/{EditingAccount.Username}", EditingAccount);

                if (success)
                {
                    MessageBox.Show(IsAddMode ? "Thêm tài khoản thành công!\n" : "Cập nhật tài khoản thành công!",
                        "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                    IsAccountPopupVisible = false;
                    await LoadAccountsAsync();
                }
                else
                {
                    MessageBox.Show(IsAddMode ? "Thêm thất bại! Tên đăng nhập có thể đã tồn tại." : "Cập nhật thất bại! Vui lòng thử lại.",
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ResetPasswordAsync(AccountDto acc)
        {
            if (acc == null) return;

            var confirm = MessageBox.Show($"Đặt lại mật khẩu của '{acc.Username}' về mặc định?",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                bool success = await ApiClient.PostNoBodyAsync($"api/NguoiDung/{acc.Username}/reset-password");
                MessageBox.Show(success ? "Đặt lại mật khẩu thành công!" : "Thất bại! Vui lòng thử lại.",
                    success ? "Thành công" : "Lỗi", MessageBoxButton.OK, success ? MessageBoxImage.Information : MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteAccountAsync(AccountDto acc)
        {
            if (acc == null) return;

            if (acc.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Đây là tài khoản quản trị của hệ thống. Bạn không thể xóa tài khoản này!", "Cảnh báo bảo mật", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            var confirm = MessageBox.Show($"Xóa vĩnh viễn tài khoản '{acc.Username}' ({acc.HoTen})?\nHành động này không thể hoàn tác!",
                "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                bool success = await ApiClient.DeleteAsync($"api/NguoiDung/{acc.Username}");
                if (success)
                {
                    _allAccounts.RemoveAll(x => x.Username == acc.Username);
                    ApplyFilterAndPagination();
                    MessageBox.Show("Đã xóa tài khoản thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể xóa tài khoản do đã có ít nhất một hóa đơn được tạo bởi tài khoản này", "Không thể xóa", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ==========================================
        // CÁC HÀM XỬ LÝ NHÓM VÀ QUYỀN (TAB 2)
        // ==========================================

        private async Task LoadPermissionsForRoleAsync(int roleId)
        {
            var data = await ApiClient.GetAsync<List<ScreenPermissionDto>>($"api/PhanQuyen/chuc-nang/{roleId}");
            if (data == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                ScreenPermissions.Clear();
                foreach (var item in data) ScreenPermissions.Add(item);
            });
        }

        private async Task SaveNewRoleAsync()
        {
            if (string.IsNullOrWhiteSpace(NewRoleName))
            {
                MessageBox.Show("Tên nhóm không được để trống!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (RolesList.Any(r => r.TenNhomNguoiDung.Equals(NewRoleName.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Tên nhóm này đã tồn tại!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var dto = new NhomNguoiDungDto { TenNhomNguoiDung = NewRoleName.Trim() };
                var created = await ApiClient.PostAsync<NhomNguoiDungDto, NhomNguoiDungDto>("api/NhomNguoiDung", dto);

                if (created != null)
                {
                    MessageBox.Show($"Đã thêm nhóm '{created.TenNhomNguoiDung}' thành công!\nBây giờ bạn có thể gán quyền cho nhóm này.",
                        "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    IsRolePopupVisible = false;

                    await LoadRolesAsync();
                    SelectedRole = RolesList.FirstOrDefault(r => r.MaNhomNguoiDung == created.MaNhomNguoiDung);
                }
                else
                {
                    MessageBox.Show("Thêm nhóm thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteRoleAsync(NhomNguoiDungDto role)
        {
            if (role == null) return;

            var confirm = MessageBox.Show($"Xóa nhóm '{role.TenNhomNguoiDung}'?\nCác tài khoản thuộc nhóm này sẽ bị ảnh hưởng!",
                "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                bool success = await ApiClient.DeleteAsync($"api/NhomNguoiDung/{role.MaNhomNguoiDung}");
                if (success)
                {
                    await LoadRolesAsync();
                    SelectedRole = null;
                    ScreenPermissions.Clear();
                    MessageBox.Show("Đã xóa nhóm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể xóa", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task SavePermissionsAsync()
        {
            if (SelectedRole == null)
            {
                MessageBox.Show("Vui lòng chọn một nhóm để lưu quyền!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedRole.TenNhomNguoiDung.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))
            {
                if (ScreenPermissions.Any(p => !p.IsGranted))
                {
                    MessageBox.Show("Nhóm 'ADMIN' là nhóm quản trị. Không được phép tắt bất kỳ quyền nào của nhóm này!", "Cảnh báo bảo mật", MessageBoxButton.OK, MessageBoxImage.Stop);

                    // Tự động bật (tick) lại toàn bộ trên giao diện cho người dùng thấy
                    foreach (var p in ScreenPermissions) p.IsGranted = true;
                    return;
                }
            }

            var grantedIds = ScreenPermissions.Where(p => p.IsGranted).Select(p => p.MaChucNang).ToList();
            var payload = new UpdatePermissionsDto { MaNhomNguoiDung = SelectedRole.MaNhomNguoiDung, GrantedChucNangIds = grantedIds };

            try
            {
                bool success = await ApiClient.PutAndCheckSuccessAsync($"api/PhanQuyen/{SelectedRole.MaNhomNguoiDung}", payload);

                MessageBox.Show(
                    success ? $"Đã lưu phân quyền cho nhóm '{SelectedRole.TenNhomNguoiDung}' thành công!"
                            : "Không thể tắt quyền 'Tài khoản' của nhóm ADMIN. Hệ thống cần ít nhất 1 nhóm có thể quản lý tài khoản!",
                    success ? "Thành công" : "Lỗi", MessageBoxButton.OK,
                    success ? MessageBoxImage.Information : MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }


    // ==========================================
    // CÁC LỚP DTO HỖ TRỢ
    // ==========================================

    public class AccountDto : BaseViewModel
    {
        public int STT { get; set; }
        public string Username { get; set; } = "";
        public string HoTen { get; set; } = "";
        public string Email { get; set; } = "";
        public string RoleName { get; set; } = "";
        public NhomNguoiDungDto? SelectedRole { get; set; }
        public int MaNhomNguoiDung { get; set; }

        public string GioiTinh { get; set; } = "Nam";
        public string ChucVu { get; set; } = "";

        private bool _dangLamViec = true;
        public bool DangLamViec
        {
            get => _dangLamViec;
            set
            {
                if (Username?.ToLower() == "admin" && !value)
                {
                    System.Windows.MessageBox.Show("Không thể tắt trạng thái làm việc của tài khoản này!", "Cảnh báo bảo mật",
                        System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    OnPropertyChanged();
                    return;
                }

                _dangLamViec = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TrangThaiText));
                OnPropertyChanged(nameof(TrangThaiColor));
            }
        }
        public DateOnly NgaySinh { get; set; } = new DateOnly(2000, 1, 1);
        public DateOnly NgayVaoLam { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public string TrangThaiText => DangLamViec ? "Đang làm" : "Đã nghỉ";
        public string TrangThaiColor => DangLamViec ? "#05CD99" : "#EE5D50";

        public DateTime? NgaySinhDateTime
        {
            get => NgaySinh.ToDateTime(TimeOnly.MinValue);
            set { if (value.HasValue) { NgaySinh = DateOnly.FromDateTime(value.Value); OnPropertyChanged(); } }
        }

        public DateTime? NgayVaoLamDateTime
        {
            get => NgayVaoLam.ToDateTime(TimeOnly.MinValue);
            set { if (value.HasValue) { NgayVaoLam = DateOnly.FromDateTime(value.Value); OnPropertyChanged(); } }
        }
    }

    public class NhomNguoiDungDto
    {
        public int MaNhomNguoiDung { get; set; }
        public string TenNhomNguoiDung { get; set; } = "";
    }

    public class ScreenPermissionDto : BaseViewModel
    {
        public int MaChucNang { get; set; }
        public string TenChucNang { get; set; } = "";
        public string TenManHinh { get; set; } = "";

        private bool _isGranted;
        public bool IsGranted
        {
            get => _isGranted;
            set { _isGranted = value; OnPropertyChanged(); }
        }
    }

    public class UpdatePermissionsDto
    {
        public int MaNhomNguoiDung { get; set; }
        public List<int> GrantedChucNangIds { get; set; } = new();
    }
}