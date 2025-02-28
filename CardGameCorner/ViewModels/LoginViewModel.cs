using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using SocialLoginWithMaui.Messages;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
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

            WeakReferenceMessenger.Default.Register<ProtocolMessage>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });

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


       
        // login with social media
            private readonly Dictionary<string, string> authUrls = new Dictionary<string, string>
            {
                    { "Google", "https://api.magiccorner.it/api/auth/mobile/Google" },
     

                    { "Facebook", "https://api.magiccorner.it/api/auth/mobile/Facebook" }
            };

            [ObservableProperty]
            public string? authToken;

            [ObservableProperty]
            public string? emails;  // Changed from userEmail to emails
        [RelayCommand]
        private async Task OnAuthenticate(string scheme)
        {
            try
            {
                AuthToken = string.Empty;
                Email = string.Empty;

                if (!authUrls.TryGetValue(scheme, out string? authUrl))
                {
                    await App.Current.MainPage.DisplayAlert("Error", $"Authentication URL not configured for {scheme}", "OK");
                    return;
                }

                //// Google logout (to clear previous session)
                //if (scheme == "Google")
                //{
                //    await Browser.OpenAsync("https://accounts.google.com/Logout", BrowserLaunchMode.SystemPreferred);
                //    await Task.Delay(1000); // Wait to ensure logout is processed
                //}

                // Web authentication flow
                var authResult = await WebAuthenticator.Default.AuthenticateAsync(
                    new WebAuthenticatorOptions()
                    {
                        Url = new Uri(authUrl),
                        CallbackUrl = new Uri("mcbuylist://"),
                        PrefersEphemeralWebBrowserSession = true
                    });

                if (authResult != null)
                {
                    OnMessageReceived(authResult.CallbackUri.OriginalString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Alert", $"Failed: {ex.Message}", "OK");
            }
        }


        private async void OnMessageReceived(string value)
        {
            try
            {
                Console.WriteLine($"New message received: {value}");

                Uri uri = new Uri(value);
                string query = uri.Fragment.TrimStart('#');
                NameValueCollection queryParameters = HttpUtility.ParseQueryString(query);

                string accessToken = queryParameters["access_token"] ?? string.Empty;
                string emailValue = queryParameters["email"] ?? string.Empty;

                string decodedEmail = HttpUtility.UrlDecode(emailValue);

                Emails = decodedEmail;
                AuthToken = accessToken;

                // Store access token securely
                //await SecureStorage.Default.SetAsync("jwt_token", accessToken);
                //await SecureStorage.Default.SetAsync("user_email", decodedEmail);
                await _secureStorage.SetAsync("jwt_token", accessToken);

                // Proceed with post-login process
                await PostLoginProcedure();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing callback: {ex.Message}");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Error", $"Failed to process login: {ex.Message}", "OK");
                });
            }
        }


        private async Task PostLoginProcedure()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                App.IsUserLoggedIn = true;

                // Navigate based on login state
                var loginAccount = await SecureStorage.GetAsync("Login");
                switch (loginAccount)
                {
                    case "loginmethod":
                        await Shell.Current.GoToAsync("..");
                        break;
                    default:
                        await Shell.Current.Navigation.PopToRootAsync();
                        break;
                }

                await SecureStorage.SetAsync("Login", "");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Post-login error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "An unexpected error occurred. Please try again.",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
