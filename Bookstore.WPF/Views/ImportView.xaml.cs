using System;
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

namespace Bookstore.WPF.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : UserControl
    {
        public ImportView()
        {
            InitializeComponent();
        }

        private void DataGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Bỏ qua sự kiện cuộn hiện tại của DataGrid
            e.Handled = true;

            // Tạo ra một sự kiện cuộn chuột mới y hệt
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;

            // Bắn sự kiện đó lên cho thằng cha (ScrollViewer bọc ngoài) xử lý
            var parent = VisualTreeHelper.GetParent((DependencyObject)sender) as UIElement;
            parent?.RaiseEvent(eventArg);
        }
    }
}
