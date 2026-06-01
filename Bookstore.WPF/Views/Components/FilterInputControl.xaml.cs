using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore.WPF.Views.Components
{
    public partial class FilterInputControl : UserControl
    {
        public FilterInputControl()
        {
            InitializeComponent();
        }

        // 1. Text (Giá trị người dùng gõ vào, Bind với ViewModel)
        public string TextValue
        {
            get { return (string)GetValue(TextValueProperty); }
            set { SetValue(TextValueProperty, value); }
        }
        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string), typeof(FilterInputControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        // 2. HintText (Chữ gợi ý mờ mờ)
        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }
        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(string), typeof(FilterInputControl), new PropertyMetadata("Nhập nội dung..."));

        // 3. IconKind (Icon hiển thị bên trái)
        public PackIconKind IconKind
        {
            get { return (PackIconKind)GetValue(IconKindProperty); }
            set { SetValue(IconKindProperty, value); }
        }
        public static readonly DependencyProperty IconKindProperty =
            DependencyProperty.Register("IconKind", typeof(PackIconKind), typeof(FilterInputControl), new PropertyMetadata(PackIconKind.Magnify));
    }
}