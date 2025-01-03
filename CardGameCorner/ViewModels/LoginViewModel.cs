//using CardGameCorner.Models;
//using CardGameCorner.Resources.Language;
//using CardGameCorner.Services;
//using CardGameCorner.Views;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using Newtonsoft.Json;
//using System.ComponentModel;
//using System.Windows.Input;
//using ISecureStorage = CardGameCorner.Services.ISecureStorage;

//namespace CardGameCorner.ViewModels
//{
//    public partial class LoginViewModel : BaseViewModel
//    {
//        private readonly IAuthService _authService;
//        private readonly ISecureStorage _secureStorage;
//        private readonly IServiceProvider _serviceProvider;

//        private string _username;
//        private string _password;
//        private bool _showForgotPassword;
//        private string _forgotPasswordEmail;

//        // Add new localized properties
//        [ObservableProperty]
//        private string usernamePlaceholder;

//        [ObservableProperty]
//        private string passwordPlaceholder;

//        [ObservableProperty]
//        private string loginButtonText;

//        [ObservableProperty]
//        private string signUpButtonText;

//        [ObservableProperty]
//        private string forgotPasswordText;

//        [ObservableProperty]
//        private string forgotPasswordEmailLabel;

//        [ObservableProperty]
//        private string sendResetLinkText;

//        // Error messages
//        [ObservableProperty]
//        private string usernameEmptyErrorMessage;

//        [ObservableProperty]
//        private string userPassEmpty;

//        [ObservableProperty]
//        private string passwordEmptyErrorMessage;

//        [ObservableProperty]
//        private string emailEmptyErrorMessage;

//        [ObservableProperty]
//        private string email;

//        public LoginViewModel(IAuthService authService, ISecureStorage secureStorage, IServiceProvider serviceProvider)
//        {
//            _authService = authService;
//            _secureStorage = secureStorage;
//            _serviceProvider = serviceProvider;

//            UpdateLocalizedStrings();

//            GlobalSettingsService.Current.PropertyChanged += OnGlobalSettingsPropertyChanged;


//            LoginCommand = new AsyncRelayCommand(LoginAsync);
//            ForgotPasswordCommand = new AsyncRelayCommand(RequestPasswordResetAsync);
//            ToggleForgotPasswordCommand = new Command(ToggleForgotPassword);
//            ShowForgotPasswordCommand = new RelayCommand(() => ShowForgotPassword = true);
//            NavigateToRegistrationCommand = new AsyncRelayCommand(NavigateToRegistrationPage);

//        }


//        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (e.PropertyName == nameof(GlobalSettingsService.Current.SelectedLanguage))
//            {
//                UpdateLocalizedStrings();
//            }
//        }

//        private void UpdateLocalizedStrings()
//        {
//            MainThread.BeginInvokeOnMainThread(() =>
//            {
//                UsernamePlaceholder = AppResources.UsernamePlaceholder;
//                PasswordPlaceholder = AppResources.PasswordPlaceholder;
//                LoginButtonText = AppResources.Login;
//                SignUpButtonText = AppResources.SignUp;
//                ForgotPasswordText = AppResources.ForgotPassword;
//                ForgotPasswordEmailLabel = AppResources.ForgotPasswordEmailLabel;
//                SendResetLinkText = AppResources.SendResetLink;
//                Email = AppResources.Email;

//                UsernameEmptyErrorMessage = AppResources.UsernameEmptyError;
//                UserPassEmpty = AppResources.UserPassEmpty;
//                PasswordEmptyErrorMessage = AppResources.PasswordEmptyError;
//                EmailEmptyErrorMessage = AppResources.EmailEmptyError;

//                OnPropertyChanged(nameof(UsernamePlaceholder));
//                OnPropertyChanged(nameof(PasswordPlaceholder));
//                OnPropertyChanged(nameof(LoginButtonText));
//                OnPropertyChanged(nameof(SignUpButtonText));
//                OnPropertyChanged(nameof(ForgotPasswordText));
//                OnPropertyChanged(nameof(ForgotPasswordEmailLabel));
//                OnPropertyChanged(nameof(SendResetLinkText));
//                OnPropertyChanged(nameof(UsernameEmptyErrorMessage));
//                OnPropertyChanged(nameof(UserPassEmpty));
//                OnPropertyChanged(nameof(PasswordEmptyErrorMessage));
//                OnPropertyChanged(nameof(EmailEmptyErrorMessage));
//                OnPropertyChanged(nameof(Email));
//            });
//        }

//        public string Username
//        {
//            get => _username;
//            set
//            {
//                SetProperty(ref _username, value);
//                LoginCommand.NotifyCanExecuteChanged();
//            }
//        }

