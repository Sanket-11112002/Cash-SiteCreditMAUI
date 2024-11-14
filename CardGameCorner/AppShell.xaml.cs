using CardGameCorner.Views;

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

            GoToAsync("login");
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute($"login", typeof(LoginPage));
            Routing.RegisterRoute($"//{nameof(HomePage)}", typeof(HomePage));
            Routing.RegisterRoute("gameDetails", typeof(GameDetailsPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(MyListPage), typeof(MyListPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(MyAccountPage), typeof(MyAccountPage));
            Routing.RegisterRoute(nameof(ScanPage), typeof(ScanPage));
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
                    ContentTemplate = new DataTemplate(typeof(MyListPage)),
                    Route = "MyListPage"
                },

                //  new ShellContent
                //{
                //     Title="Search",
                //     Icon="bxsearchalt.svg",
                //    ContentTemplate = new DataTemplate(typeof(HomePage)),
                //    Route = "home"
                //},
                //   new ShellContent
                //{

                //    ContentTemplate = new DataTemplate(typeof(HomePage)),
                //    Route = "home"
                //},
                //    new ShellContent
                //{
                //    ContentTemplate = new DataTemplate(typeof(HomePage)),
                //    Route = "home"
                //},
              
            }
            };

            // Clear any existing TabBars and add the new one
            Items.Clear();
            Items.Add(homeTabBar);

            // Navigate to the Home tab
            await GoToAsync("///home");
        }

        // Navigate to GameDetailsPage without TabBar
        public async Task ShowGameDetailsPageAsync(string gameCode)
        {
            // You can also pass gameCode or other parameters here if necessary
            await GoToAsync($"gameDetails?gameCode={gameCode}");
        }
    }
}