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

        private void btnTrang1_Click(object sender, RoutedEventArgs e)
        {

        }
        private void PopupDetail_Close(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.ImportViewModel vm)
            {
                vm.CloseDetailPopup();
            }
        }
        private void PopupAdd_Close(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.ImportViewModel vm)
            {
                vm.CloseAddPopup();
            }
        }
    }
}
