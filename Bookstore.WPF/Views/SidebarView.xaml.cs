using System.Windows.Controls;
using Bookstore.WPF.ViewModels;

namespace Bookstore.WPF.Views
{
    /// <summary>
    /// Interaction logic for SidebarView.xaml
    /// </summary>
    public partial class SidebarView : UserControl
    {
        public SidebarView()
        {
            InitializeComponent();

            //this.DataContext = new SidebarViewModel();
        }
    }
}
