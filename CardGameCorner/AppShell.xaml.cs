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

            // Set initial page
            //GoToAsync($"//{nameof(LoginPage)}");

            GoToAsync("//HomePage");
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute($"login", typeof(LoginPage));
            Routing.RegisterRoute($"//{nameof(HomePage)}", typeof(HomePage));
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

        // Navigate to HomePage with TabBar after login
        public async Task ShowHomePageWithTabBarAsync()
        {
            var homeTabBar = new TabBar
            {
                Items =
            {
                    new ShellContent
                    {
                        Title="Home",
                         Icon="bxhomealt",
                        ContentTemplate = new DataTemplate(typeof(HomePage)),
                        Route = "home"
                    },
                    new ShellContent
                    {
                         Title="Search",
                         Icon="bxsearchalt.svg",
                        ContentTemplate = new DataTemplate(typeof(SearchPage)),
                        Route = "SearchPage"
                    },
                    new ShellContent
                    {
                         Title="Scan",
                         Icon="bxqrscan.svg",
                        ContentTemplate = new DataTemplate(typeof(ScanPage)),
                        Route = "ScanPage"
                    },
                    new ShellContent
                    {
                         Title="My Account",
                         Icon="bxuser.svg",
                        ContentTemplate = new DataTemplate(typeof(MyAccountPage)),
                        Route = "MyAccountPage"
                    },
                    new ShellContent
                    {
                         Title="My List",
                         Icon="bxlistul.svg",
                        ContentTemplate = new DataTemplate(typeof(CardComparisonPage)),
                        Route = "CardComparisonPage"
                    },
                    

                }
            };

            // Clear any existing TabBars and add the new one
            Items.Clear();
            Items.Add(homeTabBar);

            // Navigate to the Home tab
            await GoToAsync("///home");
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
