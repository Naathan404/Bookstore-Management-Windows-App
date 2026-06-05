using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bookstore.WPF.Views.Components
{
    /// <summary>
    /// Interaction logic for FormComboBoxControl.xaml
    /// </summary>
    public partial class FormComboBoxControl : UserControl
    {
        public FormComboBoxControl()
        {
            InitializeComponent();
        }
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(FormComboBoxControl));

        public string TextValue { get => (string)GetValue(TextValueProperty); set => SetValue(TextValueProperty, value); }
        public static readonly DependencyProperty TextValueProperty = DependencyProperty.Register("TextValue", typeof(string), typeof(FormComboBoxControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string HintText { get => (string)GetValue(HintTextProperty); set => SetValue(HintTextProperty, value); }
        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText", typeof(string), typeof(FormComboBoxControl), new PropertyMetadata(""));

        public string HelperText { get => (string)GetValue(HelperTextProperty); set => SetValue(HelperTextProperty, value); }
        public static readonly DependencyProperty HelperTextProperty = DependencyProperty.Register("HelperText", typeof(string), typeof(FormComboBoxControl), new PropertyMetadata(""));

        public PackIconKind IconKind { get => (PackIconKind)GetValue(IconKindProperty); set => SetValue(IconKindProperty, value); }
        public static readonly DependencyProperty IconKindProperty = DependencyProperty.Register("IconKind", typeof(PackIconKind), typeof(FormComboBoxControl), new PropertyMetadata(PackIconKind.Pencil));

        public bool ShowIcon { get => (bool)GetValue(ShowIconProperty); set => SetValue(ShowIconProperty, value); }
        public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register("ShowIcon", typeof(bool), typeof(FormComboBoxControl), new PropertyMetadata(true));

        public bool IsRequired { get => (bool)GetValue(IsRequiredProperty); set => SetValue(IsRequiredProperty, value); }
        public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(FormComboBoxControl), new PropertyMetadata(false));

        public bool IsReadOnly { get => (bool)GetValue(IsReadOnlyProperty); set => SetValue(IsReadOnlyProperty, value); }
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(FormComboBoxControl), new PropertyMetadata(false));

        public FieldState State { get => (FieldState)GetValue(StateProperty); set => SetValue(StateProperty, value); }
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(FieldState), typeof(FormComboBoxControl), new PropertyMetadata(FieldState.Normal));

        // (Thêm các property: Title, HintText, HelperText, IconKind, ShowIcon, IsRequired, IsReadOnly, State giống y hệt FormInputControl)
        // Bổ sung các prop riêng cho ComboBox:
        public IEnumerable ItemsSource { get { return (IEnumerable)GetValue(ItemsSourceProperty); } set { SetValue(ItemsSourceProperty, value); } }
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(FormComboBoxControl), new PropertyMetadata(null));

        public object SelectedItem { get { return GetValue(SelectedItemProperty); } set { SetValue(SelectedItemProperty, value); } }
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(FormComboBoxControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string DisplayMemberPath { get { return (string)GetValue(DisplayMemberPathProperty); } set { SetValue(DisplayMemberPathProperty, value); } }
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(FormComboBoxControl), new PropertyMetadata(string.Empty));

        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(FormComboBoxControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(FormComboBoxControl), new PropertyMetadata(string.Empty));
        public bool IsEditable { get { return (bool)GetValue(IsEditableProperty); } set { SetValue(IsEditableProperty, value); } }
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(FormComboBoxControl), new PropertyMetadata(false));

        public static readonly DependencyProperty IsTextSearchEnabledProperty = DependencyProperty.Register("IsTextSearchEnabled", typeof(bool), typeof(FormComboBoxControl), new PropertyMetadata(true));

        public bool IsTextSearchEnabled
        {
            get { return (bool)GetValue(IsTextSearchEnabledProperty); }
            set { SetValue(IsTextSearchEnabledProperty, value); }
        }

        public static readonly DependencyProperty StaysOpenOnEditProperty =
            DependencyProperty.Register("StaysOpenOnEdit", typeof(bool), typeof(FormComboBoxControl), new PropertyMetadata(false));

        public bool StaysOpenOnEdit
        {
            get { return (bool)GetValue(StaysOpenOnEditProperty); }
            set { SetValue(StaysOpenOnEditProperty, value); }
        }
        public static readonly DependencyProperty ItemTemplateProperty =
    DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(FormComboBoxControl), new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
    }
}
