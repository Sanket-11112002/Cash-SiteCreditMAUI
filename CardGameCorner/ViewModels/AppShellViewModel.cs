using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CardGameCorner.ViewModels
{
    public partial class AppShellViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly GlobalSettingsService _globalSettings;
        private string _homeTabTitle;
        private string _searchTabTitle;
        private string _scanTabTitle;
        private string _myAccountTabTitle;
        private string _myListTabTitle;
        private string _settingsToolbarItemText;


        public AppShellViewModel(GlobalSettingsService globalSettings)
        {
            _globalSettings = globalSettings;
            UpdateLocalizedStrings();

            // Subscribe to language change events
            _globalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;
        }

        public string HomeTabTitle
        {
            get => _homeTabTitle;
            set => SetProperty(ref _homeTabTitle, value);
        }

        public string SearchTabTitle
        {
            get => _searchTabTitle;
            set => SetProperty(ref _searchTabTitle, value);
        }

        public string ScanTabTitle
        {
            get => _scanTabTitle;
            set => SetProperty(ref _scanTabTitle, value);
        }

        public string MyAccountTabTitle
        {
            get => _myAccountTabTitle;
            set => SetProperty(ref _myAccountTabTitle, value);
        }

        public string MyListTabTitle
        {
            get => _myListTabTitle;
            set => SetProperty(ref _myListTabTitle, value);
        }

        public string SettingsToolbarItemText
        {
            get => _settingsToolbarItemText;
            set => SetProperty(ref _settingsToolbarItemText, value);
        }



        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_globalSettings.SelectedLanguage))
            {
                // Update localized strings when language changes
                UpdateLocalizedStrings();
            }
        }

        private void UpdateLocalizedStrings()
        {
            HomeTabTitle = AppResources.Home; // Localized string for "Home"
            SearchTabTitle = AppResources.Search; // Localized string for "Search"
            ScanTabTitle = AppResources.Scan; // Localized string for "Scan"
            MyAccountTabTitle = AppResources.MyAccount; // Localized string for "My Account"
            MyListTabTitle = AppResources.MyList; // Localized string for "My List"
            SettingsToolbarItemText = AppResources.Settings; // Localized string for "Settings"

            OnPropertyChanged(string.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}