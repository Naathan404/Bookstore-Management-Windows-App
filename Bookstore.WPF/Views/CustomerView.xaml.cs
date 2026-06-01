using System.Windows.Controls;
using Bookstore.WPF.ViewModels;

namespace Bookstore.WPF.Views
{
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
            this.DataContext = new CustomerViewModel();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Tag = (e.Row.GetIndex() + 1).ToString();
        }
    }
}