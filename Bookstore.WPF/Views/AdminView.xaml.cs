using Bookstore.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Bookstore.WPF.Views
{
    public partial class AdminView : Window
    {
        public AdminView()
        {
            InitializeComponent();
            this.DataContext = new AdminViewModel();

            this.Loaded += (s, e) => InitializeNavigation();

            // Thêm sự kiện cho SearchBox để xử lý placeholder
            if (SearchBox != null)
            {
                SearchBox.TextChanged += SearchBox_TextChanged;
            }
        }

        private void InitializeNavigation()
        {
            if (NavListBox != null)
            {
                NavListBox.SelectionChanged += OnNavigationSelectionChanged;
                if (NavListBox.SelectedIndex == -1)
                    NavListBox.SelectedIndex = 0;
            }
        }

        private void OnNavigationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavListBox.SelectedItem is ListBoxItem selectedItem)
            {
                string pageName = selectedItem.Tag?.ToString() ?? "Dashboard";

                if (pageName == "Logout")
                {
                    var result = MessageBox.Show("Are you sure you want to logout?",
                                                "Confirm Logout",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        this.Close();
                    }
                    NavListBox.SelectedIndex = 0;
                    return;
                }

                if (pageName == "Help")
                {
                    MessageBox.Show("Help content will be available soon.\n\nFor assistance, please contact support.",
                                   "Help", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavListBox.SelectedIndex = 0;
                    return;
                }

                PageTitleText.Text = pageName;

                var fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(150)));
                fadeOut.Completed += (s, _) =>
                {
                    ContentArea.Content = GetPageContent(pageName);
                    var fadeIn = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(250)));
                    ContentArea.BeginAnimation(UIElement.OpacityProperty, fadeIn);
                };
                ContentArea.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            }
        }

        private object GetPageContent(string pageName)
        {
            var stackPanel = new StackPanel();

            var card = new Border
            {
                Background = (Brush)FindResource("CardBg"),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(40),
                Child = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Children =
                    {
                        new TextBlock
                        {
                            Text = GetIconText(pageName),
                            FontSize = 64,
                            FontFamily = new FontFamily("Segoe MDL2 Assets"),
                            Foreground = (Brush)FindResource("PrimaryColor"),
                            HorizontalAlignment = HorizontalAlignment.Center
                        },
                        new TextBlock
                        {
                            Text = $"{pageName} Management",
                            FontSize = 24,
                            FontWeight = FontWeights.SemiBold,
                            Foreground = (Brush)FindResource("TextPrimary"),
                            Margin = new Thickness(0, 20, 0, 10),
                            HorizontalAlignment = HorizontalAlignment.Center
                        },
                        new TextBlock
                        {
                            Text = "This section is under development",
                            Foreground = (Brush)FindResource("TextSecondary"),
                            FontSize = 14,
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    }
                }
            };

            stackPanel.Children.Add(card);
            return stackPanel;
        }

        private string GetIconText(string pageName)
        {
            return pageName switch
            {
                "Dashboard" => "\xE8A1",
                "Products" => "\xE7D3",
                "Clients" => "\xE716",
                "Messages" => "\xE8B7",
                "Database" => "\xE8C5",
                "Notifications" => "\xEA8F",
                "Settings" => "\xE713",
                _ => "\xE8A1"
            };
        }

        // Xử lý Placeholder cho SearchBox
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchPlaceholder != null)
            {
                SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchBox.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }
    }
}