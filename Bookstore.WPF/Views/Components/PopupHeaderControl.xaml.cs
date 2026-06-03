using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bookstore.WPF.Views.Components
{
    public partial class PopupHeaderControl : UserControl
    {
        public PopupHeaderControl()
        {
            InitializeComponent();
        }

        // 1. TIÊU ĐỀ
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PopupHeaderControl), new PropertyMetadata(string.Empty));

        // 2. ICON
        public string IconKind
        {
            get { return (string)GetValue(IconKindProperty); }
            set { SetValue(IconKindProperty, value); }
        }
        public static readonly DependencyProperty IconKindProperty =
            DependencyProperty.Register("IconKind", typeof(string), typeof(PopupHeaderControl), new PropertyMetadata("TicketPercent"));
    }
}