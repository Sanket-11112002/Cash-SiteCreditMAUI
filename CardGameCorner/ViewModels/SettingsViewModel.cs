using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly IGameService _gameService;
        private readonly ISecureStorage _secureStorage;

        [ObservableProperty]
        private ObservableCollection<Game> games;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private Game selectedGame;

        [ObservableProperty]
        private string errorMessage;

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        [ObservableProperty]
        private string language;

        [ObservableProperty]
        private string game;

        [ObservableProperty]
        private string exit;

        [ObservableProperty]
        private string settings;

        [ObservableProperty]
        private string logout;

        public SettingsViewModel(IGameService gameService, ISecureStorage secureStorage)
        {
            // Initialize with current language
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

            _gameService = gameService;
            _secureStorage = secureStorage;
            Games = new ObservableCollection<Game>();
            // Load games when the ViewModel is created
            LoadGamesAsync().ConfigureAwait(false);
        }

        // New method to handle property changes
        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
            {
                // Update localized strings when language changes
                UpdateLocalizedStrings();
            }
        }

        private void UpdateLocalizedStrings()
        {
            // Ensure these are called on the main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Language = AppResources.Language;
                Game = AppResources.Game;
                Exit = AppResources.Exit;
                Logout = AppResources.Logout;
                Settings = AppResources.Settings;

                // Trigger property changed events to update UI
                OnPropertyChanged(nameof(Language));
                OnPropertyChanged(nameof(Game));
                OnPropertyChanged(nameof(Exit));
                OnPropertyChanged(nameof(Logout));
                OnPropertyChanged(nameof(Settings));
            });
        }

        [RelayCommand]
        public async Task LoadGamesAsync()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                ErrorMessage = string.Empty;

                var gamesList = await _gameService.GetGamesAsync();
                Games.Clear();
                foreach (var game in gamesList)
                {
                    Games.Add(game);
                }
                if (!string.IsNullOrEmpty(GlobalSettingsService.Current.SelectedGame))
                {
                    SelectedGame = Games.FirstOrDefault(g => g.GameCode == GlobalSettingsService.Current.SelectedGame);
                }
            }

            catch (Exception ex)
            {
                // ErrorMessage = "Failed to load games. Please try again.";
                ErrorMessage = AppResources.ErrorLoadingGames;
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

    }
}
