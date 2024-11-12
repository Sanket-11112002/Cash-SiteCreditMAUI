namespace CardGameCorner
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            MainPage = new AppShell(_serviceProvider);
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);

            // Customize the window size for desktop platforms
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI ||
                DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
            {
                window.Width = 400;
                window.Height = 600;
            }

            return window;
        }
    }
}
