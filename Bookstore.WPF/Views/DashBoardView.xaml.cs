using Bookstore.WPF.ViewModels;
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
    /// Interaction logic for DashBoardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            //DataContext = new DashboardViewModel(); 
        }


        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;

                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;

                // Tìm ScrollViewer tổ tiên gần nhất
                DependencyObject parent = VisualTreeHelper.GetParent(sender as DependencyObject);
                while (parent != null && !(parent is ScrollViewer))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (parent is ScrollViewer scrollViewer)
                {
                    scrollViewer.RaiseEvent(eventArg);
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ChartRevenue != null) ChartRevenue.Series = null;
            if (ChartCategory != null) ChartCategory.Series = null;
            if (ChartComparison != null) ChartComparison.Series = null;
            this.DataContext = null;
        }
    }
}
