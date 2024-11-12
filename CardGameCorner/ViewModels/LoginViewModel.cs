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

            LoginCommand = new AsyncRelayCommand(LoginAsync, CanLogin);
            ForgotPasswordCommand = new AsyncRelayCommand(RequestPasswordResetAsync);
            ShowForgotPasswordCommand = new RelayCommand(() => ShowForgotPassword = true);
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

        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !IsBusy;

        private async Task LoginAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var request = new LoginRequest
                {
                    Username = Username,
                    Password = Password
                };

                var response = await _authService.LoginAsync(request);
                await _secureStorage.SetAsync("jwt_token", response.Token);

                // Update navigation to use Shell.GoToAsync with the route
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
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
    }

}
