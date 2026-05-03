using Bookstore.WPF.ViewModels;
using System.Windows;

namespace Bookstore.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainViewModel.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
