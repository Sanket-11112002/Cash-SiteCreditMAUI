using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CardGameCorner.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CardGameCorner.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        protected GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        // Commands for language and game selection
        public IAsyncRelayCommand SelectLanguageCommand { get; }
        public IAsyncRelayCommand SelectGameCommand { get; }

        protected BaseViewModel()
        {
            SelectLanguageCommand = new AsyncRelayCommand(ChangeLanguage);
            SelectGameCommand = new AsyncRelayCommand(ChangeGame);

            // Initialize with current language
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;
        }

        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
            {
                // Update localized strings when language changes
                UpdateLocalizedStrings();
            }
        }

        protected virtual void UpdateLocalizedStrings()
        {
            // To be overridden by derived view models
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async Task ChangeLanguage()
        {
            await GlobalSettings.ChangeLanguageAsync();
        }

        private async Task ChangeGame()
        {
            await GlobalSettings.ChangeGameAsync();
        }
    }
}