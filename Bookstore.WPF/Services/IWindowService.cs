using Bookstore.WPF.Services;
using System.Windows;

public interface IWindowService
{
    void ShowWindow<TViewModel>(List<string> listQuyen) where TViewModel : BaseViewModel;
    void CloseWindow<TViewModel>() where TViewModel : BaseViewModel;
}