using CardGameCorner.Models;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using Microsoft.Maui.Storage;
using System.ComponentModel;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.Views;

public partial class SettingsSlidePage : ContentPage, INotifyPropertyChanged
{
    private readonly IServiceProvider _serviceProvider;
    private readonly GlobalSettingsService _globalSettings;
    private readonly SettingsViewModel _viewModel;
    private readonly ISecureStorage _secureStorage;
    private readonly IAlertService _alertService;
    private readonly INavigationService _navigationService;
    public SettingsSlidePage(SettingsViewModel viewModel, ISecureStorage secureStorage, IAlertService alertService, INavigationService navigationService)
    {
        InitializeComponent();

        _globalSettings = GlobalSettingsService.Current;
        _viewModel = viewModel;
        _secureStorage = secureStorage;        // Set the binding context to the view model
        _alertService = alertService;
        _navigationService = navigationService;

        BindingContext = _viewModel;

        // Set initial selected items
        LanguagePicker.SelectedItem = _globalSettings.SelectedLanguage;
        // Find the game that matches the stored game code
        //GamePicker.SelectedItem = _globalSettings.SelectedGame;

        // Load games when the page is created
        LoadGamesAsync();
    }

    private async void LoadGamesAsync()
    {
        await _viewModel.LoadGamesAsync();

        if (!string.IsNullOrEmpty(_globalSettings.SelectedGame))
        {
            // Find the game that matches the stored game code
            GamePicker.SelectedItem = _viewModel.Games.FirstOrDefault(g => g.GameCode == _globalSettings.SelectedGame);
        }
    }

    private void OnLanguagePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (LanguagePicker.SelectedItem is string selectedLanguage)
        {
            _globalSettings.SelectedLanguage = selectedLanguage;
        }
    }

    private void OnGamePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (GamePicker.SelectedItem is Game selectedGame)
        {
            _globalSettings.SelectedGame = selectedGame.GameCode;
        }
    }

    //private async void OnCloseClicked(object sender, EventArgs e)
    //{
    //    await Shell.Current.Navigation.PopModalAsync();
    //}


    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Check if user is logged in
        if (App.IsUserLoggedIn)
        {
            LogoutButton.IsVisible = true;
        }
        else
        {
            LogoutButton.IsVisible = false;
        }

        // Hide the settings toolbar item when this page appears
        if (Shell.Current is AppShell appShell)
        {
            appShell.HideSettingsToolbarItem();
        }
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        // Save the selected game and language to secure storage
        if (!string.IsNullOrEmpty(_globalSettings.SelectedGame))
        {
            await _secureStorage.SetAsync("LastSelectedGame", _globalSettings.SelectedGame);
        }

        if (!string.IsNullOrEmpty(_globalSettings.SelectedLanguage))
        {
            await _secureStorage.SetAsync("LastSelectedLang", _globalSettings.SelectedLanguage);
        }

       

        // Restore the settings toolbar item when this page disappears
        if (Shell.Current is AppShell appShell)
        {
            appShell.ShowSettingsToolbarItem();
        }
    }


    private async void OnCloseClicked(object sender, EventArgs e)
    {
        // Use Shell navigation to go back
        await Shell.Current.GoToAsync("..");
    }
    private async void LogoutButton_Clicked(object sender, EventArgs e)
    {
        bool confirm = await _alertService.ShowConfirmationAsync(
            "Logout",
            "Are you sure you want to log out?");

        if (confirm)
        {

            await _navigationService.LogoutAsync();
        }
    }
}