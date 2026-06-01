using Bookstore.Share.DTO;
using Bookstore.WPF.Properties;
using Bookstore.WPF.Services;
using Bookstore.WPF.Utils;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.WPF.ViewModels
{
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
        private Visibility _isErrorLogVisible = Visibility.Hidden;
        public Visibility IsErrorLogVisible
        {
            get => _isErrorLogVisible;
            set
            {
                _isErrorLogVisible = value;
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

        public string Title { get; private set; } = "Login";
        public string SubTitle { get; private set; } = "Welcome back! Have a nice day :3";

        // --- Data Properties ---
        private string _username = String.Empty;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password = String.Empty;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private SecureString _securePassword = new SecureString();
        public SecureString SecurePassword
        {
            private get => _securePassword;
            set { _securePassword = value; OnPropertyChanged(); }
        }

        private SecureString _newPassword = new SecureString();
        public SecureString NewPassword
        {
            private get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); }
        }

        private SecureString _confirmPassword = new SecureString();
        public SecureString ConfirmPassword
        {
            private get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        private string _email = String.Empty;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _otp = String.Empty;
        public string OTP
        {
            get => _otp;
            set { _otp = value; OnPropertyChanged(); }
        }

        private bool _isRememberMe;
        public bool IsRememberMe
        {
            get => _isRememberMe;
            set { _isRememberMe = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand SwitchStateCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand SendOTPCommand { get; set; }
        public ICommand VerifyOTPCommand { get; set; }
        public ICommand ResetPasswordCommand { get; set; }
        public ICommand ResendOTPCommand { get; set; }
        #endregion

        public LoginViewModel()
        {
            LoadRememberedAccount();

            SwitchStateCommand = new RelayCommand<string>((p) =>
            {
                if (Enum.TryParse(p, out LoginState newState))
                    CurrentState = newState;
                IsErrorLogVisible = Visibility.Hidden;
            });

            LoginCommand = new RelayCommand<object>(async (p) =>
            {
                string plainText = new NetworkCredential("", SecurePassword).Password;

                if (string.IsNullOrWhiteSpace(this.Username) || string.IsNullOrWhiteSpace(plainText))
                {
                    ErrorLog = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!";
                    IsErrorLogVisible = Visibility.Visible;
                    return;
                }

                if (plainText.Length < 6)
                {
                    ErrorLog = "Mật khẩu phải có từ 6 ký tự trở lên!";
                    IsErrorLogVisible = Visibility.Visible;
                    return;
                }

                var requestData = new LoginRequest
                {
                    Username = this.Username,
                    Password = plainText
                };

                try
                {
                    //MessageBox.Show($"{requestData.Password}, {requestData.Username}");
                    /////
                    var responseUser = await ApiClient.PostAsync<LoginRequest, LoginResponse>("api/Auth/login", requestData);

                    if (responseUser != null)
                    {
                        IsErrorLogVisible = Visibility.Hidden;
                        AppState.CurrentUser = responseUser.User;
                        AppState.CurrentPermissions = responseUser.User.PermissionList;

                        if (IsRememberMe)
                        {
                            Settings.Default.RememberMe = true;
                            Settings.Default.Username = Username;
                            Settings.Default.Password = plainText;
                        }
                        else
                        {
                            Settings.Default.RememberMe = false;
                            Settings.Default.Username = string.Empty;
                            Settings.Default.Password = string.Empty;
                        }
                        Settings.Default.Save();

                        _windowService.ShowWindow<MainViewModel>();
                        _windowService.CloseWindow<LoginViewModel>();
                    }
                    else
                    {
                        // Đăng nhập thất bại
                        Username = String.Empty;
                        SecurePassword.Clear();
                        SecurePassword = new SecureString();

                        OnLoginFailed?.Invoke(); 

                        IsErrorLogVisible = Visibility.Visible;
                        ErrorLog = "Tài khoản hoặc mật khẩu không chính xác!";
                    }
                }
                catch (Exception)
                {
                    IsErrorLogVisible = Visibility.Visible;
                    ErrorLog = "Tài khoản hoặc mật khẩu không chính xác!";
                }
            });

            SendOTPCommand = new RelayCommand<object>(async (p) =>
            {
                try
                {
                    bool isSuccess = await ApiClient.PostNoBodyAsync($"api/Auth/forgot-pw?email={Email}");

                    if (isSuccess)
                    {
                        MessageBox.Show("Mã xác nhận đã được gửi, hãy kiểm tra hòm thư của bạn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        CurrentState = LoginState.Verify;
                        IsErrorLogVisible = Visibility.Hidden;
                    }
                    else
                    {
                        IsErrorLogVisible = Visibility.Visible;
                        ErrorLog = "Email không tồn tại trong hệ thống Sahara!";
                    }
                }
                catch (Exception)
                {
                    IsErrorLogVisible = Visibility.Visible;
                    ErrorLog = "Không thể kết nối đến máy chủ!";
                }
            });

            ResendOTPCommand = new RelayCommand<object>(async (p) =>
            {
                try
                {
                    bool isSuccess = await ApiClient.PostNoBodyAsync($"api/Auth/forgot-pw?email={Email}");
                    if (isSuccess)
                        MessageBox.Show("Đã gửi lại OTP về email của bạn.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                    {
                        IsErrorLogVisible = Visibility.Visible;
                        ErrorLog = "Không thể gửi lại mã, vui lòng kiểm tra email!";
                    }
                }
                catch (Exception)
                {
                    IsErrorLogVisible = Visibility.Visible;
                    ErrorLog = "Lỗi kết nối khi gửi lại mã!";
                }
            });

            VerifyOTPCommand = new RelayCommand<object>(async (p) =>
            {
                try
                {
                    bool isSuccess = await ApiClient.PostAndCheckSuccessAsync("api/Auth/verify-otp",
                        new { Email = this.Email, Otp = this.OTP });

                    if (isSuccess)
                    {
                        CurrentState = LoginState.Reset;
                        IsErrorLogVisible = Visibility.Hidden;
                    }
                    else
                    {
                        IsErrorLogVisible = Visibility.Visible;
                        ErrorLog = "Mã OTP không hợp lệ hoặc đã hết hạn!";
                    }
                }
                catch (Exception ex)
                {
                    IsErrorLogVisible = Visibility.Visible;
                    ErrorLog = $"Lỗi kết nối: {ex.Message}";
                }
            });

            ResetPasswordCommand = new RelayCommand<object>(async (p) =>
            {
                string newPw = new NetworkCredential("", NewPassword).Password;
                string ConfirmPw = new NetworkCredential("", ConfirmPassword).Password;

                if (newPw.Length < 6)
                {
                    ErrorLog = "Mật khẩu phải có từ 6 ký tự trở lên!";
                    IsErrorLogVisible = Visibility.Visible;
                    return;
                }

                if (newPw != ConfirmPw)
                {
                    ErrorLog = "Mật khẩu xác nhận không khớp!";
                    IsErrorLogVisible = Visibility.Visible;
                    return;
                }

                try
                {
                    bool isSuccess = await ApiClient.PostAndCheckSuccessAsync("api/Auth/reset-password",
                        new { Email = this.Email, NewPassword = newPw });

                    if (isSuccess)
                    {
                        MessageBox.Show("Đổi mật khẩu thành công! Vui lòng đăng nhập lại.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Xóa sạch dữ liệu cũ
                        NewPassword.Clear(); NewPassword = new SecureString();
                        ConfirmPassword.Clear(); ConfirmPassword = new SecureString();
                        OTP = string.Empty;
                        Username = string.Empty;
                        SecurePassword.Clear(); SecurePassword = new SecureString();
                        Email = string.Empty;

                        IsErrorLogVisible = Visibility.Hidden;
                        CurrentState = LoginState.Login;
                    }
                    else
                    {
                        ErrorLog = "Đổi mật khẩu thất bại. Vui lòng thử lại!";
                        IsErrorLogVisible = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    IsErrorLogVisible = Visibility.Visible;
                    ErrorLog = $"Lỗi hệ thống: {ex.Message}";
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

        private void LoadRememberedAccount()
        {
            if (Settings.Default == null) return;

            IsRememberMe = Settings.Default.RememberMe;
            if (IsRememberMe)
            {
                Username = Settings.Default.Username;
                Password = Settings.Default.Password;
                if (!string.IsNullOrEmpty(Settings.Default.Password))
                {
                    var secureString = new SecureString();
                    foreach (char c in Settings.Default.Password) secureString.AppendChar(c);
                    SecurePassword = secureString;
                }
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
}