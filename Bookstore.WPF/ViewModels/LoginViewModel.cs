using Bookstore.Share.DTORequests;
using Bookstore.Share.DTOResponses;
using Bookstore.WPF.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Windows;
using System.Windows.Input;
using System.Net.Http.Json;
using CoffeeShop.Helper;
using Bookstore.WPF.Views;
using Bookstore.WPF.ViewModels;
using System.IO.Packaging;
using System.Net.WebSockets;

public class LoginViewModel : BaseViewModel
{
    private WindowService _windowService = new WindowService();
    public Action OnLoginFailed { get; set; }
    #region Properties
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

    // log lỗi
    private Visibility _isLoginVisible = Visibility.Hidden;
    public Visibility IsErrorLogVisible
    {
        get => _isLoginVisible;
        set
        {
            _isLoginVisible = value;
            OnPropertyChanged();
        }
    }
    private string _errorLog = String.Empty;
    public string ErrorLog
    {
        get => _errorLog;
        set
        {
            _errorLog = value;
            OnPropertyChanged();
        }
    }
    
    #endregion

    public string Title { get; private set; } = "Login";
    public string SubTitle { get; private set; } = "Welcome back! Have a nice day :3";

    // --- Data Properties
    private string _username = String.Empty;
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }
    private SecureString _securePassword = new SecureString();
    public SecureString SecurePassword
    {
        private get => _securePassword;
        set
        {
            _securePassword = value;
            OnPropertyChanged();
        }
                  
    }

    public string Email { get; set; } = String.Empty;
    public string OTP { get; set; } = String.Empty;
    // Lưu ý: Password nên xử lý qua PasswordBoxAssistant hoặc CommandParameter để bảo mật

    #region Commands
    // --- Commands ---
    public ICommand SwitchStateCommand { get; }
    public ICommand LoginCommand { get; }
    public ICommand SendOTPCommand { get; }
    public ICommand VerifyOTPCommand { get; }
    public ICommand ResetPasswordCommand { get; }
    public ICommand ResendOTPCommand { get; }
    #endregion
    public LoginViewModel()
    {

        SwitchStateCommand = new RelayCommand<string>((p) => {
            if (Enum.TryParse(p, out LoginState newState))
                CurrentState = newState;
            IsErrorLogVisible = Visibility.Hidden;
        });

        
        LoginCommand = new RelayCommand<object>(async (p) => {
            string plainText = new NetworkCredential("", SecurePassword).Password;
            // call api hiaa
            var requestData = new LoginRequest
            {
                username = this.Username,
                password = HashHelper.SHA256_Encode(HashHelper.Base64_Encode(plainText))
            };
            MessageBox.Show($"username: {requestData.username}, pw: {plainText}");
            using (var client = new HttpClient())
            {
                try
                {
                    // Gửi requestData -> server
                    // Hàm PostAsJsonAsync tự động bọc requestData thành JSON
                    var response = await client.PostAsJsonAsync("https://localhost:7001/api/Auth/login", requestData);

                    // nếu response trả về là thành công thì thực hiện đăng nhập
                    if (response.IsSuccessStatusCode)
                    {
                        // đọc response
                        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                        if (result != null)
                        {
                            IsErrorLogVisible = Visibility.Hidden;
                            if(result.Role == 0)        // admin
                            {
                                _windowService.ShowWindow<AdminViewModel>();
                            }
                            else if(result.Role == 1)
                            {
                                _windowService.ShowWindow<StaffViewModel>();
                            }
                            _windowService.CloseWindow<LoginViewModel>();

                        }
                    }
                    else
                    {
                        var errorDetail = await response.Content.ReadAsStringAsync();
                        //MessageBox.Show($"Server từ chối (Mã {response.StatusCode}): {errorDetail}");
                        Username = String.Empty;
                        SecurePassword.Clear();
                        SecurePassword = new SecureString();
                        OnLoginFailed?.Invoke();
                        IsErrorLogVisible = Visibility.Visible;
                        ErrorLog = "Incorrect Username or Password.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi kết nối đến Server: {ex.Message}");
                }
            }

        });

        SendOTPCommand = new RelayCommand<object>(async (p) => {
            using(var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync($"https://localhost:7001/api/Auth/check-email?email={Email}");
                    if(response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Email);
                        var result = await response.Content.ReadFromJsonAsync<EmailCheckResponse>();
                        if(result != null)
                        {
                            CurrentState = LoginState.Verify;
                            IsErrorLogVisible = Visibility.Hidden;
                        }
                    }
                    else
                    {
                        IsErrorLogVisible = Visibility.Visible;
                        ErrorLog = "Email not registered. Please try again!";
                    }
                }
                catch
                {

                }
            }
            //CurrentState = LoginState.Verify; 
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