using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore.WPF.Views.Components
{
    public partial class FormInputControl : UserControl
    {
        public FormInputControl() { InitializeComponent(); }

        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(FormInputControl));

        public string TextValue { get => (string)GetValue(TextValueProperty); set => SetValue(TextValueProperty, value); }
        public static readonly DependencyProperty TextValueProperty = DependencyProperty.Register("TextValue", typeof(string), typeof(FormInputControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double InputHeight
        {
            get { return (double)GetValue(InputHeightProperty); }
            set { SetValue(InputHeightProperty, value); }
        }
        public static readonly DependencyProperty InputHeightProperty =
            DependencyProperty.Register("InputHeight", typeof(double), typeof(FormInputControl), new PropertyMetadata(42.0));
        public bool IsMultiLine
        {
            get { return (bool)GetValue(IsMultiLineProperty); }
            set { SetValue(IsMultiLineProperty, value); }
        }

        public static readonly DependencyProperty IsMultiLineProperty =
            DependencyProperty.Register("IsMultiLine", typeof(bool), typeof(FormInputControl), new PropertyMetadata(false));
        public string HintText { get => (string)GetValue(HintTextProperty); set => SetValue(HintTextProperty, value); }
        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText", typeof(string), typeof(FormInputControl), new PropertyMetadata(""));

        public string HelperText { get => (string)GetValue(HelperTextProperty); set => SetValue(HelperTextProperty, value); }
        public static readonly DependencyProperty HelperTextProperty = DependencyProperty.Register("HelperText", typeof(string), typeof(FormInputControl), new PropertyMetadata(""));

        public string UnitText
        {
            get { return (string)GetValue(UnitTextProperty); }
            set { SetValue(UnitTextProperty, value); }
        }

        public static readonly DependencyProperty UnitTextProperty =
            DependencyProperty.Register("UnitText", typeof(string), typeof(FormInputControl), new PropertyMetadata(string.Empty));

        public PackIconKind IconKind { get => (PackIconKind)GetValue(IconKindProperty); set => SetValue(IconKindProperty, value); }
        public static readonly DependencyProperty IconKindProperty = DependencyProperty.Register("IconKind", typeof(PackIconKind), typeof(FormInputControl), new PropertyMetadata(PackIconKind.Pencil));

        public bool ShowIcon { get => (bool)GetValue(ShowIconProperty); set => SetValue(ShowIconProperty, value); }
        public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register("ShowIcon", typeof(bool), typeof(FormInputControl), new PropertyMetadata(true));

        public bool IsRequired { get => (bool)GetValue(IsRequiredProperty); set => SetValue(IsRequiredProperty, value); }
        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(FormInputControl), new PropertyMetadata(false));

        public bool IsReadOnly { get => (bool)GetValue(IsReadOnlyProperty); set => SetValue(IsReadOnlyProperty, value); }
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(FormInputControl), new PropertyMetadata(false));

        public FieldState State { get => (FieldState)GetValue(StateProperty); set => SetValue(StateProperty, value); }
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(FieldState), typeof(FormInputControl), new PropertyMetadata(FieldState.Normal));
        // Thêm công tắc IsCurrencyMode vào FormInputControl
        public bool IsCurrencyMode
        {
            get { return (bool)GetValue(IsCurrencyModeProperty); }
            set { SetValue(IsCurrencyModeProperty, value); }
        }
        public static readonly DependencyProperty IsCurrencyModeProperty =
            DependencyProperty.Register("IsCurrencyMode", typeof(bool), typeof(FormInputControl), new PropertyMetadata(false));
    }
}