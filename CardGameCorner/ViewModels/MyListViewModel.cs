using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.Views;
using CommunityToolkit.Mvvm.ComponentModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Microsoft.Maui.ApplicationModel.Permissions;




namespace CardGameCorner.ViewModels {
    public partial class MyListViewModel : ObservableObject
    {
        // Observable collection to hold card items
        public ObservableCollection<ProductListViewModel> CardItems { get; set; } = new ObservableCollection<ProductListViewModel>();

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        [ObservableProperty]
        private string loginRequiredTitle;

        [ObservableProperty]
        private string loginRequiredMessage;

        [ObservableProperty]
        private string loginText;

        [ObservableProperty]
        private string continueText;

        private readonly IScanCardService scanCardService;
        private ICommand _navigateToCardDetailCommand;
        private ICommand _deleteCardCommand;
        private bool _listvisibility;
        private bool _listvisibilitycards;


        public bool listvisibility
        {
            get => _listvisibility;
            set
            {
                _listvisibility = value;
                OnPropertyChanged();
            }
        }
        public bool listvisibilitycards
        {
            get => _listvisibilitycards;
            set
            {
                _listvisibilitycards = value;
                OnPropertyChanged();
            }
        }
        public ICommand NavigateToCardDetailCommand
        {
            get => _navigateToCardDetailCommand;
            set => SetProperty(ref _navigateToCardDetailCommand, value);
        }
        public ICommand DeleteCardCommand
        {
            get => _deleteCardCommand;
            set => SetProperty(ref _deleteCardCommand, value);
        }

        public MyListViewModel()
        {
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

            CardItems = new ObservableCollection<ProductListViewModel>();
            NavigateToCardDetailCommand = new Command<ProductListViewModel>(NavigateToCardDetail);
            DeleteCardCommand = new Command<ProductListViewModel>(DeleteCard);
            getlist(); // Get the card data asynchronously
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
                LoginRequiredTitle = AppResources.LoginRequiredTitle;
                LoginRequiredMessage = AppResources.LoginRequiredMessage;
                LoginText = AppResources.Login;
                ContinueText = AppResources.Continue;

                OnPropertyChanged(nameof(LoginRequiredTitle));
                OnPropertyChanged(nameof(LoginRequiredMessage));
                //OnPropertyChanged(nameof(ContinueText));
                //OnPropertyChanged(nameof(LoginText));
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Method to handle navigation to card details
        private async void NavigateToCardDetail(ProductListViewModel selectedCard)
        {
            var details = new List<CardDetailViewModel>();

            if (selectedCard == null) return;
            
            // Create a new instance of CardDetailViewModel and populate it with data from selectedCard
            var cardDetailViewModel = new CardDetailViewModel()
            {
                Id=selectedCard.Id ?? 0,
                Name = selectedCard.Model,
                Rarity = selectedCard.Rarity,
                Category = selectedCard.Category,
                ImageUrl = selectedCard.Image,
                buyList = selectedCard.Buylist ?? 0,
                siteCredit = selectedCard.Sitecredit ?? 0,
               IsFirstEdition = selectedCard.IsFirstEdition ?? false,
                //IsReverse = selectedCard.IsReverse,
                Game = selectedCard.Game,
                 Languages = selectedCard.Languages,
                Conditions = selectedCard.Conditions,
                SelectedLanguage = selectedCard.Language,
                selectedCondition = selectedCard.Condition,
              //  Buylist = selectedCard.Buylist?? 0,
                Quantity = selectedCard.Quantity ?? 0,
            };
            details.Add(cardDetailViewModel);
            // Pass the ViewModel instance as a navigation parameter
            //        var navigationParameter = new Dictionary<string, object>
            //{
            //    { "CardDetailViewModel", cardDetailViewModel }
            //};

            //   await Shell.Current.GoToAsync(nameof(CardDetailPage), navigationParameter);

            // await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(details));

            var detailsJson = JsonConvert.SerializeObject(details);  // Ensure you have 'Newtonsoft.Json' or other serializer for this


            // Navigate using GoToAsync with the serialized data as a query parameter
            await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(detailsJson)}");
        }

