using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore.WPF.Views.Components
{
    public partial class FilterDatePickerControl : UserControl
    {
        public FilterDatePickerControl()
        {
            InitializeComponent();
        }

        // 1. Icon và HintText
        public PackIconKind IconKind
        {
            get { return (PackIconKind)GetValue(IconKindProperty); }
            set { SetValue(IconKindProperty, value); }
        }
        public static readonly DependencyProperty IconKindProperty =
            DependencyProperty.Register("IconKind", typeof(PackIconKind), typeof(FilterDatePickerControl), new PropertyMetadata(PackIconKind.Calendar));

        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }
        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(string), typeof(FilterDatePickerControl), new PropertyMetadata("Chọn ngày..."));

        // 2. Dữ liệu Ngày tháng (Dùng DateTime? để hỗ trợ trường hợp Null)
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(FilterDatePickerControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}