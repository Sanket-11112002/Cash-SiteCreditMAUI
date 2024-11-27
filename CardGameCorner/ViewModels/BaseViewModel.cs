using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CardGameCorner.Services;

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