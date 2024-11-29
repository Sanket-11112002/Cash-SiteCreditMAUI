﻿//using CardGameCorner.Models;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using Microsoft.Maui.Controls;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CardGameCorner.Services;
//using CardGameCorner.Resources.Language;

//namespace CardGameCorner.ViewModels
//{
//    public partial class SearchViewModel : ObservableObject,INotifyPropertyChanged
//    {
//        private readonly SearchService _searchService;
//        private string _searchQuery = string.Empty;
//        private bool _isLoading;
//        private bool _hasError;
//        private string _errorMessage;
//        private bool _noResultsFound;
//        private CancellationTokenSource _searchCancellationTokenSource;

//        public ObservableCollection<Product> Products { get; private set; }

//        public bool IsLoading
//        {
//            get => _isLoading;
//            set
//            {
//                _isLoading = value;
//                OnPropertyChanged();
//            }
//        }

//        public bool HasError
//        {
//            get => _hasError;
//            set
//            {
//                _hasError = value;
//                OnPropertyChanged();
//            }
//        }

//        public string ErrorMessage
//        {
//            get => _errorMessage;
//            set
//            {
//                _errorMessage = value;
//                OnPropertyChanged();
//            }
//        }

//        public bool NoResultsFound
//        {
//            get => _noResultsFound;
//            set
//            {
//                _noResultsFound = value;
//                OnPropertyChanged();
//            }
//        }

//        public string SearchQuery
//        {
//            get => _searchQuery;
//            set
//            {
//                if (_searchQuery != value)
//                {
//                    _searchQuery = value;
//                    OnPropertyChanged();
//                    CancelPreviousSearch();
//                    _ = DelayedSearchAsync();
//                }
//            }
//        }

//        public ICommand RefreshCommand { get; }
//        public ICommand OpenProductUrlCommand { get; }

//        private readonly int _searchDelayMilliseconds = 500;

//        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

//        [ObservableProperty]
//        private string searchText;

//        [ObservableProperty]
//        private string scanText;

//        [ObservableProperty]
//        private string welcomeMessage;

//        public SearchViewModel()
//        {
//            // Initialize with current language
//            UpdateLocalizedStrings();

//            // Subscribe to language change events
//            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

//            _searchService = new SearchService();
//            Products = new ObservableCollection<Product>();
//            RefreshCommand = new Command(async () => await LoadDataAsync(SearchQuery));
//            OpenProductUrlCommand = new Command<string>(async (url) => await OpenProductUrl(url));

//            // Load initial data
//            MainThread.BeginInvokeOnMainThread(async () =>
//            {
//                await LoadDataAsync(string.Empty);
//            });
//        }

//        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
//            {
//                // Update localized strings when language changes
//                UpdateLocalizedStrings();
//            }
//        }

//        private void UpdateLocalizedStrings()
//        {
//            // Ensure these are called on the main thread
//            MainThread.BeginInvokeOnMainThread(() =>
//            {
//                SearchText = AppResources.Search; // Localized string for "Search"
//                ScanText = AppResources.Scan_with_camera; // Localized string for "Scan with camera"
//                WelcomeMessage = AppResources.WelcomeMessage;

//                // Trigger property changed events to update UI
//                OnPropertyChanged(nameof(SearchText));
//                OnPropertyChanged(nameof(ScanText));
//                OnPropertyChanged(nameof(WelcomeMessage));
//            });
//        }
//        private void CancelPreviousSearch()
//        {
//            _searchCancellationTokenSource?.Cancel();
//            _searchCancellationTokenSource = new CancellationTokenSource();
//        }

//        private async Task DelayedSearchAsync()
//        {
//            try
//            {
//                var token = _searchCancellationTokenSource.Token;
//                await Task.Delay(_searchDelayMilliseconds, token);
//                await LoadDataAsync(SearchQuery);
//            }
//            catch (TaskCanceledException)
//            {
//                // Search was cancelled, ignore
//            }
//        }

//        private async Task LoadDataAsync(string query)
//        {
//            if (IsLoading) return;

//            try
//            {
//                IsLoading = true;
//                HasError = false;
//                ErrorMessage = string.Empty;

//                var products = await _searchService.SearchProductsAsync(query);

//                // Only update UI if we weren't cancelled
//                if (!_searchCancellationTokenSource?.Token.IsCancellationRequested ?? true)
//                {
//                    UpdateProducts(products, !string.IsNullOrWhiteSpace(query));
//                }
//            }
//            catch (Exception ex)
//            {
//                if (!_searchCancellationTokenSource?.Token.IsCancellationRequested ?? true)
//                {
//                    HandleError("An error occurred while loading data. Please try again.", ex);
//                }
//            }
//            finally
//            {
//                IsLoading = false;
//            }
//        }

