using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
    public class SidebarViewModel : BaseViewModel
    {
        private Action<object> _handleChangeView;


        #region Properties
        private string _homeTabName;
        public string HomeTabName
        {
            get => _homeTabName;
            set { _homeTabName = value; OnPropertyChanged(); }
        }

        private Visibility _isDashboardVisible;
        public Visibility IsDashboardVisible
        {
            get => _isDashboardVisible;
            set { _isDashboardVisible = value; OnPropertyChanged(); }
        }

        private Visibility _isSaleVisible;
        public Visibility IsSaleVisible
        {
            get => _isSaleVisible;
            set { _isSaleVisible = value; OnPropertyChanged(); }
        }

        private Visibility _isProductVisible;
        public Visibility IsProductVisible
        {
            get => _isProductVisible;
            set { _isProductVisible = value; OnPropertyChanged(); }
        }

        private Visibility _isCustomerVisible;
        public Visibility IsCustomerVisible
        {
            get => _isCustomerVisible;
            set { _isCustomerVisible = value; OnPropertyChanged(); }
        }

        private Visibility _isImportVisible;
        public Visibility IsImportVisible
        {
            get => _isImportVisible;
            set { _isImportVisible = value; OnPropertyChanged(); }
        }

        private Visibility _isSupplierVisible;
        public Visibility IsSupplierVisible
        {
            get => _isSupplierVisible;
            set { _isSupplierVisible = value; OnPropertyChanged(); }
        }

        private Visibility _isPromotionVisible;
        public Visibility IsPromotionVisible
        {
            get => _isPromotionVisible;
            set { _isPromotionVisible = value; OnPropertyChanged(); }
        }

        private Visibility _isReportVisible;
        public Visibility IsReportVisible
        {
            get => _isReportVisible;
            set { _isReportVisible = value; OnPropertyChanged(); }
        }

        private Visibility _isAccountVisible;
        public Visibility IsAccountVisible
        {
            get => _isAccountVisible;
            set { _isAccountVisible = value; OnPropertyChanged(); }
        }

        private Visibility _isSettingVisible;
        public Visibility IsSettingVisible
        {
            get => _isSettingVisible;
            set { _isSettingVisible = value; OnPropertyChanged(); }
        }
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

        public SidebarViewModel(Action<object> changeViewAction)
        {
            _handleChangeView = changeViewAction;
            var listQuyen = AppState.CurrentPermissions;

            string debugstring = string.Empty;
            foreach (var s in AppState.CurrentPermissions) debugstring += s.ToString();
            MessageBox.Show(debugstring);

            // Đọc phân quyền từ api
            // ==========================================
            IsDashboardVisible = listQuyen.Contains("DashboardView") ? Visibility.Visible : Visibility.Collapsed;
            if (listQuyen.Contains("DashboardView"))
                HomeTabName = "Trang chủ";
            else
                HomeTabName = "Bán hàng";
            IsDashboardVisible = listQuyen.Contains("DashboardView") ? Visibility.Visible : Visibility.Collapsed;
            IsSaleVisible = listQuyen.Contains("SaleView") ? Visibility.Visible : Visibility.Collapsed;
            IsProductVisible = listQuyen.Contains("ProductView") ? Visibility.Visible : Visibility.Collapsed;
            IsCustomerVisible = listQuyen.Contains("CustomerView") ? Visibility.Visible : Visibility.Collapsed;
            IsImportVisible = listQuyen.Contains("ImportView") ? Visibility.Visible : Visibility.Collapsed;
            IsSupplierVisible = listQuyen.Contains("SupplierView") ? Visibility.Visible : Visibility.Collapsed;
            IsPromotionVisible = listQuyen.Contains("PromotionView") ? Visibility.Visible : Visibility.Collapsed;
            IsReportVisible = listQuyen.Contains("ReportView") ? Visibility.Visible : Visibility.Collapsed;
            IsAccountVisible = listQuyen.Contains("AccountView") ? Visibility.Visible : Visibility.Collapsed;
            IsSettingVisible = listQuyen.Contains("SettingView") ? Visibility.Visible : Visibility.Collapsed;

            // Command
            // ==========================================
            if (listQuyen.Contains("DashboardView"))
                ShowDashboardCommand = new RelayCommand<object>((p) => _handleChangeView(new DashboardViewModel()));

            if (listQuyen.Contains("SaleView"))
                ShowBanHangCommand = new RelayCommand<object>((p) => _handleChangeView(new SaleViewModel()));
            if (listQuyen.Contains("ProductView"))
                ShowTraCuuSachCommand = new RelayCommand<object>((p) => _handleChangeView(new ProductViewModel()));
            if (listQuyen.Contains("CustomerView"))
                ShowKhachHangCommand = new RelayCommand<object>((p) => _handleChangeView(new CustomerViewModel()));
            if (listQuyen.Contains("ImportView"))
                ShowNhapKhoCommand = new RelayCommand<object>((p) => _handleChangeView(new ImportViewModel()));
            if (listQuyen.Contains("SupplierView"))
                ShowNhaCungCapCommand = new RelayCommand<object>((p) => _handleChangeView(new SupplierViewModel()));
            if (listQuyen.Contains("PromotionView"))
                ShowUuDaiCommand = new RelayCommand<object>((p) => _handleChangeView(new PromotionViewModel()));
            if (listQuyen.Contains("ReportView"))
                ShowBaoCaoCommand = new RelayCommand<object>((p) => _handleChangeView(new ReportViewModel()));
            if (listQuyen.Contains("AccountView"))
                ShowTaiKhoanCommand = new RelayCommand<object>((p) => _handleChangeView(new AccountViewModel()));
            if (listQuyen.Contains("SettingView"))
                ShowCaiDatCommand = new RelayCommand<object>((p) => _handleChangeView(new SettingViewModel()));

            // Đăng xuất
            LogoutCommand = new RelayCommand<object>((p) =>
            {
                // Gọi API Logout \\

                // Mở lại màn hình Login
                var loginWindow = new Bookstore.WPF.Views.LoginView();
                loginWindow.Show();

                // Đóng Window chính
                Application.Current.MainWindow.Close();
            });
        }
    }
}