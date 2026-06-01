using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bookstore.WPF.Views.Components
{
    public partial class FilterActionPanel : UserControl
    {
        public FilterActionPanel() { InitializeComponent(); }

        public ICommand ClearCommand
        {
            get { return (ICommand)GetValue(ClearCommandProperty); }
            set { SetValue(ClearCommandProperty, value); }
        }
        public static readonly DependencyProperty ClearCommandProperty =
            DependencyProperty.Register("ClearCommand", typeof(ICommand), typeof(FilterActionPanel), new PropertyMetadata(null));

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }
        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(FilterActionPanel), new PropertyMetadata(null));

        // Cho phép ẩn hiện nút Xóa Lọc (Mặc định True)
        public bool ShowClearButton
        {
            get { return (bool)GetValue(ShowClearButtonProperty); }
            set { SetValue(ShowClearButtonProperty, value); }
        }
        public static readonly DependencyProperty ShowClearButtonProperty =
            DependencyProperty.Register("ShowClearButton", typeof(bool), typeof(FilterActionPanel), new PropertyMetadata(true));

        // Cho phép ẩn hiện nút Refresh (Mặc định True)
        public bool ShowRefreshButton
        {
            get { return (bool)GetValue(ShowRefreshButtonProperty); }
            set { SetValue(ShowRefreshButtonProperty, value); }
        }
        public static readonly DependencyProperty ShowRefreshButtonProperty =
            DependencyProperty.Register("ShowRefreshButton", typeof(bool), typeof(FilterActionPanel), new PropertyMetadata(true));
    }
}