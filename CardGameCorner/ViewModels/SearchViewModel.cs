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

      


        public ICommand ToggleFavoriteCommand { get; }
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
            // Load initial data
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await LoadDataAsync(string.Empty);
            });


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

        private  void UpdateProducts(List<Product> products, bool isSearching)
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