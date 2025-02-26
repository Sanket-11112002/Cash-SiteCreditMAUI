using CardGameCorner.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Windows.Input;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;
using CardGameCorner.Resources.Language;

namespace CardGameCorner.ViewModels
{
    public partial class GameDetailsViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly ISecureStorage secureStorage;

        private ObservableCollection<Card>? _cards;
        private ObservableCollection<Banner1> _banners;
        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        [ObservableProperty]
        private string bestDeals;

        public ICommand OpenProductUrlCommand { get; }

        public GameDetailsViewModel()
        {
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;


            OpenProductUrlCommand = new Command<string>(async (url) =>
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    try
                    {
                        await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error opening URL: {ex.Message}");
                    }
                }
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
                BestDeals = AppResources.Best_Deals; // Localized string for "Search"


                // Trigger property changed events to update UI

                OnPropertyChanged(nameof(BestDeals));
            });
        }

        public ObservableCollection<Card> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Banner1> Banners
        {
            get => _banners;
            set
            {
                _banners = value;
                OnPropertyChanged(nameof(Banners));
            }
        }
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



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadGameDetails(string uiCode, string gameCode)
        {
            var url = $"https://api.magiccorner.it/api/McHomePage/{Uri.EscapeDataString(uiCode)}/{Uri.EscapeDataString(gameCode)}";

            using (var client = new HttpClient())
            {
                // Set the authorization header with the Bearer token
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "0d1bb073-9dfb-4c6d-a1c0-1e8f7d5d8e9f");

                try
                {
                    // Fetch data from the API
                    var response = await client.GetStringAsync(url);
                    Debug.WriteLine($"API Response: {response}");

                    // Deserialize response into GameDetailsResponse model
                    var gameDetails = JsonConvert.DeserializeObject<GameDetailsResponse>(response);

                    if (gameDetails?.Data?.Value == null || gameDetails.Data.Value.Products == null)
                    {
                        Debug.WriteLine("Deserialization returned null. Check the API response and GameDetailsResponse model.");
                        return;
                    }

                    // Populate the Cards collection
                    var cards = gameDetails.Data.Value.Products.Select(product => new Card
                    {
                        Image = ImageSource.FromUri(new Uri($"https://www.cardgamecorner.com{product.Image}")),
                        Note = uiCode == "it" ? product.Model : product.ModelEn,
                        MaxPrice = product.MaxPrice, // No need to parse as it's already decimal
                        MinPrice = product.MinPrice
                    }).ToList();

                    Cards = new ObservableCollection<Card>(cards);

                    // Filter and populate banners
                    var banners = gameDetails.Banners?
                        .Where(banner =>
                            !string.IsNullOrWhiteSpace(banner.Image) &&
                            banner.Image.StartsWith("http") &&
                            !string.Equals(banner.Image, "https://www.cardgamecorner.com", StringComparison.OrdinalIgnoreCase) &&
                            !string.IsNullOrWhiteSpace(banner.Url))
                        .Select(banner => new Banner1
                        {
                            Title = banner.Title,
                            ImageUrl = banner.Image,
                            Url = banner.Url
                        })
                        .ToList();

                    if (banners != null)
                        Banners = new ObservableCollection<Banner1>(banners);

                    // Debugging banners
                    foreach (var banner in Banners)
                    {
                        Debug.WriteLine($"Banner - Title: {banner.Title}, Image URL: {banner.ImageUrl}, Url: {banner.Url}");
                    }

                    Debug.WriteLine($"Selected Game Code: {GlobalSettings.SelectedGame}");

                    var selectedGameCode = GlobalSettings.SelectedGame;

                    var service = new GameService(secureStorage);
                    var games = await service.GetGamesAsync();

                    var selectedGame = games.FirstOrDefault(item => item.GameCode == selectedGameCode);
                    if (selectedGame != null)
                    {
                        Debug.WriteLine($"Home Best Deals Image for {gameCode}: {selectedGame.HomeBestDealsImage}");
                        HomeBestDealsImage = selectedGame.HomeBestDealsImage;
                    }
                    else
                    {
                        Debug.WriteLine($"No game found with game code: {gameCode}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error fetching game details: {ex.Message}");
                }
            }
        }

    }
}




