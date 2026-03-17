using Bookstore.WPF.Services;
using System.Windows.Input;
using System;
using System.Windows;

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
            // Cập nhật lại trạng thái hiển thị của các View
            OnPropertyChanged(nameof(IsLoginVisible));
            OnPropertyChanged(nameof(IsForgotVisible));
            OnPropertyChanged(nameof(IsVerifyVisible));
            OnPropertyChanged(nameof(IsResetVisible));
        }
    }

    public string Title { get; private set; } = "Login";
    public string SubTitle { get; private set; } = "Welcome back! Have a nice day :3";

    // --- Data Properties (Binding vào TextBox/PasswordBox) ---
    public string Username { get; set; }
    public string Email { get; set; }
    public string OTP { get; set; }
    // Lưu ý: Password nên xử lý qua PasswordBoxAssistant hoặc CommandParameter để bảo mật

    // --- Commands ---
    public ICommand SwitchStateCommand { get; }
    public ICommand LoginCommand { get; }
    public ICommand SendOTPCommand { get; }
    public ICommand VerifyOTPCommand { get; }
    public ICommand ResetPasswordCommand { get; }

    public LoginViewModel()
    {
        // Điều hướng giữa các màn hình
        SwitchStateCommand = new RelayCommand<string>((p) => {
            if (Enum.TryParse(p, out LoginState newState))
                CurrentState = newState;
        });

        // Giả lập các logic nghiệp vụ
        LoginCommand = new RelayCommand<object>((p) => {
            MessageBox.Show($"Đang đăng nhập cho: {Username}");
            // Sau này gọi API của Hưng ở đây
        });

        SendOTPCommand = new RelayCommand<object>((p) => {
            CurrentState = LoginState.Verify; // Chuyển sang nhập OTP
        });

        VerifyOTPCommand = new RelayCommand<object>((p) => {
            CurrentState = LoginState.Reset; // OTP đúng thì cho reset
        });

        ResetPasswordCommand = new RelayCommand<object>((p) => {
            MessageBox.Show("Mật khẩu đã đổi! Về đăng nhập thôi.");
            CurrentState = LoginState.Login;
        });
    }

    // --- Visibility Helpers ---
    public Visibility IsLoginVisible => CurrentState == LoginState.Login ? Visibility.Visible : Visibility.Collapsed;
    public Visibility IsForgotVisible => CurrentState == LoginState.Forgot ? Visibility.Visible : Visibility.Collapsed;
    public Visibility IsVerifyVisible => CurrentState == LoginState.Verify ? Visibility.Visible : Visibility.Collapsed;
    public Visibility IsResetVisible => CurrentState == LoginState.Reset ? Visibility.Visible : Visibility.Collapsed;

    private void UpdateHeader()
    {
        switch (CurrentState)
        {
            case LoginState.Login: Title = "Login"; SubTitle = "Welcome back! Have a nice day :3"; break;
            case LoginState.Forgot: Title = "Recovery"; SubTitle = "Nhập email Sahara của ông nhé!"; break;
            case LoginState.Verify: Title = "Verify"; SubTitle = "Mã xác thực đã bay tới mail ông rồi."; break;
            case LoginState.Reset: Title = "Reset"; SubTitle = "Thiết lập mật khẩu mới thật bảo mật."; break;
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