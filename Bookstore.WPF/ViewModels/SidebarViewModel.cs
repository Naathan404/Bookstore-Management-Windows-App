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
        public Visibility IsSaleVisible { get; set; }
        public Visibility IsProductVisible { get; set; }
        public Visibility IsCustomerVisible { get; set; }
        public Visibility IsImportVisible { get; set; }
        public Visibility IsSupplierVisible { get; set; }
        public Visibility IsPromotionVisible { get; set; }
        public Visibility IsReportVisible { get; set; }
        public Visibility IsAccountVisible { get; set; }
        public Visibility IsSettingVisible { get; set; }

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
            IsSaleVisible = listQuyen.Contains("CN_BANHANG") ? Visibility.Visible : Visibility.Collapsed;
            IsProductVisible = listQuyen.Contains("CN_TRACUU") ? Visibility.Visible : Visibility.Collapsed;
            IsCustomerVisible = listQuyen.Contains("CN_KHACHHANG") ? Visibility.Visible : Visibility.Collapsed;
            IsImportVisible = listQuyen.Contains("CN_NHAPKHO") ? Visibility.Visible : Visibility.Collapsed;
            IsSupplierVisible = listQuyen.Contains("CN_NHACUNGCAP") ? Visibility.Visible : Visibility.Collapsed;
            IsPromotionVisible = listQuyen.Contains("CN_UUDAI") ? Visibility.Visible : Visibility.Collapsed;
            IsReportVisible = listQuyen.Contains("CN_BAOCAO") ? Visibility.Visible : Visibility.Collapsed;
            IsAccountVisible = listQuyen.Contains("CN_TAIKHOAN") ? Visibility.Visible : Visibility.Collapsed;
            IsSettingVisible = listQuyen.Contains("CN_CAIDAT") ? Visibility.Visible : Visibility.Collapsed;

            // Command
            // ==========================================
            //if (listQuyen.Contains("CN_DASHBOARD"))
            //    ShowDashboardCommand = new RelayCommand<object>((p) => _handleChangeView(new DashboardViewModel()));

            if (listQuyen.Contains("CN_BANHANG"))
                ShowBanHangCommand = new RelayCommand<object>((p) => _handleChangeView(new SaleViewModel()));
            if (listQuyen.Contains("CN_TRACUU"))
                ShowTraCuuSachCommand = new RelayCommand<object>((p) => _handleChangeView(new ProductViewModel()));
            if (listQuyen.Contains("CN_KHACHHANG"))
                ShowKhachHangCommand = new RelayCommand<object>((p) => _handleChangeView(new CustomerViewModel()));
            if (listQuyen.Contains("CN_NHAPKHO"))
                ShowNhapKhoCommand = new RelayCommand<object>((p) => _handleChangeView(new ImportViewModel()));
            if (listQuyen.Contains("CN_NHACUNGCAP"))
                ShowNhaCungCapCommand = new RelayCommand<object>((p) => _handleChangeView(new SupplierViewModel()));
            if (listQuyen.Contains("CN_UUDAI"))
                ShowUuDaiCommand = new RelayCommand<object>((p) => _handleChangeView(new PromotionViewModel()));
            if (listQuyen.Contains("CN_BAOCAO"))
                ShowBaoCaoCommand = new RelayCommand<object>((p) => _handleChangeView(new ReportViewModel()));
            if (listQuyen.Contains("CN_TAIKHOAN"))
                ShowTaiKhoanCommand = new RelayCommand<object>((p) => _handleChangeView(new AccountViewModel()));
            if (listQuyen.Contains("CN_CAIDAT"))
                ShowCaiDatCommand = new RelayCommand<object>((p) => _handleChangeView(new SettingViewModel()));

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