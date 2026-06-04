using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bookstore.WPF.Views.Components
{
    public partial class DataGridActionPanel : UserControl
    {
        public DataGridActionPanel()
        {
            InitializeComponent();
        }

        // 1. DỮ LIỆU CỦA DÒNG (Để truyền vào CommandParameter)
        public object RowItem
        {
            get { return GetValue(RowItemProperty); }
            set { SetValue(RowItemProperty, value); }
        }
        public static readonly DependencyProperty RowItemProperty = DependencyProperty.Register("RowItem", typeof(object), typeof(DataGridActionPanel), new PropertyMetadata(null));

        // 2. CÁC NÚT CƠ BẢN (XEM / SỬA / XÓA)
        public bool ShowEdit { get => (bool)GetValue(ShowEditProperty); set => SetValue(ShowEditProperty, value); }
        public static readonly DependencyProperty ShowEditProperty = DependencyProperty.Register("ShowEdit", typeof(bool), typeof(DataGridActionPanel), new PropertyMetadata(false));
        public ICommand EditCommand { get => (ICommand)GetValue(EditCommandProperty); set => SetValue(EditCommandProperty, value); }
        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register("EditCommand", typeof(ICommand), typeof(DataGridActionPanel), new PropertyMetadata(null));

        public bool ShowDelete { get => (bool)GetValue(ShowDeleteProperty); set => SetValue(ShowDeleteProperty, value); }
        public static readonly DependencyProperty ShowDeleteProperty = DependencyProperty.Register("ShowDelete", typeof(bool), typeof(DataGridActionPanel), new PropertyMetadata(false));
        public ICommand DeleteCommand { get => (ICommand)GetValue(DeleteCommandProperty); set => SetValue(DeleteCommandProperty, value); }
        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(DataGridActionPanel), new PropertyMetadata(null));

        // 3. NÚT NGHIỆP VỤ ĐẶC BIỆT (XANH LÁ)
        public bool ShowAction { get => (bool)GetValue(ShowActionProperty); set => SetValue(ShowActionProperty, value); }
        public static readonly DependencyProperty ShowActionProperty = DependencyProperty.Register("ShowAction", typeof(bool), typeof(DataGridActionPanel), new PropertyMetadata(false));

        public ICommand ActionCommand { get => (ICommand)GetValue(ActionCommandProperty); set => SetValue(ActionCommandProperty, value); }
        public static readonly DependencyProperty ActionCommandProperty = DependencyProperty.Register("ActionCommand", typeof(ICommand), typeof(DataGridActionPanel), new PropertyMetadata(null));

        public string ActionText { get => (string)GetValue(ActionTextProperty); set => SetValue(ActionTextProperty, value); }
        public static readonly DependencyProperty ActionTextProperty = DependencyProperty.Register("ActionText", typeof(string), typeof(DataGridActionPanel), new PropertyMetadata("Thao tác"));

        public PackIconKind ActionIcon { get => (PackIconKind)GetValue(ActionIconProperty); set => SetValue(ActionIconProperty, value); }
        public static readonly DependencyProperty ActionIconProperty = DependencyProperty.Register("ActionIcon", typeof(PackIconKind), typeof(DataGridActionPanel), new PropertyMetadata(PackIconKind.Check));

        public bool IsActionEnabled { get => (bool)GetValue(IsActionEnabledProperty); set => SetValue(IsActionEnabledProperty, value); }
        public static readonly DependencyProperty IsActionEnabledProperty = DependencyProperty.Register("IsActionEnabled", typeof(bool), typeof(DataGridActionPanel), new PropertyMetadata(true));

        public string ActionToolTip { get => (string)GetValue(ActionToolTipProperty); set => SetValue(ActionToolTipProperty, value); }
        public static readonly DependencyProperty ActionToolTipProperty = DependencyProperty.Register("ActionToolTip", typeof(string), typeof(DataGridActionPanel), new PropertyMetadata(""));


        public static readonly DependencyProperty ShowViewProperty =
                DependencyProperty.Register("ShowView", typeof(bool), typeof(DataGridActionPanel), new PropertyMetadata(false));

        public bool ShowView
        {
            get { return (bool)GetValue(ShowViewProperty); }
            set { SetValue(ShowViewProperty, value); }
        }

        public static readonly DependencyProperty ViewCommandProperty =
            DependencyProperty.Register("ViewCommand", typeof(ICommand), typeof(DataGridActionPanel), new PropertyMetadata(null));

        public ICommand ViewCommand
        {
            get { return (ICommand)GetValue(ViewCommandProperty); }
            set { SetValue(ViewCommandProperty, value); }
        }
    }
}