using CardGameCorner.Views;
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
            await _secureStorage.SetAsync("jwt_token", string.Empty);
            App.IsUserLoggedIn = false;

            // Show logout confirmation
            await _alertService.ShowAlertAsync("Logged Out", "You have been successfully logged out.");

            // Navigate to login page
            // await NavigateToLoginAsync();
            await Shell.Current.GoToAsync("//GameDetailsPage");
        }
    }
}
