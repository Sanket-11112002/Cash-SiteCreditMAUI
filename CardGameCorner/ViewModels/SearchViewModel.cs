using CardGameCorner.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CardGameCorner.Services;
using CardGameCorner.Resources.Language;
using SkiaSharp;
using System.Globalization;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;
using System.Diagnostics;
using Newtonsoft.Json;
using CardGameCorner.Views;

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
        private readonly IScanCardService _scanCardService;
        private readonly ISecureStorage secureStorage;
        private readonly IListboxService _listboxService;


        public ObservableCollection<Product> Products { get; private set; }

        private string _homeBestDealsImage;
        public string HomeBestDealsImage
        {
            get => _homeBestDealsImage;
            set
            {
                _homeBestDealsImage = value;
                OnPropertyChanged();
            }
        }

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

        private bool _isInteractionEnabled = true;
        public bool IsInteractionEnabled
        {
            get => _isInteractionEnabled;
            set
            {
                _isInteractionEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand CardSelectedCommand { get; }

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
        private string notFound;

        [ObservableProperty]
        private string notFoundProduct;

        [ObservableProperty]
        private string tryAgain;

        [ObservableProperty]
        private string internettryAgain;

        public ICommand ToggleFavoriteCommand { get; }
        public ICommand OnUploadButtonClickedCommand { get; }


        private bool _isFiltersVisible;
        public bool IsFiltersVisible
        {
            get => _isFiltersVisible;
            set
            {
                if (SetProperty(ref _isFiltersVisible, value))
                {
                    OnPropertyChanged(nameof(IsFiltersVisible));
                }
            }
        }

        public ICommand ToggleFiltersCommand { get; private set; }
        public ICommand ApplyFiltersCommand { get; private set; }
        public ICommand ClearFiltersCommand { get; private set; }


        public SearchViewModel()
        {
            ResetFilters();
            // Initialize with current language
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;
            ConfigureFiltersForGame(GlobalSettings.SelectedGame);

            _searchService = new SearchService();
            _scanCardService = new ScanCardService();
            _listboxService = new ListBoxService();

            Products = new ObservableCollection<Product>();
            RefreshCommand = new Command(async () => await LoadDataAsync(SearchQuery));
            OpenProductUrlCommand = new Command<string>(async (url) => await OpenProductUrl(url));
            SearchCommand = new Command(async () => await LoadDataAsync(SearchQuery));
            ToggleFavoriteCommand = new Command<Product>(ToggleFavorite);
            CardSelectedCommand = new Command<Product>(async (product) => await OnCardSelected(product));

            InitializeCommands();
            //OnUploadButtonClickedCommand = new Command<CardDetailViewModel>(OnUploadButtonClick);
            LoadFilterOptionsAsync();

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await LoadDataAsync(string.Empty);
            });

        }

        // Filters code
        private void InitializeCommands()
        {
            ToggleFiltersCommand = new Command(ExecuteToggleFilters);
            ApplyFiltersCommand = new Command(ExecuteApplyFilters);
            ClearFiltersCommand = new Command(ExecuteClearFilters);
        }

        private void ConfigureFiltersForGame(string gameCode)
        {
            switch (gameCode.ToLower())
            {
                case "pokemon":
                case "yugioh":
                    IsColorFilterVisible = false;
                    IsFirstEditionFilterVisible = true;
                    break;
                case "onepiece":
                    IsColorFilterVisible = false;
                    IsFirstEditionFilterVisible = false;
                    break;
                case "magic":
                    IsColorFilterVisible = true;
                    IsFirstEditionFilterVisible = false;
                    break;
                default:
                    IsColorFilterVisible = false;
                    IsFirstEditionFilterVisible = false;
                    break;
            }
        }
        private void ExecuteClearFilters()
        {
            IsLoading = true;

            try
            {
                // Clear all temporary and selected values
                _tempSelectedEdition = null;
                _tempSelectedRarity = null;
                _tempSelectedLanguage = null;
                _tempSelectedColor = null;
                _tempSelectedFirstEdition = null;
                _tempSelectedHotList = "All";

                SelectedEdition = null;
                SelectedRarity = null;
                SelectedLanguage = null;
                SelectedColor = null;
                SelectedFirstEdition = null;
                SelectedHotList = "All";

                // Reset collections
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Editions.Clear();
                    Rarities.Clear();
                    Languages.Clear();
                    Colors.Clear();
                    FirstEditions.Clear();

                    // Reload filter options
                    LoadFilterOptionsAsync();
                });

                // Trigger search with cleared filters
                SearchCommand.Execute(null);

                // Close the filters panel
                IsFiltersVisible = false;
            }
            finally
            {
                IsLoading = false;
            }
        }
        private void ResetFilters()
        {
            // Clear the collections first to trigger UI update
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Reset collections
                Editions.Clear();
                Rarities.Clear();
                Languages.Clear();
                Colors.Clear();
                FirstEditions.Clear();

                // Re-add the null placeholder
                Editions.Add(null);
                Rarities.Add(null);
                Languages.Add(null);
                Colors.Add(null);
                FirstEditions.Add(null);

                // Reset both temporary and actual values to null except HotList
                _tempSelectedEdition = null;
                _tempSelectedRarity = null;
                _tempSelectedLanguage = null;
                _tempSelectedColor = null;
                _tempSelectedFirstEdition = null;
                _tempSelectedHotList = "All";

                // Reset selected values
                SelectedEdition = null;
                SelectedRarity = null;
                SelectedLanguage = null;
                SelectedColor = null;
                SelectedFirstEdition = null;
                SelectedHotList = "All";

                // Reload filter options
                LoadFilterOptionsAsync();
            });

            // Force UI refresh
            OnPropertyChanged(nameof(SelectedEdition));
            OnPropertyChanged(nameof(SelectedRarity));
            OnPropertyChanged(nameof(SelectedLanguage));
            OnPropertyChanged(nameof(SelectedColor));
            OnPropertyChanged(nameof(SelectedFirstEdition));
            OnPropertyChanged(nameof(SelectedHotList));
        }


        private void ExecuteToggleFilters()
        {
            if (IsFiltersVisible)
            {
                // When closing, restore the previous applied values
                IsFiltersVisible = false;
                RestoreAppliedFilters();
            }
            else
            {
                // When opening, show current values
                IsFiltersVisible = true;
                SetCurrentFiltersFromTemp();
            }
        }

        private void SetCurrentFiltersFromTemp()
        {
            SelectedEdition = _tempSelectedEdition;
            SelectedRarity = _tempSelectedRarity;
            SelectedLanguage = _tempSelectedLanguage;
            SelectedColor = _tempSelectedColor;
            SelectedFirstEdition = _tempSelectedFirstEdition;
            SelectedHotList = _tempSelectedHotList ?? "All";
        }

        private void RestoreAppliedFilters()
        {
            // Restore the last applied filter values
            SelectedEdition = _tempSelectedEdition;
            SelectedRarity = _tempSelectedRarity;
            SelectedLanguage = _tempSelectedLanguage;
            SelectedColor = _tempSelectedColor;
            SelectedFirstEdition = _tempSelectedFirstEdition;
            SelectedHotList = _tempSelectedHotList;
        }


        private async void ExecuteApplyFilters()
        {
            IsLoading = true;

            try
            {
                // Store current values as temporary values
                _tempSelectedEdition = SelectedEdition;
                _tempSelectedRarity = SelectedRarity;
                _tempSelectedLanguage = SelectedLanguage;
                _tempSelectedColor = SelectedColor;
                _tempSelectedFirstEdition = SelectedFirstEdition;
                _tempSelectedHotList = SelectedHotList;

                // Execute search with current filters
                await LoadDataAsync(SearchQuery);

                // Close the filters panel
                IsFiltersVisible = false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadFilterOptionsAsync()
        {
            try
            {
                var filterOptions = await _listboxService.GetFilterListBoxDataAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Clear and add null as first option for all except HotList
                    Editions.Clear();
                    if (filterOptions?.Expansions != null)
                    {
                        foreach (var edition in filterOptions.Expansions)
                        {
                            Editions.Add(edition.Espansione);
                        }
                    }

                    Rarities.Clear();
                    var rarityListbox = filterOptions.Listboxes.FirstOrDefault(lb => lb.Filter == "rarity");
                    if (rarityListbox?.Options != null)
                    {
                        foreach (var rarity in rarityListbox.Options)
                        {
                            if (!string.IsNullOrEmpty(rarity.Name))
                            {
                                Rarities.Add(rarity.Name);
                            }
                        }
                    }

                    Languages.Clear();
                    var languageListbox = filterOptions.Listboxes.FirstOrDefault(lb => lb.Filter == "language");
                    if (languageListbox?.Options != null)
                    {
                        foreach (var language in languageListbox.Options)
                        {
                            if (!string.IsNullOrEmpty(language.Name))
                            {
                                Languages.Add(language.Name);
                            }
                        }
                    }

                    Colors.Clear();
                    var colorListbox = filterOptions.Listboxes.FirstOrDefault(lb => lb.Filter == "color");
                    if (colorListbox?.Options != null)
                    {
                        foreach (var color in colorListbox.Options)
                        {
                            if (!string.IsNullOrEmpty(color.Name))
                            {
                                Colors.Add(color.Name);
                            }
                        }
                    }

                    FirstEditions.Clear();
                    var firstEditionListbox = filterOptions.Listboxes.FirstOrDefault(lb => lb.Filter == "firstedition");
                    if (firstEditionListbox?.Options != null)
                    {
                        foreach (var edition in firstEditionListbox.Options)
                        {
                            if (!string.IsNullOrEmpty(edition.Name))
                            {
                                FirstEditions.Add(edition.Name);
                            }
                        }
                        FirstEditions.Add("null");
                    }

                    // Ensure HotList keeps "All" as default
                    SelectedHotList = "All";
                });
            }
            catch (Exception ex)
            {
                HandleError("Failed to load filter options", ex);
            }
        }



        // Your existing filter collections and properties remain the same
        // Existing collections
        public ObservableCollection<string> Editions { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Rarities { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Languages { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Colors { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> FirstEditions { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> HotList { get; } = new ObservableCollection<string>
    {
        "All",
        "Hot BuyList Only"
    };

        private bool _isColorFilterVisible;
        public bool IsColorFilterVisible
        {
            get => _isColorFilterVisible;
            set => SetProperty(ref _isColorFilterVisible, value);
        }

        private bool _isFirstEditionFilterVisible;
        public bool IsFirstEditionFilterVisible
        {
            get => _isFirstEditionFilterVisible;
            set => SetProperty(ref _isFirstEditionFilterVisible, value);
        }


        private string _tempSelectedEdition;
        private string _tempSelectedRarity;
        private string _tempSelectedLanguage;
        private string _tempSelectedColor;
        private string _tempSelectedFirstEdition;
        private string _tempSelectedHotList = "All"; // Only HotList has default value


        private string _selectedEdition;
        public string SelectedEdition
        {
            get => _selectedEdition;
            set
            {
                SetProperty(ref _selectedEdition, value);
                _tempSelectedEdition = value; // Update temp value when selection changes
            }
        }

        private string _selectedRarity;
        public string SelectedRarity
        {
            get => _selectedRarity;
            set
            {
                SetProperty(ref _selectedRarity, value);
                _tempSelectedRarity = value;
            }
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                SetProperty(ref _selectedLanguage, value);
                _tempSelectedLanguage = value;
            }
        }

        private string _selectedColor;
        public string SelectedColor
        {
            get => _selectedColor;
            set
            {
                SetProperty(ref _selectedColor, value);
                _tempSelectedColor = value;
            }
        }

        private string _selectedFirstEdition;
        public string SelectedFirstEdition
        {
            get => _selectedFirstEdition;
            set
            {
                SetProperty(ref _selectedFirstEdition, value);
                _tempSelectedFirstEdition = value;
            }
        }

        private string _selectedHotList = "All";
        public string SelectedHotList
        {
            get => _selectedHotList;
            set
            {
                SetProperty(ref _selectedHotList, value);
                _tempSelectedHotList = value;
            }
        }

        private async Task OnCardSelected(Product selectedCard)
        {
            if (!IsInteractionEnabled) return;
            IsInteractionEnabled = false;
            IsLoading = true;
            try
            {
                if (selectedCard == null)
                {
                    await Application.Current.MainPage.DisplayAlert(ErrorMessage, "Selected card object is not found.", "OK");
                    return;
                }
                string imageUrl = selectedCard.Image;
                if (string.IsNullOrEmpty(imageUrl))
                {
                    await Application.Current.MainPage.DisplayAlert(ErrorMessage, NotFound, "OK");
                    return;
                }
                var imageBytes = await DownloadImageAsync(imageUrl);
                if (imageBytes == null || imageBytes.Length == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(ErrorMessage, TryAgain, "OK");
                    return;
                }
                var compressedImageStream = await CompressImageAsync(new MemoryStream(imageBytes), 100 * 1024);
                var uploadStream = new MemoryStream();
                compressedImageStream.Position = 0;
                await compressedImageStream.CopyToAsync(uploadStream);
                uploadStream.Position = 0;
                var cardRequest = new CardSearchRequest
                {
                    Title = selectedCard.Model,
                    Set = selectedCard.SetCode,
                    Game = selectedCard.Game,
                    Lang = "",
                    Foil = 0,
                    FirstEdition = 0
                };
                CardComparisonViewModel data = await SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(uploadStream.ToArray())));
                if (data != null && data.responseContent.Products != null)
                {
                    var detailslst = await ProcessCardDetails(data.responseContent.Products);
                    var detailsJson = JsonConvert.SerializeObject(detailslst);
                    await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(detailsJson)}");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(ErrorMessage, NotFoundProduct, "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(ErrorMessage, $"Failed to process card: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
                IsInteractionEnabled = true;
            }
        }


        //private async Task OnCardSelected(Product selectedCard)
        //{
        //    if (!IsInteractionEnabled) return;
        //    IsInteractionEnabled = false;
        //    IsLoading = true;

        //    try
        //    {
        //        if (selectedCard == null)
        //        {
        //            await Application.Current.MainPage.DisplayAlert(ErrorMessage, "Selected card object is not found.", "OK");
        //            return;
        //        }

        //        if (string.IsNullOrEmpty(selectedCard.Image))
        //        {
        //            await Application.Current.MainPage.DisplayAlert(ErrorMessage, NotFound, "OK");
        //            return;
        //        }

        //        var selectedCardJson = JsonConvert.SerializeObject(selectedCard);
        //        await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(selectedCardJson)}");
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert(ErrorMessage, $"Failed to process card: {ex.Message}", "OK");
        //    }
        //    finally
        //    {
        //        IsLoading = false;
        //        IsInteractionEnabled = true;
        //    }
        //}

        private async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                Console.WriteLine("Error: Image URL is null or empty");
                return null;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                    using (var response = await client.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Error: Failed to download image. Status code: {response.StatusCode}");
                            return null;
                        }

                        using (var ms = new MemoryStream())
                        {
                            await response.Content.CopyToAsync(ms);
                            var imageBytes = ms.ToArray();
                            if (imageBytes.Length == 0)
                            {
                                Console.WriteLine("Error: Downloaded image is empty");
                                return null;
                            }
                            return imageBytes;
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error downloading image: {ex.Message}");
                return null;
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Download timeout: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading image: {ex.Message}");
                return null;
            }
        }

        private async Task<List<CardDetailViewModel>> ProcessCardDetails(List<Product1> products)
        {
            var detailslst = new List<CardDetailViewModel>();
            var response = new ListBoxService();
            var conditinlst = await response.GetConditionsAsync();
            var lnglst = await response.GetLanguagesAsync();

            foreach (var product in products)
            {
                var variants = product.Variants;
                if (variants != null)
                {
                    var languageConditionsMap = variants
                        .GroupBy(v => v.Language)
                        .ToDictionary(
                            group => group.Key,
                            group => group.Select(v => v.Condition)
                                .Distinct()
                                .ToList()
                        );

                    var distinctLanguages = variants.Select(v => v.Language).Distinct().ToList();
                    var distinctConditions = variants.Select(v => v.Condition).Distinct().ToList();

                    var details = new CardDetailViewModel()
                    {
                        Languages = distinctLanguages,
                        Conditions = distinctConditions,
                        Name = product.Model,
                        Rarity = product.Rarity,
                        Category = product.Category,
                        ImageUrl = "https://www.cardgamecorner.com" + product.Image,
                        game = product.Game,
                        LanguageConditionsMap = languageConditionsMap,
                        //SelectedLanguage = lnglst.Where(item => item.Id == product.Language)
                        //    .Select(item => item.Language).FirstOrDefault(),
                        SelectedLanguage = variants.Select(v => v.Language).FirstOrDefault(),
                        selectedCondition = variants.Select(v => v.Condition).FirstOrDefault(),
                        IsFirstEdition = variants.Select(v => v.FirstEdition != null ? true : false).FirstOrDefault(),
                        siteCredit= variants.Select(v => v.Credit).FirstOrDefault(),
                        buyList = variants.Select(v => v.BuyList).FirstOrDefault(),
                        varinats = variants.ToList()
                    };
                    detailslst.Add(details);
                }
            }
            return detailslst;
        }
        public async Task LoadBannerImage(string gameCode)
        {
            try
            {
                var selectedgamecode = GlobalSettings.SelectedGame;
                var service = new GameService(secureStorage);
                var games = await service.GetGamesAsync();

                var selectedgame = games.FirstOrDefault(item => item.GameCode == selectedgamecode);
                if (selectedgame != null)
                {
                    Debug.WriteLine($"Home Best Deals Image for {gameCode}: {selectedgame.HomeBestDealsImage}");
                    HomeBestDealsImage = selectedgame.HomeBestDealsImage;
                    // Force property change notification
                    OnPropertyChanged(nameof(HomeBestDealsImage));
                }
                else
                {
                    Debug.WriteLine($"No game found with game code: {gameCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading banner image: {ex.Message}");
            }
        }

        public void OnUploadButtonClicked(List<CardDetailViewModel> list)
        {

        }


        private void ToggleFavorite(Product product)
        {
            if (product == null) return;

            // Toggle the IsFavorite property
            product.IsFavorite = !product.IsFavorite;


            SaveFavorites();
            // Optional: Log or perform additional actions (e.g., store in DB or sync favorites)
            if (product.IsFavorite)
            {
                Console.WriteLine($"{product} added to favorites.");
            }
            else
            {
                Console.WriteLine($"{product} removed from favorites.");
            }
        }
        public class FavoriteIconConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (bool)value ? "filled_heart.png" : "empty_heart.png";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
            {
                UpdateLocalizedStrings();
                ConfigureFiltersForGame(GlobalSettings.SelectedGame);
                ResetFilters();
                LoadFilterOptionsAsync();
            }
        }

        private void UpdateLocalizedStrings()
        {
            // Ensure these are called on the main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                SearchText = AppResources.Search;
                ScanText = AppResources.Scan_with_camera;
                ErrorMessage = AppResources.ErrorTitle;
                NotFound = AppResources.NoResultsFound;
                NotFoundProduct = AppResources.notFoundProduct;
                TryAgain = AppResources.tryAgain;
                InternettryAgain = AppResources.ErrorMessage;

                // Trigger property changed events to update UI
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(ScanText));
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(InternettryAgain));
                OnPropertyChanged(nameof(NotFound));
                OnPropertyChanged(nameof(NotFoundProduct));
                OnPropertyChanged(nameof(TryAgain));

            });
        }

        private void CancelPreviousSearch()
        {
            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource = new CancellationTokenSource();
        }

        private async Task LoadDataAsync(string query)
        {
            // Only cancel if another search is already in progress
            if (IsLoading)
            {
                CancelPreviousSearch();
                // Small delay to ensure cancellation is processed
                await Task.Delay(100);
            }

            try
            {
                IsLoading = true;
                OnPropertyChanged(nameof(IsLoading));
                await Task.Delay(100);
                HasError = false;
                ErrorMessage = string.Empty;
                NoResultsFound = false; // Reset the no results flag

                var selectedGame = GlobalSettings.SelectedGame;
                var filterParams = new SearchFilters
                {
                    Edition = _tempSelectedEdition,
                    Rarity = _tempSelectedRarity,
                    Language = _tempSelectedLanguage,
                    HotList = _tempSelectedHotList == "All" ? null : _tempSelectedHotList
                };

                // Add game-specific filters
                switch (selectedGame.ToLower())
                {
                    case "pokemon":
                    case "yugioh":
                        filterParams.FirstEdition = _tempSelectedFirstEdition;
                        break;
                    case "magic":
                        filterParams.Colors = _tempSelectedColor;
                        break;
                }

                var products = await _searchService.SearchProductsAsync(query, filterParams);

                if (!_searchCancellationTokenSource?.Token.IsCancellationRequested ?? true)
                {
                    if (products == null || !products.Any())
                    {
                        NoResultsFound = true;
                        if (string.IsNullOrWhiteSpace(query) && !HasActiveFilters(filterParams))
                        {
                            NotFound = AppResources.notFoundProduct; // "Search for cards using the search bar above"
                        }
                        else if (HasActiveFilters(filterParams))
                        {
                            NotFound = AppResources.notFoundProduct; // "No cards found with the selected filters"
                        }
                        else
                        {
                            NotFound = AppResources.NoResultsFound; // "No cards found matching your search"
                        }
                    }
                    else
                    {
                        UpdateProducts(products, !string.IsNullOrWhiteSpace(query));
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                if (!_searchCancellationTokenSource?.Token.IsCancellationRequested ?? true)
                {
                    HandleError(ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                if (!_searchCancellationTokenSource?.Token.IsCancellationRequested ?? true)
                {
                    HandleError(AppResources.GenericErrorMessage, ex); // "An error occurred while loading data. Please try again."
                }
            }
            finally
            {
                IsLoading = false;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private bool HasActiveFilters(SearchFilters filters)
        {
            return !string.IsNullOrEmpty(filters.Edition) ||
                   !string.IsNullOrEmpty(filters.Rarity) ||
                   !string.IsNullOrEmpty(filters.Language) ||
                   !string.IsNullOrEmpty(filters.Colors) ||
                   !string.IsNullOrEmpty(filters.FirstEdition) ||
                   (filters.HotList?.Equals("Hot BuyList Only", StringComparison.OrdinalIgnoreCase) ?? false);
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
                LoadFavorites();

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

        public async Task<MemoryStream> CompressImageAsync(Stream inputStream, long maxSize)
        {
            try
            {

                using var skiaImage = SKBitmap.Decode(inputStream);
                if (skiaImage == null)
                    throw new Exception("Failed to decode the input image.");

                var width = 800; // Target width
                var height = (int)((float)skiaImage.Height * width / skiaImage.Width);
                var resizedImage = skiaImage.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);

                if (resizedImage == null)
                    throw new Exception("Failed to resize the image.");

                var quality = 85;
                MemoryStream compressedStream;
                do
                {
                    compressedStream = new MemoryStream();
                    using (var skImage = SKImage.FromBitmap(resizedImage))
                    {
                        var skData = skImage.Encode(SKEncodedImageFormat.Jpeg, quality);
                        skData.SaveTo(compressedStream);
                    }

                    if (compressedStream.Length <= maxSize)
                        break;

                    quality -= 5;
                    compressedStream.Dispose();
                } while (quality > 0);

                compressedStream.Position = 0;
                return compressedStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image compression failed: {ex.Message}");
                return null;
            }
        }
        public async Task<CardComparisonViewModel> SearchCardAsync(CardSearchRequest cardSearch, ImageSource imageSource)
        {
            try
            {
                var cardSearchResponseList = await _scanCardService.SearchCardAsync(cardSearch);

                if (cardSearchResponseList != null && cardSearchResponseList.Any())
                {
                    foreach (var cardSearchResponseViewModel in cardSearchResponseList)
                    {
                        if (cardSearchResponseViewModel.Products != null && cardSearchResponseViewModel.Products.Any())
                        {
                            foreach (var product in cardSearchResponseViewModel.Products)
                            {
                                Console.WriteLine($"Product Model: {product.ModelEn}, Price: {product.MinPrice}");
                            }

                            // Initialize the CardComparisonPage ViewModel
                            var comparisonData = new CardComparisonViewModel();
                            comparisonData.Initialize(cardSearchResponseViewModel, imageSource);

                            IsLoading = false;
                            return comparisonData; // Return the first valid comparison data
                        }
                    }

                    Console.WriteLine("No products found in the card search responses.");
                }
                else
                {
                    Console.WriteLine("No card search responses found.");
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Card search failed: {ex.Message}");
                return null;
            }
            finally
            {
                IsLoading = false;
            }
        }
        public async Task<ApiResponse_Card> UploadImageAsync(Stream imageStream)
        {

            var ApiResponse_Card = await _scanCardService.UploadImageAsync(imageStream);

            return ApiResponse_Card;
        }


        private void SaveFavorites()
        {
            //var _favcard = new FavouriteService();
            // Get the products marked as favorites
            var favoriteProducts = Products.Where(p => p.IsFavorite).ToList();

            // Store the favorite products using the FavoritesService
            FavouriteService.StoreFavorites(favoriteProducts);
        }

        private async Task LoadFavorites()
        {


            var favoriteProducts = FavouriteService.GetFavorites();

            // Set the IsFavorite property for each product based on stored favorites
            foreach (var product in Products)
            {
                product.IsFavorite = favoriteProducts.Any(f => f.Model == product.Model); // Match by ID or other properties
            }
        }
    }

    // Filters code
    public class SearchFilters
    {
        public string Edition { get; set; }
        public string Rarity { get; set; }
        public string Language { get; set; }
        public string Colors { get; set; }
        public string FirstEdition { get; set; }
        public string HotList { get; set; } = "All"; // Only HotList has default value
    }
}