//        private void UpdateProducts(List<Product> products, bool isSearching)
//        {
//            MainThread.BeginInvokeOnMainThread(() =>
//            {
//                Products.Clear();

//                if (products == null || !products.Any())
//                {
//                    NoResultsFound = isSearching;
//                    return;
//                }

//                foreach (var product in products)
//                {
//                    if (!string.IsNullOrEmpty(product.Image))
//                    {
//                        product.Image = product.Image.StartsWith("http")
//                            ? product.Image
//                            : $"https://www.cardgamecorner.com{product.Image}";
//                    }
//                    Products.Add(product);
//                }

//                NoResultsFound = false;
//            });
//        }

//        private async Task OpenProductUrl(string url)
//        {
//            if (!string.IsNullOrEmpty(url))
//            {
//                try
//                {
//                    await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
//                }
//                catch (Exception ex)
//                {
//                    HandleError("Could not open the product URL.", ex);
//                }
//            }
//        }

//        private void HandleError(string userMessage, Exception ex)
//        {
//            MainThread.BeginInvokeOnMainThread(() =>
//            {
//                HasError = true;
//                ErrorMessage = userMessage;
//                Console.WriteLine($"Error details: {ex}");
//            });
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}

using CardGameCorner.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CardGameCorner.Services;
using CardGameCorner.Resources.Language;

namespace CardGameCorner.ViewModels
{
    public partial class SearchViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly SearchService _searchService;
        private string _searchQuery = string.Empty;
        private bool _isLoading;
        private bool _hasError;
        private string _errorMessage;
        private bool _noResultsFound;
        private CancellationTokenSource _searchCancellationTokenSource;

        public ObservableCollection<Product> Products { get; private set; }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool NoResultsFound
        {
            get => _noResultsFound;
            set
            {
                _noResultsFound = value;
                OnPropertyChanged();
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand OpenProductUrlCommand { get; }
        public ICommand SearchCommand { get; }

        private readonly int _searchDelayMilliseconds = 500;

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private string scanText;

        [ObservableProperty]
        private string welcomeMessage;

        public SearchViewModel()
        {
            // Initialize with current language
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

            _searchService = new SearchService();
            Products = new ObservableCollection<Product>();
            RefreshCommand = new Command(async () => await LoadDataAsync(SearchQuery));
            OpenProductUrlCommand = new Command<string>(async (url) => await OpenProductUrl(url));
            SearchCommand = new Command(async () => await LoadDataAsync(SearchQuery));

            // Load initial data
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await LoadDataAsync(string.Empty);
            });
        }

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
                SearchText = AppResources.Search; // Localized string for "Search"
                ScanText = AppResources.Scan_with_camera; // Localized string for "Scan with camera"
                WelcomeMessage = AppResources.WelcomeMessage;

                // Trigger property changed events to update UI
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(ScanText));
                OnPropertyChanged(nameof(WelcomeMessage));
            });
        }

        private void CancelPreviousSearch()
        {
            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource = new CancellationTokenSource();
        }

        private async Task LoadDataAsync(string query)
        {
            if (IsLoading) return;

            CancelPreviousSearch();

            try
            {
                IsLoading = true;
                HasError = false;
                ErrorMessage = string.Empty;

                var products = await _searchService.SearchProductsAsync(query);

                // Only update UI if we weren't cancelled
                if (!_searchCancellationTokenSource?.Token.IsCancellationRequested ?? true)
                {
                    UpdateProducts(products, !string.IsNullOrWhiteSpace(query));
                }
            }
            catch (Exception ex)
            {
                if (!_searchCancellationTokenSource?.Token.IsCancellationRequested ?? true)
                {
                    HandleError("An error occurred while loading data. Please try again.", ex);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void UpdateProducts(List<Product> products, bool isSearching)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Products.Clear();

                if (products == null || !products.Any())
                {
                    NoResultsFound = isSearching;
                    return;
                }

                foreach (var product in products)
                {
                    if (!string.IsNullOrEmpty(product.Image))
                    {
                        product.Image = product.Image.StartsWith("http")
                            ? product.Image
                            : $"https://www.cardgamecorner.com{product.Image}";
                    }
                    Products.Add(product);
                }

                NoResultsFound = false;
            });
        }

        private async Task OpenProductUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    HandleError("Could not open the product URL.", ex);
                }
            }
        }

        private void HandleError(string userMessage, Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                HasError = true;
                ErrorMessage = userMessage;
                Console.WriteLine($"Error details: {ex}");
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}