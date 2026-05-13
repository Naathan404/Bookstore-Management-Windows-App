using Bookstore.WPF.ViewModels;
using Bookstore.WPF.Views;
using MaterialDesignThemes.Wpf;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Bookstore.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await Task.Run(async () => {
                try
                {
                    using var client = new HttpClient();
                    await client.GetAsync("https://localhost:7001/api/System/ping");
                }
                catch {}
            });


            LiveCharts.Configure(config =>
                config.AddSkiaSharp()
                      .AddDefaultMappers()
                      .AddLightTheme()
            );

            //var login = new LoginView();
            //login.Show();
            // Thử DashboardView
            //var mainView = new MainView();
            //mainView.Show();
            //var admin = new AdminView();
            //admin.DataContext = new AdminViewModel();
            //admin.Show();
            //var window = new Window
            //{
            //    Content = new ImportView(),
            //    Title = "NHẬP KHẨU",
            //    WindowState = WindowState.Maximized
            //};
            //window.Show();

            var window = new Window
            {
                Content = new SupplierView(),
                Title = "NHẬP KHẨU",
                WindowState = WindowState.Maximized
            };
            window.Show();


        }

        private async void App_Startup(object sender, StartupEventArgs e)
        {
            // nữa mà có slash sceen thì đặt ở đây nha :333 

        }

        
    }

}
