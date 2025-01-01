using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
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

        LanguagePicker.SelectedItem = _globalSettings.SelectedLanguage;
        
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

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Check if user is logged in
        if (App.IsUserLoggedIn)
        {
            LogoutButton.IsVisible = true;
            MyOrdersButton.IsVisible = true;
        }
        else
        {
            LogoutButton.IsVisible = false;
            MyOrdersButton.IsVisible = false;
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
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            GlobalSettingsService.Current.OnLanguageChanged();
        });

        // Force refresh of the page before navigating back
        await Shell.Current.GoToAsync("..");
        // If the issue persists, you can try forcing a page refresh like this:

    }
    private async void MyOrdersButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MyOrdersPage));
    }
    private async void LogoutButton_Clicked(object sender, EventArgs e)
    {
        bool confirm = await _alertService.ShowConfirmationAsync(
            AppResources.LogoutConfirmationTitle,
            AppResources.LogoutConfirmationMessage
        );

        if (confirm)
        {
            await _navigationService.LogoutAsync();
        }
    }
}