using MaterialDesignThemes.Wpf;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Bookstore.WPF.Views.Components
{
    public partial class FilterComboBoxControl : UserControl
    {
        public FilterComboBoxControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => AutoSelectFirstItem();
        }

        // 1. Icon và HintText
        public PackIconKind IconKind
        {
            get { return (PackIconKind)GetValue(IconKindProperty); }
            set { SetValue(IconKindProperty, value); }
        }
        public static readonly DependencyProperty IconKindProperty =
            DependencyProperty.Register("IconKind", typeof(PackIconKind), typeof(FilterComboBoxControl), new PropertyMetadata(PackIconKind.FormatListBulleted));

        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }
        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(string), typeof(FilterComboBoxControl), new PropertyMetadata("Chọn..."));

        // 2. Nguồn dữ liệu (ItemsSource) - BẮT SỰ KIỆN NẠP DATA THÔNG MINH
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(FilterComboBoxControl),
                new PropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FilterComboBoxControl control)
            {
                // Hủy lắng nghe danh sách cũ để chống tràn RAM
                if (e.OldValue is INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= control.OnCollectionChanged;
                }

                // Bắt đầu lắng nghe sự thay đổi của danh sách mới (khi ViewModel gọi .Add hoặc .Clear)
                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    newCollection.CollectionChanged += control.OnCollectionChanged;
                }

                // Chạy thử luôn phòng trường hợp danh sách đã có sẵn data từ đầu
                control.AutoSelectFirstItem();
            }
        }

        // Hàm này sẽ tự động chạy mỗi khi API đổ data xong và ViewModel gọi .Add()
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            AutoSelectFirstItem();
        }

        // CHÌA KHÓA: TỰ ĐỘNG CHỌN OPTION ĐẦU TIÊN
        private void AutoSelectFirstItem()
        {
            // Tránh việc ghi đè nếu người dùng đã cố tình chọn một item khác
            if (ItemsSource != null && (SelectedItem == null || SelectedIndex == -1))
            {
                var enumerator = ItemsSource.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    // Gán cả 2 thuộc tính để đảm bảo Binding 2 chiều bắn tín hiệu chuẩn về ViewModel
                    SelectedItem = enumerator.Current;
                    SelectedIndex = 0;
                }
            }
        }

        // 3. Các thuộc tính Binding 2 chiều
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(FilterComboBoxControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(FilterComboBoxControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(FilterComboBoxControl),
                new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)); // Đổi mặc định về -1 cho an toàn

        // 4. Các đường dẫn (Path)
        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }
        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(FilterComboBoxControl), new PropertyMetadata(string.Empty));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(FilterComboBoxControl), new PropertyMetadata(string.Empty));
    }
}