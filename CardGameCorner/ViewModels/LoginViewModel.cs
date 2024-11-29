using CardGameCorner.Models;
using CardGameCorner.Services;
using CardGameCorner.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly ISecureStorage _secureStorage;
        private readonly IServiceProvider _serviceProvider;

        private string _username;
        private string _password;
        private bool _showForgotPassword;
        private string _forgotPasswordEmail;

        public LoginViewModel(IAuthService authService, ISecureStorage secureStorage, IServiceProvider serviceProvider)
        {
            _authService = authService;
            _secureStorage = secureStorage;
            _serviceProvider = serviceProvider;

            LoginCommand = new AsyncRelayCommand(LoginAsync);
            ForgotPasswordCommand = new AsyncRelayCommand(RequestPasswordResetAsync);
            ToggleForgotPasswordCommand = new Command(ToggleForgotPassword);
            ShowForgotPasswordCommand = new RelayCommand(() => ShowForgotPassword = true);
            NavigateToRegistrationCommand = new AsyncRelayCommand(NavigateToRegistrationPage);
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
            ShowForgotPassword = !ShowForgotPassword; // Toggles the visibility
        }

        //private bool CanLogin() =>
        //    !string.IsNullOrWhiteSpace(Username) &&
        //    !string.IsNullOrWhiteSpace(Password) &&
        //    !IsBusy;

        //private async Task LoginAsync()
        //{
        //    if (IsBusy) return;

        //    try
        //    {
        //        IsBusy = true;

        //        var request = new LoginRequest
        //        {
        //            Username = Username,
        //            Password = Password
        //        };

        //        if (string.IsNullOrWhiteSpace(Username))
        //        {

        //            await Application.Current.MainPage.DisplayAlert("Error", "Username cannot be empty", "OK");
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(Password))
        //        {

        //            await Application.Current.MainPage.DisplayAlert("Error", "Password cannot be empty.", "OK");
        //            return;
        //        }


        //        var response = await _authService.LoginAsync(request);
        //        await _secureStorage.SetAsync("jwt_token", response.Token);

        //        App.IsUserLoggedIn = true;
        //        await Shell.Current.GoToAsync("..");

        //        // Update navigation to use Shell.GoToAsync with the route
        //        // await (App.Current.MainPage as AppShell).ShowHomePageWithTabBarAsync();
        //        await Shell.Current.GoToAsync(nameof(MyAccountPage));
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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

                // Validate inputs (as you already have)
                if (string.IsNullOrWhiteSpace(Username))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Username cannot be empty", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Password cannot be empty.", "OK");
                    return;
                }

                var request = new LoginRequest
                {
                    Username = Username,
                    Password = Password
                };

                var response = await _authService.LoginAsync(request);
                await _secureStorage.SetAsync("jwt_token", response.Token);

                // Set logged in state
                App.IsUserLoggedIn = true;

                // Navigate to the tab bar and select My Account tab
                //await Shell.Current.GoToAsync(nameof(MyAccountPage));
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter your email address", "OK");
                return;
            }

            try
            {
                IsBusy = true;
                await _authService.RequestPasswordReset(ForgotPasswordEmail);
                await Application.Current.MainPage.DisplayAlert("Success", "Password reset link has been sent to your email", "OK");
                ShowForgotPassword = false;
                ForgotPasswordEmail = string.Empty;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
