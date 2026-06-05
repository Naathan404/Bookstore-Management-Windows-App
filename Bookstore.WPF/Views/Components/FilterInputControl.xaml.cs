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
        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(FilterInputControl), new PropertyMetadata(true));
        // 1. Thuộc tính canh lề (Trái, Giữa, Phải)
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(FilterInputControl), new PropertyMetadata(TextAlignment.Left));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        // 2. Thuộc tính bật/tắt định dạng tiền tệ
        public static readonly DependencyProperty IsCurrencyModeProperty =
            DependencyProperty.Register("IsCurrencyMode", typeof(bool), typeof(FilterInputControl), new PropertyMetadata(false));

        public bool IsCurrencyMode
        {
            get { return (bool)GetValue(IsCurrencyModeProperty); }
            set { SetValue(IsCurrencyModeProperty, value); }
        }

        public static readonly DependencyProperty UnitTextProperty =
    DependencyProperty.Register("UnitText", typeof(string), typeof(FilterInputControl), new PropertyMetadata(string.Empty));

        public string UnitText
        {
            get { return (string)GetValue(UnitTextProperty); }
            set { SetValue(UnitTextProperty, value); }
        }

    }
}