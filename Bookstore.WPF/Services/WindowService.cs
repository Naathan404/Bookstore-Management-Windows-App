using Bookstore.WPF.Services;
using Bookstore.WPF.ViewModels;
using Bookstore.WPF.Views;
using System.Windows;

public class WindowService : IWindowService
{
    public void ShowWindow<TViewModel>() where TViewModel : BaseViewModel
    {
        Window window = null;

        if (typeof(TViewModel) == typeof(MainViewModel))
        {
            window = new MainView();
        }
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