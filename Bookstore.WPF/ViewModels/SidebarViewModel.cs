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

        private bool _isCollapsed;
        public bool IsCollapsed
        {
            get => _isCollapsed;
            set { _isCollapsed = value; OnPropertyChanged(); }
        }

        public ICommand ToggleSidebarCommand { get; }

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
        private Visibility _isInvoiceVisible;
        public Visibility IsInvoiceVisible
        {
            get => _isInvoiceVisible;
            set
            {
                _isInvoiceVisible = value;
                OnPropertyChanged();
            }
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

        private Visibility _isReceiptVisible;
        public Visibility IsReceiptVisible
        {
            get => _isReceiptVisible;
            set { _isReceiptVisible = value; OnPropertyChanged(); }
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

        private Visibility _isCategoryVisible;
        public Visibility IsCategoryVisible
        {
            get => _isCategoryVisible;
            set { _isCategoryVisible = value; OnPropertyChanged(); }
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

        #region Cache View models
        private DashboardViewModel _dashboardViewModel;
        private SaleViewModel _saleViewModel;
        private InvoiceViewModel _invoiceViewModel;
        private ProductViewModel _productViewModel;
        private CustomerViewModel _customerViewModel;
        private ReceiptViewModel _receiptViewModel;
        private ImportViewModel _importViewModel;
        private SupplierViewModel _supplierViewModel;
        private PromotionViewModel _promotionViewModel;
        private CategoryViewModel _categoryViewModel;
        private ReportViewModel _reportViewModel;
        private AccountViewModel _accountViewModel;
        private SettingViewModel _settingViewModel;
        #endregion

        #region Commands (Lệnh điều hướng)
        //
        public ICommand ShowDashboardCommand { get; set; }
        public ICommand ShowBanHangCommand { get; set; }
        public ICommand ShowHoaDonCommand { get; set; }
        public ICommand ShowTraCuuSachCommand { get; set; }
        public ICommand ShowKhachHangCommand { get; set; }
        public ICommand ShowPhieuThuCommand { get; set; }
        public ICommand ShowNhapKhoCommand { get; set; }
        public ICommand ShowNhaCungCapCommand { get; set; }
        public ICommand ShowUuDaiCommand { get; set; }
        public ICommand ShowDanhMucCommand { get; set; }
        public ICommand ShowBaoCaoCommand { get; set; }
        public ICommand ShowTaiKhoanCommand { get; set; }
        public ICommand ShowCaiDatCommand { get; set; }
        public ICommand LogoutCommand { get; set; }


        #endregion

        #region Checked 
        private bool _isDashboardChecked;
        public bool IsDashboardChecked
        {
            get => _isDashboardChecked;
            set { _isDashboardChecked = value; OnPropertyChanged(); }
        }

        private bool _isSaleChecked;
        public bool IsSaleChecked
        {
            get => _isSaleChecked;
            set { _isSaleChecked = value; OnPropertyChanged(); }
        }
        private bool _isInvoiceChecked;
        public bool IsInvoiceChecked
        {
            get => _isAccountChecked;
            set
            {
                _isAccountChecked = value; OnPropertyChanged();
            }
        }

        private bool _isProductChecked;
        public bool IsProductChecked
        {
            get => _isProductChecked;
            set { _isProductChecked = value; OnPropertyChanged(); }
        }

        private bool _isCustomerChecked;
        public bool IsCustomerChecked
        {
            get => _isCustomerChecked;
            set { _isCustomerChecked = value; OnPropertyChanged(); }
        }
        private bool _isReceiptChecked;
        public bool IsReceiptChecked
        {
            get => _isReceiptChecked;
            set { _isReceiptChecked = value; OnPropertyChanged(); } 
        }

        private bool _isImportChecked;
        public bool IsImportChecked
        {
            get => _isImportChecked;
            set { _isImportChecked = value; OnPropertyChanged(); }
        }

        private bool _isSupplierChecked;
        public bool IsSupplierChecked
        {
            get => _isSupplierChecked;
            set { _isSupplierChecked = value; OnPropertyChanged(); }
        }

        private bool _isPromotionChecked;
        public bool IsPromotionChecked
        {
            get => _isPromotionChecked;
            set { _isPromotionChecked = value; OnPropertyChanged(); }
        }

        private bool _isCategoryChecked;
        public bool IsCategoryChecked
        {
            get => _isCategoryChecked;
            set { _isCategoryChecked = value; OnPropertyChanged(); }
        }

        private bool _isReportChecked;
        public bool IsReportChecked
        {
            get => _isReportChecked;
            set { _isReportChecked = value; OnPropertyChanged(); }
        }

        private bool _isAccountChecked;
        public bool IsAccountChecked
        {
            get => _isAccountChecked;
            set { _isAccountChecked = value; OnPropertyChanged(); }
        }

        private bool _isSettingChecked;
        public bool IsSettingChecked
        {
            get => _isSettingChecked;
            set { _isSettingChecked = value; OnPropertyChanged(); }
        }
        #endregion

        public SidebarViewModel(Action<object> changeViewAction)
        {
            _handleChangeView = changeViewAction;
            var listQuyen = AppState.CurrentPermissions;

            ToggleSidebarCommand = new RelayCommand<object>((p) => IsCollapsed = !IsCollapsed);

            HomeTabName = AppState.CurrentUser.Name;
            // Đọc phân quyền từ api
            // ==========================================
            //IsDashboardVisible = listQuyen.Contains("DashboardView") ? Visibility.Visible : Visibility.Collapsed;
            IsDashboardVisible = listQuyen.Contains("DashboardView") ? Visibility.Visible : Visibility.Collapsed;
            IsSaleVisible = listQuyen.Contains("SaleView") ? Visibility.Visible : Visibility.Collapsed;
            IsInvoiceVisible = listQuyen.Contains("InvoiceView") ? Visibility.Visible: Visibility.Collapsed;
            IsProductVisible = listQuyen.Contains("ProductView") ? Visibility.Visible : Visibility.Collapsed;
            IsCustomerVisible = listQuyen.Contains("CustomerView") ? Visibility.Visible : Visibility.Collapsed;
            IsReceiptVisible = listQuyen.Contains("ReceiptView") ? Visibility.Visible : Visibility.Collapsed;
            IsImportVisible = listQuyen.Contains("ImportView") ? Visibility.Visible : Visibility.Collapsed;
            IsSupplierVisible = listQuyen.Contains("SupplierView") ? Visibility.Visible : Visibility.Collapsed;
            IsPromotionVisible = listQuyen.Contains("PromotionView") ? Visibility.Visible : Visibility.Collapsed;
            IsCategoryVisible = listQuyen.Contains("CategoryView") ? Visibility.Visible : Visibility.Collapsed;
            IsReportVisible = listQuyen.Contains("ReportView") ? Visibility.Visible : Visibility.Collapsed;
            IsAccountVisible = listQuyen.Contains("AccountView") ? Visibility.Visible : Visibility.Collapsed;
            IsSettingVisible = listQuyen.Contains("SettingView") ? Visibility.Visible : Visibility.Collapsed;

            // Command
            // ==========================================
            if (listQuyen.Contains("DashboardView"))
            {
                _dashboardViewModel = new DashboardViewModel();
                ShowDashboardCommand = new RelayCommand<object>((p) => _handleChangeView(_dashboardViewModel));

            }

            // Command 
            // ==========================================
            if (listQuyen.Contains("DashboardView"))
            { 
                _dashboardViewModel = new DashboardViewModel();
                ShowDashboardCommand = new RelayCommand<object>(async (p) =>
                {
                    await _dashboardViewModel.LoadAllDataAsync();
                    _handleChangeView(_dashboardViewModel);
                });
            }

            if (listQuyen.Contains("SaleView"))
            {
                _saleViewModel = new SaleViewModel();
                ShowBanHangCommand = new RelayCommand<object>(async (p) =>
                {
                    await _saleViewModel.LoadMasterData();
                    _handleChangeView(_saleViewModel);
                });
            }

            if (listQuyen.Contains("InvoiceView"))
            {
                _invoiceViewModel = new InvoiceViewModel();
                ShowHoaDonCommand = new RelayCommand<object>((p) => _handleChangeView(_invoiceViewModel));
            }

            if (listQuyen.Contains("ProductView"))
            {
                _productViewModel = new ProductViewModel();
                ShowTraCuuSachCommand = new RelayCommand<object>(async (p) =>
                {
                    await _productViewModel.LoadMasterData();

                    _handleChangeView(_productViewModel);
                });
            }

            if (listQuyen.Contains("CustomerView"))
            {
                _customerViewModel = new CustomerViewModel();
                ShowKhachHangCommand = new RelayCommand<object>((p) =>
                {
                    _customerViewModel.LoadMasterData();
                    _handleChangeView(_customerViewModel);
                });
            }

            if (listQuyen.Contains("ReceiptView"))
            {
                _receiptViewModel = new ReceiptViewModel();
                ShowPhieuThuCommand = new RelayCommand<object>((p) => _handleChangeView(_receiptViewModel));
            }

            if (listQuyen.Contains("ImportView"))
            {
                _importViewModel = new ImportViewModel();
                ShowNhapKhoCommand = new RelayCommand<object>((p) => _handleChangeView(_importViewModel));
            }

            if (listQuyen.Contains("SupplierView"))
            {
                _supplierViewModel = new SupplierViewModel();
                ShowNhaCungCapCommand = new RelayCommand<object>((p) => _handleChangeView(_supplierViewModel));
            }

            if (listQuyen.Contains("PromotionView"))
            {
                _promotionViewModel = new PromotionViewModel();
                ShowUuDaiCommand = new RelayCommand<object>((p) =>
                {
                    _ = _promotionViewModel.LoadMasterData();
                    _handleChangeView(_promotionViewModel);
                });
            }


            if (listQuyen.Contains("CategoryView"))
            {
                _categoryViewModel = new CategoryViewModel();
                //ShowDanhMucCommand = new RelayCommand<object>((p) => _handleChangeView(_categoryViewModel));
                ShowDanhMucCommand = new RelayCommand<object>(async (p) =>
                {
                    await _categoryViewModel.LoadAllDataAsync();
                    _handleChangeView(_categoryViewModel);
                });
            }

            if (listQuyen.Contains("ReportView"))
            {
                _reportViewModel = new ReportViewModel();
                ShowBaoCaoCommand = new RelayCommand<object>((p) => _handleChangeView(_reportViewModel));
            }

            if (listQuyen.Contains("AccountView"))
            {
                _accountViewModel = new AccountViewModel();
                ShowTaiKhoanCommand = new RelayCommand<object>((p) => _handleChangeView(_accountViewModel));
            }

            if (listQuyen.Contains("SettingView"))
            {
                _settingViewModel = new SettingViewModel();
                ShowCaiDatCommand = new RelayCommand<object>((p) => _handleChangeView(_settingViewModel));
            }

            // Đăng xuất
            LogoutCommand = new RelayCommand<object>((p) =>
            {
                Bookstore.WPF.Utils.AppState.Logout();

                var loginWindow = new Bookstore.WPF.Views.LoginView();
                loginWindow.Show();

                foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
                {
                    if (window is Bookstore.WPF.Views.MainView)
                    {
                        window.Close();
                        break;
                    }
                }
            });


            // Tự động Highlight Tab đầu tiên dựa theo danh sách quyền
            if (listQuyen.Contains("DashboardView")) IsDashboardChecked = true;
            else if (listQuyen.Contains("SaleView")) IsSaleChecked = true;
            else if (listQuyen.Contains("ProductView")) IsProductChecked = true;
            else if (listQuyen.Contains("CustomerView")) IsCustomerChecked = true;
            else if (listQuyen.Contains("ReceiptView")) IsReceiptChecked = true;
            else if (listQuyen.Contains("ImportView")) IsImportChecked = true;
            else if (listQuyen.Contains("SupplierView")) IsSupplierChecked = true;
            else if (listQuyen.Contains("PromotionView")) IsPromotionChecked = true;
            else if (listQuyen.Contains("CategoryView")) IsCategoryChecked = true;
            else if (listQuyen.Contains("ReportView")) IsReportChecked = true;
            else if (listQuyen.Contains("AccountView")) IsAccountChecked = true;
            else if (listQuyen.Contains("SettingView")) IsSettingChecked = true;
        }
    }
}