using System.Windows;
using System.Windows.Controls;

public static class PasswordHelper
{
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

    public static readonly DependencyProperty AttachProperty =
        DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper),
            new PropertyMetadata(false, OnAttachChanged));

    private static readonly DependencyProperty IsUpdatingProperty =
        DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper));

    public static void SetAttach(DependencyObject dp, bool value) => dp.SetValue(AttachProperty, value);
    public static bool GetAttach(DependencyObject dp) => (bool)dp.GetValue(AttachProperty);
    public static string GetPassword(DependencyObject dp) => (string)dp.GetValue(PasswordProperty);
    public static void SetPassword(DependencyObject dp, string value) => dp.SetValue(PasswordProperty, value);

    private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        PasswordBox? passwordBox = sender as PasswordBox;
        if (passwordBox != null && (bool)passwordBox.GetValue(AttachProperty))
        {
            passwordBox.PasswordChanged -= PasswordChanged;
            if (!(bool)passwordBox.GetValue(IsUpdatingProperty))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }
    }

    private static void OnAttachChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        PasswordBox? passwordBox = sender as PasswordBox;
        if (passwordBox == null) return;

        if ((bool)e.OldValue) passwordBox.PasswordChanged -= PasswordChanged;
        if ((bool)e.NewValue) passwordBox.PasswordChanged += PasswordChanged;
    }

    private static void PasswordChanged(object sender, RoutedEventArgs e)
    {
        PasswordBox? passwordBox = sender as PasswordBox;
        passwordBox.SetValue(IsUpdatingProperty, true);
        SetPassword(passwordBox, passwordBox.Password);
        passwordBox.SetValue(IsUpdatingProperty, false);
    }
}