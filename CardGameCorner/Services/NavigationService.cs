using CardGameCorner.Resources.Language;
using CardGameCorner.Views;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IAlertService _alertService;
        private readonly ISecureStorage _secureStorage;
        private GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;
        public NavigationService(IAlertService alertService, ISecureStorage secureStorage)
        {
            _alertService = alertService;
            _secureStorage = secureStorage;
        }

        public async Task NavigateToLoginAsync()
        {
           
            await Shell.Current.GoToAsync(nameof(LoginPage));

        }
    

        public async Task NavigateToHomeAsync()
        {
            await Shell.Current.GoToAsync("//GameDetailsPage");
        }

        public async Task LogoutAsync()
        {
            // Clear any stored tokens or user data
            await _secureStorage.RemoveAsync("jwt_token");
            App.IsUserLoggedIn = false;

            // Show logout confirmation
            //await _alertService.ShowAlertAsync("Logged Out", "You have been successfully logged out.");
            // Show logout confirmation using localized strings
            await _alertService.ShowAlertAsync(
                AppResources.LogoutTitle,
                AppResources.LogoutMessage
            );

            // Navigate to login page
            // await NavigateToLoginAsync();
            await Shell.Current.GoToAsync("//GameDetailsPage");
        }
    }
}
