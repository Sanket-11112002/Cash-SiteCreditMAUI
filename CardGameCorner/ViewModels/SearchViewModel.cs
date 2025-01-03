//using CardGameCorner.Models;
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
using SkiaSharp;
using System.Globalization;
using System.Resources;
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
        public SearchViewModel()
        {
            // Initialize with current language
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

            _searchService = new SearchService();
            _scanCardService = new ScanCardService();
            Products = new ObservableCollection<Product>();
            RefreshCommand = new Command(async () => await LoadDataAsync(SearchQuery));
            OpenProductUrlCommand = new Command<string>(async (url) => await OpenProductUrl(url));
            SearchCommand = new Command(async () => await LoadDataAsync(SearchQuery));
            ToggleFavoriteCommand = new Command<Product>(ToggleFavorite);
            CardSelectedCommand = new Command<Product>(async (product) => await OnCardSelected(product));
        
        //OnUploadButtonClickedCommand = new Command<CardDetailViewModel>(OnUploadButtonClick);
        // Load initial data
        MainThread.BeginInvokeOnMainThread(async () =>
            {
                await LoadDataAsync(string.Empty);
            });


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
        //            await Application.Current.MainPage.DisplayAlert("Error", "Selected card object is not found.", "OK");
        //            return;
        //        }

        //        string imageUrl = selectedCard.Image;
        //        if (string.IsNullOrEmpty(imageUrl))
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error", "No image URL available.", "OK");
        //            return;
        //        }

        //        var imageBytes = await DownloadImageAsync(imageUrl);
        //        if (imageBytes == null || imageBytes.Length == 0)
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error", "Failed to download image.", "OK");
        //            return;
        //        }

        //        var compressedImageStream = await CompressImageAsync(new MemoryStream(imageBytes), 100 * 1024);
        //        var uploadStream = new MemoryStream();
        //        compressedImageStream.Position = 0;
        //        await compressedImageStream.CopyToAsync(uploadStream);
        //        uploadStream.Position = 0;

        //        var cardRequest = new CardSearchRequest
        //        {
        //            Title = selectedCard.Model,
        //            Set = selectedCard.SetCode,
        //            Game = selectedCard.Game,
        //            Lang = "en",
        //            Foil = 0,
        //            FirstEdition = 0
        //        };

        //        CardComparisonViewModel data = await SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(uploadStream.ToArray())));

        //        if (data != null && data.responseContent.Products != null)
        //        {
        //            var detailslst = await ProcessCardDetails(data.responseContent.Products);
        //            var detailsJson = JsonConvert.SerializeObject(detailslst);
        //            await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(detailsJson)}");
        //        }
        //        else
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error", "No products found.", "OK");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", $"Failed to process card: {ex.Message}", "OK");
        //    }
        //    finally
        //    {
        //        IsLoading = false;
        //        IsInteractionEnabled = true;
        //    }
        //}

        //private async Task<byte[]> DownloadImageAsync(string imageUrl)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            var response = await client.GetAsync(imageUrl);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                return await response.Content.ReadAsByteArrayAsync();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error downloading image: {ex.Message}");
        //    }
        //    return null;
        //}

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
                    Lang = "en",
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
                        SelectedLanguage = lnglst.Where(item => item.Id == product.Language)
                            .Select(item => item.Language).FirstOrDefault(),
                        selectedCondition = conditinlst.Where(item => item.Id == product.Condition)
                            .Select(item => item.Condition).FirstOrDefault(),
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



        //public async Task<CardComparisonViewModel> SearchCardAsync(CardSearchRequest cardSearch, ImageSource imageSource)
        //{
        //    try
        //    {
        //        CardSearchResponseViewModel cardSearchResponseViewModel = await _scanCardService.SearchCardAsync(cardSearch);

        //        if (cardSearchResponseViewModel != null && cardSearchResponseViewModel.Products.Count > 0)
        //        {
        //            // Here, you can access and display data from the response


        //            var product = cardSearchResponseViewModel.Products[0];
        //            Console.WriteLine($"Product Model: {product.ModelEn}, Price: {product.MinPrice}");

        //            // Initialize the CardComparisonPage ViewModel
        //            CardComparisonViewModel comparisonData = new CardComparisonViewModel();

        //            // Initialize the comparison data (you can pass the full response or use specific data)
        //            comparisonData.Initialize(cardSearchResponseViewModel, imageSource);


        //            return comparisonData;


        //            //await Shell.Current.GoToAsync(nameof(CardComparisonPage), new Dictionary<string, object>
        //            //{
        //            //    { "ComparisonData", comparisonData }
        //            //});

        //            //        var navigationParameters = new Dictionary<string, object>
        //            //{
        //            //    { "ComparisonData", comparisonData }
        //            //};

        //            //        await Shell.Current.GoToAsync(nameof(CardComparisonPage), navigationParameters);






        //        }
        //        else
        //        {
        //            Console.WriteLine("No products found in card search.");
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Image compression failed: {ex.Message}");
        //        return null;
        //    }
        //    finally
        //    {

        //    }

        //}


        //public async Task<List<CardComparisonViewModel>> SearchCardAsync(CardSearchRequest cardSearch, ImageSource imageSource)
        //{
        //    try
        //    {
        //        CardSearchResponseViewModel cardSearchResponseViewModel = await _scanCardService.SearchCardAsync(cardSearch);
        //        List<CardComparisonViewModel> comparisonDataList = new List<CardComparisonViewModel>();
        //        if (cardSearchResponseViewModel != null && cardSearchResponseViewModel.Products.Count > 0)
        //        {
        //            // Here, you can access and display data from the response


        //            // Iterate through each product
        //            for (int i = 0; i < cardSearchResponseViewModel.Products.Count; i++)
        //            {
        //                var product = cardSearchResponseViewModel.Products[i];
        //                Console.WriteLine($"Product Model: {product.ModelEn}, Price: {product.MinPrice}");

        //                // Initialize the CardComparisonPage ViewModel
        //                CardComparisonViewModel comparisonData = new CardComparisonViewModel();

        //                // Initialize the comparison data (you can pass the full response or use specific data)
        //                comparisonData.Initialize(cardSearchResponseViewModel, imageSource);

        //                // Add the comparison data to the list
        //                comparisonDataList.Add(comparisonData);
        //            }

        //            // Return the list of comparison data after processing all products
        //            return comparisonDataList;

        //            //await Shell.Current.GoToAsync(nameof(CardComparisonPage), new Dictionary<string, object>
        //            //{
        //            //    { "ComparisonData", comparisonData }
        //            //});

        //            //        var navigationParameters = new Dictionary<string, object>
        //            //{
        //            //    { "ComparisonData", comparisonData }
        //            //};

        //            //        await Shell.Current.GoToAsync(nameof(CardComparisonPage), navigationParameters);






        //        }
        //        else
        //        {
        //            Console.WriteLine("No products found in card search.");
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Image compression failed: {ex.Message}");
        //        return null;
        //    }
        //    finally
        //    {

        //    }

        //}



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
}