//        public string Password
//        {
//            get => _password;
//            set
//            {
//                SetProperty(ref _password, value);
//                LoginCommand.NotifyCanExecuteChanged();
//            }
//        }

//        public bool ShowForgotPassword
//        {
//            get => _showForgotPassword;
//            set => SetProperty(ref _showForgotPassword, value);
//        }

//        public string ForgotPasswordEmail
//        {
//            get => _forgotPasswordEmail;
//            set => SetProperty(ref _forgotPasswordEmail, value);
//        }

//        public IAsyncRelayCommand LoginCommand { get; }
//        public IAsyncRelayCommand ForgotPasswordCommand { get; }
//        public ICommand ShowForgotPasswordCommand { get; }
//        public IAsyncRelayCommand NavigateToRegistrationCommand { get; }
//        public Command ToggleForgotPasswordCommand { get; }

//        private void ToggleForgotPassword()
//        {
//            ShowForgotPassword = !ShowForgotPassword;
//        }

//        private async Task LoginAsync()
//        {
//            if (IsBusy) return;

//            try
//            {
//                IsBusy = true;

//                if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Password))
//                {
//                    await Application.Current.MainPage.DisplayAlert(
//                        AppResources.ErrorTitle,
//                        UserPassEmpty,
//                        AppResources.OK);
//                    return;
//                }

//                // Validate inputs
//                if (string.IsNullOrWhiteSpace(Username))
//                {
//                    await Application.Current.MainPage.DisplayAlert(
//                     AppResources.ErrorTitle,
//                     UsernameEmptyErrorMessage,
//                     AppResources.OK);
//                    return;
//                }

//                if (string.IsNullOrWhiteSpace(Password))
//                {
//                    await Application.Current.MainPage.DisplayAlert(
//                   AppResources.ErrorTitle,
//                   PasswordEmptyErrorMessage,
//                   AppResources.OK);
//                    return;
//                }

//                var request = new LoginRequest
//                {
//                    Username = Username,
//                    Password = Password
//                };

//                var response = await _authService.LoginAsync(request);

//                await _secureStorage.SetAsync("jwt_token", response.Token);

//                App.IsUserLoggedIn = true;

//                // Clear sensitive data
//                Username = string.Empty;
//                Password = string.Empty;


//                var loginaccount = await SecureStorage.GetAsync("Login");
//                switch (loginaccount)
//                {
//                    case "loginmethod":
//                        // Assuming you have a method to navigate to the add to list page or trigger the action
//                        await Shell.Current.GoToAsync("..");
//                        break;
//                    default:
//                        // Default navigation (e.g., to home or previous page)
//                        await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack

//                        await Shell.Current.GoToAsync("//MyListPage");
//                        break;
//                }

//                await SecureStorage.SetAsync("Login", "");
//                //await Shell.Current.GoToAsync("..");

//                //await Shell.Current.GoToAsync($"{nameof(MyListPage)}", true);
//            }
//            catch (Exception ex)
//            {
//                await Application.Current.MainPage.DisplayAlert(
//              AppResources.ErrorTitle,
//              ex.Message,
//              AppResources.OK);
//            }
//            finally
//            {
//                IsBusy = false;
//            }
//        }
//        private async Task RequestPasswordResetAsync()
//        {
//            if (string.IsNullOrWhiteSpace(ForgotPasswordEmail))
//            {
//                await Application.Current.MainPage.DisplayAlert(
//                AppResources.ErrorTitle,
//                EmailEmptyErrorMessage,
//                AppResources.OK);
//                return;
//            }

//            try
//            {
//                IsBusy = true;
//                await _authService.RequestPasswordReset(ForgotPasswordEmail);
//                await Application.Current.MainPage.DisplayAlert(
//                    AppResources.SuccessTitle,
//                    AppResources.PasswordResetSuccessMessage,
//                    AppResources.OK);
//                ShowForgotPassword = false;
//                ForgotPasswordEmail = string.Empty;
//            }
//            catch (Exception ex)
//            {
//                await Application.Current.MainPage.DisplayAlert(
//                AppResources.ErrorTitle,
//                ex.Message,
//                AppResources.OK);
//            }
//            finally
//            {
//                IsBusy = false;
//            }
//        }
//        private async Task NavigateToRegistrationPage()
//        {
//            await Shell.Current.GoToAsync(nameof(RegistrationPage));
//        }
//    }
//}

