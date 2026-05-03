using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels;
using Bookstore.WPF.Views;
using System.Windows;

public class WindowService : IWindowService
{
    public void ShowWindow<TViewModel>(List<string>  listQuyen) where TViewModel : BaseViewModel
    {
        Window window = null;

        //if (typeof(TViewModel) == typeof(AdminViewModel))
        //{
        //    window = new AdminView();
        //}
        //else if (typeof(TViewModel) == typeof(StaffViewModel))
        //{
        //    window = new StaffView();
        //}

        if (window != null)
        {
            window.Show();
        }
    }

    public void CloseWindow<TViewModel>() where TViewModel : BaseViewModel
    {
        var window = Application.Current.Windows.OfType<Window>()
            .FirstOrDefault(w => w.DataContext is TViewModel);
        window?.Close();
    }
}