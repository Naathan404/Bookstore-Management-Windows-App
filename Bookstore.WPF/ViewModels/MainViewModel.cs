using Bookstore.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public SidebarViewModel SidebarVM { get; set; }
        public MainViewModel(List<string> listQuyen)
        {
            SidebarVM = new SidebarViewModel(listQuyen, ChangeView);
            CurrentView = new DashboardViewModel();
            SetDefaultView(listQuyen);
        }
        private void SetDefaultView(List<string> listQuyen)
        {
            if (listQuyen == null || listQuyen.Count == 0)
            {
                CurrentView = null; 
                return;
            }

            if (listQuyen.Contains("DashboardView"))
                CurrentView = new DashboardViewModel();
            else if (listQuyen.Contains("SaleView"))
                CurrentView = new SaleViewModel();
            else if (listQuyen.Contains("ProductView"))
                CurrentView = new ProductViewModel();
            // bổ sung thêm 
        }
        private void ChangeView(object newViewModel)
        {
            CurrentView = newViewModel;
        }
    }
}
