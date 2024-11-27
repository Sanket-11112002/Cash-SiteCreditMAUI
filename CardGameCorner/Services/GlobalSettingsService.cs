using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CardGameCorner.Services
{
    public class GlobalSettingsService : INotifyPropertyChanged
    {
        // Singleton implementation
        private static GlobalSettingsService _instance;
        public static GlobalSettingsService Current =>
            _instance ??= new GlobalSettingsService();

        private GlobalSettingsService()
        {
            // Initialize with default values
            SelectedLanguage = "English";
        }

        private string _selectedLanguage = "English";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value))
                {
                    // Trigger any language change logic
                    OnLanguageChanged();
                }
            }
        }

        private string _selectedGame;
        public string SelectedGame
        {
            get => _selectedGame;
            set
            {
                if (SetProperty(ref _selectedGame, value))
                {
                    // Trigger any game change logic
                    OnGameChanged();
                }
            }
        }

        // Method to handle language change
        //private void OnLanguageChanged()
        //{
        //    // Implement language change logic
        //    // For example, update UI or load language resources
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedLanguage)));
        //}

        private void OnLanguageChanged()
        {
            try
            {
                var culture = new CultureInfo(SelectedLanguage == "English" ? "en" : "it");
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedLanguage)));
            }
            catch (CultureNotFoundException ex)
            {
                Application.Current.MainPage.DisplayAlert("Error",
                    $"The selected language is not supported: {ex.Message}", "OK");
            }
        }


        // Method to handle game change
        private void OnGameChanged()
        {
            // Implement game change logic
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedGame)));
        }

        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Methods for changing settings
        public async Task ChangeLanguageAsync()
        {
            try
            {
                string language = await Application.Current.MainPage.DisplayActionSheet(
                    "Choose a Language",
                    "Cancel",
                    null,
                    "English",
                    "Italian");

                if (!string.IsNullOrEmpty(language) && language != "Cancel")
                {
                    SelectedLanguage = language;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"An error occurred while selecting language: {ex.Message}", "OK");
            }
        }

        public async Task ChangeGameAsync()
        {
            try
            {
                string game = await Application.Current.MainPage.DisplayActionSheet(
                    "Choose a Game",
                    "Cancel",
                    null,
                    "Pokémon",
                    "One Piece",
                    "Magic",
                    "Yu-Gi-Oh!");

                if (!string.IsNullOrEmpty(game) && game != "Cancel")
                {
                    SelectedGame = game;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"An error occurred while selecting game: {ex.Message}", "OK");
            }
        }
    }
}