        public async Task getlist()
        {
            try
            {
                // Validate user is logged in
                if (!App.IsUserLoggedIn)
                {
                    throw new Exception("User is not logged in.");
                }

                // Retrieve JWT token
                var jwtToken = await SecureStorage.GetAsync("jwt_token");
                if (string.IsNullOrEmpty(jwtToken))
                {
                    throw new Exception("JWT token is missing in SecureStorage.");
                }

                // Decode username from token
                var username = DecodeJwtAndGetUsername(jwtToken);

                // Get items from database
                var myListService = new SQLiteService();
                var items = await myListService.GetAllItemsAsync(username) ?? new List<ProductList>();

                // Ensure CardItems is initialized
                if (CardItems == null)
                {
                    CardItems = new ObservableCollection<ProductListViewModel>();
                }
                CardItems.Clear();

                // Map items to CardItems
                foreach (var item in items)
                {
                    var card = new ProductListViewModel
                    {
                        Id = item.Id ?? 0,
                        Model = item.Model ?? string.Empty,
                        ModelEn = item.ModelEn ?? string.Empty,
                        Image = item.Image ?? string.Empty,
                        Color = item.Color ?? string.Empty,
                        Rarity = item.Rarity ?? string.Empty,
                        Category = item.Category ?? string.Empty,
                        Sitecredit = item.Sitecredit ?? 0,
                        IsFirstEdition = item.IsFirstEdition ?? false,
                        IsReverse = item.IsReverse ?? false,
                        Game = item.Game ?? string.Empty,
                        UserName = item.Username ?? string.Empty,
                        Language = item.Language ?? string.Empty,
                        Condition = item.Condition ?? string.Empty,
                        Buylist = item.Buylist ?? 0,
                        Quantity = item.Quantity ?? 0,
                        Languageflag = "italianlngimage.png",
                      
                                        Languages = !string.IsNullOrEmpty(item.Languagejsonlst)
                    ? new HashSet<string>(JsonConvert.DeserializeObject<List<string>>(item.Languagejsonlst)).ToList()
                    : new List<string>(),

                        Conditions = !string.IsNullOrEmpty(item.Conditionjsonlst)
    ? new HashSet<string>(JsonConvert.DeserializeObject<List<string>>(item.Conditionjsonlst)).ToList()
    : new List<string>(),
                    };

                    CardItems.Add(card);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in getlist: {e.Message}");
            }
        }

        //public async Task<List<ProductListViewModel>> getlist()
        //{
        //    var fetchedList = new List<ProductListViewModel>();
        //    try
        //    {
        //        var myListService = new SQLiteService();
        //        var items = await myListService.GetAllItemsAsync();

        //        if (items != null)
        //        {
        //            CardItems.Clear();
        //            foreach (var item in items)
        //            {
        //                var card = new ProductListViewModel
        //                {
        //                    Id = item.Id ?? 0,
        //                    Model = item.Model,
        //                    Rarity = item.Rarity,
        //                    Category = item.Category,
        //                    Image = item.Image,
        //                    Sitecredit = item.Sitecredit ?? 0,
        //                    IsFirstEdition = item.IsFirstEdition,
        //                    IsReverse = item.IsReverse,
        //                    Game = item.Game,
        //                    Language = item.Language,
        //                    Condition = item.Condition,
        //                    Buylist = item.Buylist ?? 0,
        //                    Quantity = item.Quantity ?? 0,
        //                    Languageflag = "italianlng.svg",
        //                    Languages = !string.IsNullOrEmpty(item.Languagejsonlst)
        //                        ? JsonConvert.DeserializeObject<List<string>>(item.Languagejsonlst)
        //                        : null,
        //                    Conditions = !string.IsNullOrEmpty(item.Conditionjsonlst)
        //                        ? JsonConvert.DeserializeObject<List<string>>(item.Conditionjsonlst)
        //                        : null
        //                };

        //                CardItems.Add(card);
        //                fetchedList.Add(card);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Error fetching card list: {e.Message}");
        //    }

        //    return fetchedList;
        //}

        public async Task LoadDataAsync()
        {
            await getlist();
        }

        private async void DeleteCard(ProductListViewModel selectedCard)
        {
            if (selectedCard != null)
            {
                // Show a confirmation dialog
                bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Delete", // Title of the alert
                    "Are you sure you want to delete this card?", // Message
                    "Yes", // Confirmation button
                    "No" // Cancel button
                );

                if (isConfirmed)
                {
                    var myListService = new SQLiteService();
                    var product = selectedCard.MapToProductList();

                    // Call your SQLite service to delete the item
                    await myListService.DeleteItemAsync(product);

                    // Refresh the list or perform necessary actions after deletion
                    await getlist();

                    if (CardItems.Count == 0)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Card List Empty",
                            "Your card list is empty.",
                            "OK"
                        );

                        await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack

                        await Shell.Current.GoToAsync("//SearchPage");
                    }

                    



                }
            }
        }

        public string DecodeJwtAndGetUsername(string jwtToken)
        {
            try
            {
                // Split the JWT into its three parts
                var parts = jwtToken.Split('.');
                if (parts.Length != 3)
                    throw new Exception("Invalid JWT format");

                // Decode the payload (second part of the JWT)
                var payload = parts[1];

                // Base64 decode the payload
                var jsonBytes = Convert.FromBase64String(PadBase64String(payload));
                var jsonString = System.Text.Encoding.UTF8.GetString(jsonBytes);

                // Parse the payload to get the username
                var payloadJson = JObject.Parse(jsonString);

                // Extract the username using the claim name
                var usernameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

                return payloadJson[usernameClaim]?.ToString() ?? throw new Exception("Username claim not found in the JWT");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decoding JWT: {ex.Message}");
                return null;
            }
        }

        // Helper method to pad Base64 strings properly
        private string PadBase64String(string base64)
        {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }

    }
}

