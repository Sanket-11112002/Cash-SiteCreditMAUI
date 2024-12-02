using CardGameCorner.Models;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public SettingsViewModel(IGameService gameService, ISecureStorage secureStorage)
        {
            _gameService = gameService;
            _secureStorage = secureStorage;
            Games = new ObservableCollection<Game>();
            // Load games when the ViewModel is created
    LoadGamesAsync().ConfigureAwait(false);
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
                ErrorMessage = "Failed to load games. Please try again.";
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

    }
}
