using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bookstore.WPF.Views.Components
{
    public partial class PaginationControl : UserControl
    {
        public PaginationControl()
        {
            InitializeComponent();
        }

        // Trang hiện tại
        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(PaginationControl), new PropertyMetadata(1));

        // Tổng số trang
        public int TotalPages
        {
            get { return (int)GetValue(TotalPagesProperty); }
            set { SetValue(TotalPagesProperty, value); }
        }
        public static readonly DependencyProperty TotalPagesProperty =
            DependencyProperty.Register("TotalPages", typeof(int), typeof(PaginationControl), new PropertyMetadata(1));

        // Tổng số bản ghi (Dữ liệu)
        public int TotalRecords
        {
            get { return (int)GetValue(TotalRecordsProperty); }
            set { SetValue(TotalRecordsProperty, value); }
        }
        public static readonly DependencyProperty TotalRecordsProperty =
            DependencyProperty.Register("TotalRecords", typeof(int), typeof(PaginationControl), new PropertyMetadata(0));

        // CHỈ 1 LỆNH DUY NHẤT
        public ICommand PaginationCommand
        {
            get { return (ICommand)GetValue(PaginationCommandProperty); }
            set { SetValue(PaginationCommandProperty, value); }
        }
        public static readonly DependencyProperty PaginationCommandProperty =
            DependencyProperty.Register("PaginationCommand", typeof(ICommand), typeof(PaginationControl), new PropertyMetadata(null));
    }
}