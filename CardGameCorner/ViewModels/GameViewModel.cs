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
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.ViewModels
{
    public class GameDetailsViewModel : BaseViewModel,INotifyPropertyChanged

    {
        private readonly ISecureStorage secureStorage;
      
        private ObservableCollection<Card> _cards;
        private ObservableCollection<Banner1> _banners;
        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

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
                 // OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the API request
                    Debug.WriteLine($"Error fetching game details: {ex.Message}");
                }
            }
        }

        public ICommand OpenProductUrlCommand { get; }



        public GameDetailsViewModel()
        {
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
    }


}
