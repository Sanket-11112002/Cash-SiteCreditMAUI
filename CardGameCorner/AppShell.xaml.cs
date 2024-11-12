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
            GoToAsync($"//{nameof(LoginPage)}");
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute($"//{nameof(LoginPage)}", typeof(LoginPage));
            Routing.RegisterRoute($"//{nameof(HomePage)}", typeof(HomePage));
        }
    }
}