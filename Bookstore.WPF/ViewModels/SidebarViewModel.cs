using Bookstore.WPF.Services;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class SidebarViewModel : BaseViewModel // Bắt buộc kế thừa BaseViewModel
    {
        private Action<object> _handleChangeView;

        #region Properties (Biến hiển thị UI)
        public string HomeTabName { get; set; }

        public Visibility IsDashboardVisible { get; set; }
        public Visibility IsBanHangVisible { get; set; }
        public Visibility IsTraCuuSachVisible { get; set; }
        public Visibility IsKhachHangVisible { get; set; }
        public Visibility IsNhapKhoVisible { get; set; }
        public Visibility IsNhaCungCapVisible { get; set; }
        public Visibility IsUuDaiVisible { get; set; }
        public Visibility IsBaoCaoVisible { get; set; }
        public Visibility IsTaiKhoanVisible { get; set; }
        public Visibility IsCaiDatVisible { get; set; }

        #endregion

        #region Commands (Lệnh điều hướng)
        //
        public ICommand ShowDashboardCommand { get; set; }
        public ICommand ShowBanHangCommand { get; set; }
        public ICommand ShowTraCuuSachCommand { get; set; }
        public ICommand ShowKhachHangCommand { get; set; }
        public ICommand ShowNhapKhoCommand { get; set; }
        public ICommand ShowNhaCungCapCommand { get; set; }
        public ICommand ShowUuDaiCommand { get; set; }
        public ICommand ShowBaoCaoCommand { get; set; }
        public ICommand ShowTaiKhoanCommand { get; set; }
        public ICommand ShowCaiDatCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        #endregion

        public SidebarViewModel() { }
        public SidebarViewModel(List<string> listQuyen, Action<object> changeViewAction)
        {
            _handleChangeView = changeViewAction;

            // Đọc phân quyền từ api
            // ==========================================
            IsDashboardVisible = listQuyen.Contains("CN_DASHBOARD") ? Visibility.Visible : Visibility.Collapsed;
            if (listQuyen.Contains("CN_DASHBOARD"))
                HomeTabName = "Dashboard";
            else
                HomeTabName = "Bán hàng";
            IsBanHangVisible = listQuyen.Contains("CN_BANHANG") ? Visibility.Visible : Visibility.Collapsed;
            IsTraCuuSachVisible = listQuyen.Contains("CN_TRACUU") ? Visibility.Visible : Visibility.Collapsed;
            IsKhachHangVisible = listQuyen.Contains("CN_KHACHHANG") ? Visibility.Visible : Visibility.Collapsed;
            IsNhapKhoVisible = listQuyen.Contains("CN_NHAPKHO") ? Visibility.Visible : Visibility.Collapsed;
            IsNhaCungCapVisible = listQuyen.Contains("CN_NHACUNGCAP") ? Visibility.Visible : Visibility.Collapsed;
            IsUuDaiVisible = listQuyen.Contains("CN_UUDAI") ? Visibility.Visible : Visibility.Collapsed;
            IsBaoCaoVisible = listQuyen.Contains("CN_BAOCAO") ? Visibility.Visible : Visibility.Collapsed;
            IsTaiKhoanVisible = listQuyen.Contains("CN_TAIKHOAN") ? Visibility.Visible : Visibility.Collapsed;
            IsCaiDatVisible = listQuyen.Contains("CN_CAIDAT") ? Visibility.Visible : Visibility.Collapsed;

            // Command
            // ==========================================
            //ShowDashboardCommand = new RelayCommand<object>((p) => _handleChangeView(new DashboardViewModel()));
            //ShowBanHangCommand = new RelayCommand<object>((p) => _handleChangeView(new BanHangViewModel()));
            //ShowTraCuuSachCommand = new RelayCommand<object>((p) => _handleChangeView(new TraCuuSachViewModel()));
            //ShowKhachHangCommand = new RelayCommand<object>((p) => _handleChangeView(new KhachHangViewModel()));
            //ShowNhapKhoCommand = new RelayCommand<object>((p) => _handleChangeView(new NhapKhoViewModel()));
            //ShowNhaCungCapCommand = new RelayCommand<object>((p) => _handleChangeView(new NhaCungCapViewModel()));
            //ShowUuDaiCommand = new RelayCommand<object>((p) => _handleChangeView(new UuDaiViewModel()));
            //ShowBaoCaoCommand = new RelayCommand<object>((p) => _handleChangeView(new BaoCaoViewModel()));
            //ShowTaiKhoanCommand = new RelayCommand<object>((p) => _handleChangeView(new TaiKhoanViewModel()));
            //ShowCaiDatCommand = new RelayCommand<object>((p) => _handleChangeView(new CaiDatViewModel()));

            // Đăng xuất
            LogoutCommand = new RelayCommand<object>((p) =>
            {
                // Gọi API Logout (nếu có)

                // Mở lại màn hình Login
                var loginWindow = new Bookstore.WPF.Views.LoginView();
                loginWindow.Show();

                // Đóng Window chính (MainWindow)
                Application.Current.MainWindow.Close();
            });
        }
    }
}