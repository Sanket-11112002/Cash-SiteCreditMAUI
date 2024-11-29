using CardGameCorner.Services;
using CardGameCorner.Views;
using System.Diagnostics;

namespace CardGameCorner
{
    public partial class AppShell : Shell
    {
        private readonly IServiceProvider _serviceProvider;

        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            RegisterRoutes();

            // Delay navigation until after the Shell has been initialized
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var selectedGameCode = Preferences.Get("SelectedGame", string.Empty);

                // Navigate based on whether a game is selected or not
                if (string.IsNullOrEmpty(selectedGameCode))
                {
                    GoToAsync("//HomePage"); // Navigate to HomePage if no game is selected
                }
                else
                {
                    NavigateToGameDetails(selectedGameCode); // Navigate to GameDetailsPage with the selected game code
                }
            });
        }


        private void RegisterRoutes()
        {
            Routing.RegisterRoute($"login", typeof(LoginPage));
            Routing.RegisterRoute("//HomePage", typeof(HomePage));  // Register HomePage route
            //Routing.RegisterRoute("GameDetailsPage", typeof(GameDetailsPage));
            //Routing.RegisterRoute(nameof(GameDetailsPage), typeof(GameDetailsPage));
            Routing.RegisterRoute("GameDetailsPage", typeof(CardGameCorner.Views.GameDetailsPage));

            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(MyListPage), typeof(MyListPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(MyAccountPage), typeof(MyAccountPage));
            Routing.RegisterRoute(nameof(ScanPage), typeof(ScanPage));
            Routing.RegisterRoute(nameof(CardDetailPage), typeof(CardDetailPage));
            Routing.RegisterRoute(nameof(CardComparisonPage), typeof(CardComparisonPage));
        }

        private async void MyAccountTab_Navigated(object sender, ShellNavigatedEventArgs e)
        {
            // Check if user is logged in before accessing MyAccountTab
            if (!App.IsUserLoggedIn)
            {
                await GoToAsync("//login");
            }
        }

        public async Task NavigateToGameDetails(string gameCode)
        {
            try
            {
                if (string.IsNullOrEmpty(gameCode))
                {
                    return;
                }

                await GameNavigationService.ShowGameDetailsPageAsync(gameCode);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await DisplayAlert("Error", "Unable to open game details. Please try again.", "OK");
                });
            }
        }
    }
}