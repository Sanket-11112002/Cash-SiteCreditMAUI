using CardGameCorner.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Input;
using System.Reflection;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;
using CardGameCorner.Resources.Language;

namespace CardGameCorner.ViewModels
{
    public partial class GameDetailsViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly ISecureStorage secureStorage;
      
        private ObservableCollection<Card> _cards;
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
                OnPropertyChanged();
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
                    var gameDetails = JsonConvert.DeserializeObject<GameDetailsResponse>(response);
                    if (gameDetails?.Data == null)
                    {
                        Debug.WriteLine("Deserialization returned null. Check the API response and GameDetailsResponse model.");
                        return;
                    }

                    // Populate the Cards collection
                    var cards = gameDetails.Data.Results.Select(result => new Card
                    {
                        Image = ImageSource.FromUri(new Uri($"https://www.cardgamecorner.com{result.image.raw}")),
                        Note = result.model.snippet,
                        Label = result.novita.raw,
                        MaxPrice = decimal.Parse(result.maxprice.raw.ToString("N2")),
                        MinPrice = decimal.Parse(result.minprice.raw.ToString("N2")),
                        ProductUrl = uiCode == "en"
                            ? $"https://www.cardgamecorner.com{result.urlen.raw}"
                            : $"https://www.cardgamecorner.com{result.url.raw}"
                    });
                    Cards = new ObservableCollection<Card>(cards);

                    var banners = gameDetails.Banners.Select(banner => new Banner1
                    {
                        Title = banner.Title,
                       //
                       Image = ImageSource.FromUri(new Uri(banner.Image)),
                      // Image=banner.Image,
                        Url = banner.Url
                    });
                    Banners = new ObservableCollection<Banner1>(banners);

                    foreach (var banner in Banners)
                    {
                        Debug.WriteLine($"Title: {banner.Title}, Image: {banner.Image}, Url: {banner.Url}");
                    }
                    Debug.WriteLine(GlobalSettings.SelectedGame);

                    var selectedgamecode = GlobalSettings.SelectedGame;

                    var service = new GameService(secureStorage);
                    var games = new List<Game>();
                    games = await service.GetGamesAsync();

                    var selectedgame = games.Where(item => item.GameCode == selectedgamecode).FirstOrDefault();
                    if (selectedgame != null)
                    {
                        // Print only the HomeBestDealsImage for the specific game
                        Debug.WriteLine($"Home Best Deals Image for {gameCode}: {selectedgame.HomeBestDealsImage}");
                        HomeBestDealsImage = selectedgame.HomeBestDealsImage;
                    }
                    else
                    {
                        Debug.WriteLine($"No game found with game code: {gameCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the API request
                    Debug.WriteLine($"Error fetching game details: {ex.Message}");
                }
            }
        }
       
    }
}




