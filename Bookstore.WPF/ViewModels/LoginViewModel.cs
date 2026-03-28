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

    private SecureString _newPassword = new SecureString();
    public SecureString NewPassword
    {
        private get => _newPassword;
        set
        {
            _newPassword = value;
            OnPropertyChanged();
        }
    }

    private SecureString _confirmPassword = new SecureString();
    public SecureString ConfirmPassword
    {
        private get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged();
        }
    }

    private string _email = String.Empty;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    private string _otp;
    public string OTP
    { 
        get { return _otp; }
        set
        {
            _otp = value;
            OnPropertyChanged();
        }
    }

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
            MessageBox.Show($"username: {requestData.username}, pw: {plainText}, pwHASH: {requestData.password}");
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
                    var response = await client.PostAsync($"https://localhost:7001/api/Auth/forgot-password?email={Email}", null);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Gửi otp rồi đó, Check mail liền đi má!");
                        CurrentState = LoginState.Verify;
                        IsErrorLogVisible = Visibility.Hidden;
                    }
                    else
                    {
                        var errorDetail = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Lỗi Server: {response.StatusCode} - {errorDetail}");

                        IsErrorLogVisible = Visibility.Visible;
                        ErrorLog = "Email not registered in Sahara System!";
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

        VerifyOTPCommand = new RelayCommand<object>(async (p) => {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsJsonAsync("https://localhost:7001/api/Auth/verify-otp",
                        new { Email = this.Email, Otp = this.OTP });

                    if (response.IsSuccessStatusCode)
                    {
                        CurrentState = LoginState.Reset;
                        IsErrorLogVisible = Visibility.Hidden;
                    }
                    else
                    {
                        var realError = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Server từ chối (Mã {response.StatusCode}):\n{realError}");

                        IsErrorLogVisible = Visibility.Visible;
                        ErrorLog = "OTP is invalid or expired!";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi kết nối: {ex.Message}");
                }
            }
        });

        ResetPasswordCommand = new RelayCommand<object>(async (p) => {
            string newPw = new NetworkCredential("", NewPassword).Password;
            string ConfirmPw = new NetworkCredential("", ConfirmPassword).Password;
            MessageBox.Show($"{newPw} và {ConfirmPw}");
            if (newPw != ConfirmPw)
            {
                ErrorLog = "Passwords do not match!";
                IsErrorLogVisible = Visibility.Visible;
                return;
            }

            using (var client = new HttpClient())
            {
                try
                {
                    string pwHash = HashHelper.SHA256_Encode(HashHelper.Base64_Encode(newPw));
                    var response = await client.PostAsJsonAsync("https://localhost:7001/api/Auth/reset-password", 
                        new { Email = this.Email, NewPassword = pwHash });

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Mật khẩu đã đổi thành công!");
                        NewPassword.Clear();
                        NewPassword = new SecureString();
                        ConfirmPassword.Clear();
                        ConfirmPassword = new SecureString();
                        OTP = string.Empty;
                        Username = string.Empty;
                        SecurePassword.Clear();
                        SecurePassword = new SecureString();
                        Email = string.Empty;
                        IsErrorLogVisible = Visibility.Hidden;
                        CurrentState = LoginState.Login;
                    }
                    else
                    {
                        ErrorLog = "Reset failed. Try again!";
                        IsErrorLogVisible = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            }
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

    private async Task SendOTPAsync()
    {   
        using var client = new HttpClient();
        var response = await client.PostAsJsonAsync("https://localhost:7001/api/Auth//send-otp", Email);

        if (response.IsSuccessStatusCode)
        {
            MessageBox.Show("Mã OTP đã được gửi vào Email!");
            CurrentState = LoginState.Verify; 
        }
        else
        {
            MessageBox.Show("Gửi mã thất bại, kiểm tra lại Email.");
        }
    }
}
public enum LoginState 
{ 
    Login,
    Forgot,
    Verify,
    Reset 
}