using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly ISecureStorage _secureStorage;
        private readonly IServiceProvider _serviceProvider;

        private string _username;
        private string _password;
        private bool _showForgotPassword;
        private string _forgotPasswordEmail;

        // Add new localized properties
        [ObservableProperty]
        private string usernamePlaceholder;

        [ObservableProperty]
        private string passwordPlaceholder;

        [ObservableProperty]
        private string loginButtonText;

        [ObservableProperty]
        private string signUpButtonText;

        [ObservableProperty]
        private string forgotPasswordText;

        [ObservableProperty]
        private string forgotPasswordEmailLabel;

        [ObservableProperty]
        private string sendResetLinkText;

        // Error messages
        [ObservableProperty]
        private string usernameEmptyErrorMessage;

        [ObservableProperty]
        private string userPassEmpty;

        [ObservableProperty]
        private string passwordEmptyErrorMessage;

        [ObservableProperty]
        private string emailEmptyErrorMessage;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string loginFailed;

        [ObservableProperty]
        private string emailFailed;


        public LoginViewModel(IAuthService authService, ISecureStorage secureStorage, IServiceProvider serviceProvider)
        {
            _authService = authService;
            _secureStorage = secureStorage;
            _serviceProvider = serviceProvider;

            UpdateLocalizedStrings();

            GlobalSettingsService.Current.PropertyChanged += OnGlobalSettingsPropertyChanged;


            LoginCommand = new AsyncRelayCommand(LoginAsync);
            ForgotPasswordCommand = new AsyncRelayCommand(RequestPasswordResetAsync);
            ToggleForgotPasswordCommand = new Command(ToggleForgotPassword);
            ShowForgotPasswordCommand = new RelayCommand(() => ShowForgotPassword = true);
            NavigateToRegistrationCommand = new AsyncRelayCommand(NavigateToRegistrationPage);

        }


        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettingsService.Current.SelectedLanguage))
            {
                UpdateLocalizedStrings();
            }
        }

        private void UpdateLocalizedStrings()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UsernamePlaceholder = AppResources.UsernamePlaceholder;
                PasswordPlaceholder = AppResources.PasswordPlaceholder;
                LoginButtonText = AppResources.Login;
                SignUpButtonText = AppResources.SignUp;
                ForgotPasswordText = AppResources.ForgotPassword;
                ForgotPasswordEmailLabel = AppResources.ForgotPasswordEmailLabel;
                SendResetLinkText = AppResources.SendResetLink;
                Email = AppResources.Email;
                LoginFailed = AppResources.Login_failed;
                EmailFailed = AppResources.EmailFailed;

                UsernameEmptyErrorMessage = AppResources.UsernameEmptyError;
                UserPassEmpty = AppResources.UserPassEmpty;
                PasswordEmptyErrorMessage = AppResources.PasswordEmptyError;
                EmailEmptyErrorMessage = AppResources.EmailEmptyError;

                OnPropertyChanged(nameof(EmailFailed));
                OnPropertyChanged(nameof(UsernamePlaceholder));
                OnPropertyChanged(nameof(PasswordPlaceholder));
                OnPropertyChanged(nameof(LoginButtonText));
                OnPropertyChanged(nameof(SignUpButtonText));
                OnPropertyChanged(nameof(ForgotPasswordText));
                OnPropertyChanged(nameof(ForgotPasswordEmailLabel));
                OnPropertyChanged(nameof(SendResetLinkText));
                OnPropertyChanged(nameof(UsernameEmptyErrorMessage));
                OnPropertyChanged(nameof(UserPassEmpty));
                OnPropertyChanged(nameof(PasswordEmptyErrorMessage));
                OnPropertyChanged(nameof(EmailEmptyErrorMessage));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(LoginFailed));
            });
        }

        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);
                LoginCommand.NotifyCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                LoginCommand.NotifyCanExecuteChanged();
            }
        }

        public bool ShowForgotPassword
        {
            get => _showForgotPassword;
            set => SetProperty(ref _showForgotPassword, value);
        }

        public string ForgotPasswordEmail
        {
            get => _forgotPasswordEmail;
            set => SetProperty(ref _forgotPasswordEmail, value);
        }

        public IAsyncRelayCommand LoginCommand { get; }
        public IAsyncRelayCommand ForgotPasswordCommand { get; }
        public ICommand ShowForgotPasswordCommand { get; }
        public IAsyncRelayCommand NavigateToRegistrationCommand { get; }
        public Command ToggleForgotPasswordCommand { get; }

        private void ToggleForgotPassword()
        {
            ShowForgotPassword = !ShowForgotPassword;
        }

        //private async Task LoginAsync()
        //{
        //    if (IsBusy) return;

        //    try
        //    {
        //        IsBusy = true;

        //        if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Password))
        //        {
        //            await Application.Current.MainPage.DisplayAlert(
        //                AppResources.ErrorTitle,
        //                UserPassEmpty,
        //                AppResources.OK);
        //            return;
        //        }

        //        // Validate inputs
        //        if (string.IsNullOrWhiteSpace(Username))
        //        {
        //            await Application.Current.MainPage.DisplayAlert(
        //             AppResources.ErrorTitle,
        //             UsernameEmptyErrorMessage,
        //             AppResources.OK);
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(Password))
        //        {
        //            await Application.Current.MainPage.DisplayAlert(
        //           AppResources.ErrorTitle,
        //           PasswordEmptyErrorMessage,
        //           AppResources.OK);
        //            return;
        //        }

        //        var request = new LoginRequest
        //        {
        //            Username = Username,
        //            Password = Password
        //        };

        //        var response = await _authService.LoginAsync(request);

        //        await _secureStorage.SetAsync("jwt_token", response.Token);

        //        App.IsUserLoggedIn = true;

        //        // Clear sensitive data
        //        Username = string.Empty;
        //        Password = string.Empty;


        //        var loginaccount = await SecureStorage.GetAsync("Login");
        //        switch (loginaccount)
        //        {
        //            case "loginmethod":
        //                // Assuming you have a method to navigate to the add to list page or trigger the action
        //                await Shell.Current.GoToAsync("..");
        //                break;
        //            default:
        //                // Default navigation (e.g., to home or previous page)
        //                await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack

        //                await Shell.Current.GoToAsync("//MyListPage");
        //                break;
        //        }

        //        await SecureStorage.SetAsync("Login", "");
        //        //await Shell.Current.GoToAsync("..");

        //        //await Shell.Current.GoToAsync($"{nameof(MyListPage)}", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert(
        //      AppResources.ErrorTitle,
        //      ex.Message,
        //      AppResources.OK);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        private async Task LoginAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Password))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        AppResources.ErrorTitle,
                        UserPassEmpty,
                        AppResources.OK);
                    return;
                }

                // Validate inputs
                if (string.IsNullOrWhiteSpace(Username))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        AppResources.ErrorTitle,
                        UsernameEmptyErrorMessage,
                        AppResources.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        AppResources.ErrorTitle,
                        PasswordEmptyErrorMessage,
                        AppResources.OK);
                    return;
                }

                var request = new LoginRequest
                {
                    Username = Username,
                    Password = Password
                };

                try
                {
                    // Logging the username being used in login request
                    Trace.WriteLine("Sending login request with username: " + Username);

                    var response = await _authService.LoginAsync(request);

                    // Log response token (if present)
                    Trace.WriteLine($"Login response: {response?.Token}");



                    if (response == null || string.IsNullOrEmpty(response.Token))
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            AppResources.ErrorTitle,
                           LoginFailed,
                            AppResources.OK);
                    }

                    await _secureStorage.SetAsync("jwt_token", response.Token);

                    App.IsUserLoggedIn = true;

                    // Clear sensitive data
                    Username = string.Empty;
                    Password = string.Empty;

                    var loginaccount = await SecureStorage.GetAsync("Login");
                    switch (loginaccount)
                    {
                        case "loginmethod":
                            await Shell.Current.GoToAsync("..");
                            Trace.WriteLine("Sending login request with username: ");
                            break;
                        default:
                            await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack
                           // await Shell.Current.GoToAsync("//GameDetailsPage");
                            break;
                    }

                    await SecureStorage.SetAsync("Login", "");
                }
                catch (Exception ex)
                {
                    // Log the exception with Trace
                    Trace.WriteLine($"Exception during login: {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert(
                        AppResources.ErrorTitle,
                        LoginFailed,
                        AppResources.OK);
                }
                finally
                {
                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                // Catch and log general issues outside the inner try
                Trace.WriteLine($"General error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert(
                    AppResources.ErrorTitle,
                    "An unexpected error occurred. Please try again.",
                    AppResources.OK);
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task RequestPasswordResetAsync()
        {
            if (string.IsNullOrWhiteSpace(ForgotPasswordEmail))
            {
                await Application.Current.MainPage.DisplayAlert(
                AppResources.ErrorTitle,
                EmailEmptyErrorMessage,
                AppResources.OK);
                return;
            }

            try
            {
                IsBusy = true;
                await _authService.RequestPasswordReset(ForgotPasswordEmail);
                await Application.Current.MainPage.DisplayAlert(
                    AppResources.SuccessTitle,
                    AppResources.PasswordResetSuccessMessage,
                    AppResources.OK);
                ShowForgotPassword = false;
                ForgotPasswordEmail = string.Empty;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                AppResources.ErrorTitle,
                EmailFailed,
                AppResources.OK);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task NavigateToRegistrationPage()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }
    }
}
