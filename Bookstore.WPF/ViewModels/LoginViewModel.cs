using Bookstore.WPF.Services;
using System;
using System.Net;
using System.Security;
using System.Windows;
using System.Windows.Input;

public class LoginViewModel : BaseViewModel
{
    // --- State & Header Logic ---
    private LoginState _currentState = LoginState.Login;
    public LoginState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            OnPropertyChanged();
            UpdateHeader();
            
            OnPropertyChanged(nameof(IsLoginVisible));
            OnPropertyChanged(nameof(IsForgotVisible));
            OnPropertyChanged(nameof(IsVerifyVisible));
            OnPropertyChanged(nameof(IsResetVisible));
        }
    }

    public string Title { get; private set; } = "Login";
    public string SubTitle { get; private set; } = "Welcome back! Have a nice day :3";

    // --- Data Properties
    public string Username { get; set; }
    public string Password { get; private set; }
    public SecureString SecurePassword { private get; set; }

    public string Email { get; set; }
    public string OTP { get; set; }
    // Lưu ý: Password nên xử lý qua PasswordBoxAssistant hoặc CommandParameter để bảo mật

    // --- Commands ---
    public ICommand SwitchStateCommand { get; }
    public ICommand LoginCommand { get; }
    public ICommand SendOTPCommand { get; }
    public ICommand VerifyOTPCommand { get; }
    public ICommand ResetPasswordCommand { get; }
    public ICommand ResendOTPCommand { get; }

    public LoginViewModel()
    {
       
        SwitchStateCommand = new RelayCommand<string>((p) => {
            if (Enum.TryParse(p, out LoginState newState))
                CurrentState = newState;
        });

        
        LoginCommand = new RelayCommand<object>((p) => {
            string plainText = new NetworkCredential("", SecurePassword).Password;
            MessageBox.Show($"Login for: username - {Username} and password - {plainText}");
            // call api hiaa
        });

        SendOTPCommand = new RelayCommand<object>((p) => {
            MessageBox.Show("Da gui OTP ve email");
            CurrentState = LoginState.Verify; 
        });

        ResendOTPCommand = new RelayCommand<object>((p) =>
        {
            MessageBox.Show("Da gui lai OTP ve email");
        });

        VerifyOTPCommand = new RelayCommand<object>((p) => {
            MessageBox.Show("Xac thuc OTP thanh cong");
            CurrentState = LoginState.Reset;
        });

        ResetPasswordCommand = new RelayCommand<object>((p) => {
            MessageBox.Show("Mật khẩu đã đổi! Về đăng nhập thôi.");
            CurrentState = LoginState.Login;
        });
    }

    // Visibility Helpers
    public Visibility IsLoginVisible => CurrentState == LoginState.Login ? Visibility.Visible : Visibility.Collapsed;
    public Visibility IsForgotVisible => CurrentState == LoginState.Forgot ? Visibility.Visible : Visibility.Collapsed;
    public Visibility IsVerifyVisible => CurrentState == LoginState.Verify ? Visibility.Visible : Visibility.Collapsed;
    public Visibility IsResetVisible => CurrentState == LoginState.Reset ? Visibility.Visible : Visibility.Collapsed;

    private void UpdateHeader()
    {
        switch (CurrentState)
        {
            case LoginState.Login: Title = "Login"; SubTitle = "Welcome back! Have a nice day :3"; break;
            case LoginState.Forgot: Title = "Recovery"; SubTitle = "Enter your registered email"; break;
            case LoginState.Verify: Title = "Verify"; SubTitle = "We have sent OTP to your email!"; break;
            case LoginState.Reset: Title = "Reset"; SubTitle = "Enter your new password."; break;
        }
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(SubTitle));
    }
}
public enum LoginState 
{ 
    Login,
    Forgot,
    Verify,
    Reset 
}