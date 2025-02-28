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




namespace CardGameCorner.ViewModels
{
    public partial class MyListViewModel : ObservableObject
    {
        // Observable collection to hold card items
        public ObservableCollection<ProductListViewModel> CardItems { get; set; } = new ObservableCollection<ProductListViewModel>();

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        [ObservableProperty]
        private string loginRequiredTitle;

        [ObservableProperty]
        private string loginRequiredMessage;

        private bool isOperationInProgress;

        [ObservableProperty]
        private string loginText;

        [ObservableProperty]
        private string continueText;

        [ObservableProperty]
        private string placeOrder;


        [ObservableProperty]
        private string cashTitle;

        [ObservableProperty]
        private string creditTitle;

        [ObservableProperty]
        private string quantityTitle;

        [ObservableProperty]
        private string pEdit;

        [ObservableProperty]
        private string pDelete;

        [ObservableProperty]
        private string deleteTitle;

        [ObservableProperty]
        private string deleteMsg;

        [ObservableProperty]
        private string yesMsg;

        [ObservableProperty]
        private string listEmptyTitle;

        [ObservableProperty]
        private string listEmptyMsg;

        [ObservableProperty]
        private string emptyListMessage;  // New property for empty list message

        [ObservableProperty]
        private bool showEmptyMessage;

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

            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

            CardItems = new ObservableCollection<ProductListViewModel>();
            NavigateToCardDetailCommand = new Command<ProductListViewModel>(NavigateToCardDetail);
            DeleteCardCommand = new Command<ProductListViewModel>(DeleteCard);
            getlist();
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
                PlaceOrder = AppResources.PlaceOrder;

                PEdit = AppResources.Edit;
                PDelete = AppResources.Delete;
                DeleteTitle = AppResources.DeleteTitle;
                DeleteMsg = AppResources.DeleteMsg;
                YesMsg = AppResources.YesMsg;
                ListEmptyTitle = AppResources.LoginRequiredTitle;
                ListEmptyMsg = AppResources.EmptyListMessage;
                CashTitle = AppResources.Cash;
                QuantityTitle = AppResources.Quantity;
                CreditTitle = AppResources.Site_credit;

                EmptyListMessage = AppResources.EmptyListMessage;

                OnPropertyChanged(nameof(PlaceOrder));
                OnPropertyChanged(nameof(LoginRequiredTitle));
                OnPropertyChanged(nameof(LoginRequiredMessage));

                OnPropertyChanged(nameof(EmptyListMessage));
                OnPropertyChanged(nameof(CashTitle));
                OnPropertyChanged(nameof(QuantityTitle));
                OnPropertyChanged(nameof(CreditTitle));
                OnPropertyChanged(nameof(PEdit));
                OnPropertyChanged(nameof(PDelete));
                OnPropertyChanged(nameof(DeleteTitle));
                OnPropertyChanged(nameof(DeleteMsg));
                OnPropertyChanged(nameof(YesMsg));
                OnPropertyChanged(nameof(ListEmptyTitle));
                OnPropertyChanged(nameof(ListEmptyMsg));
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
            if (isOperationInProgress) return;

            try
            {
                isOperationInProgress = true;

                if (selectedCard == null) return;
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    // UI updates or navigation
                });

                // Rest of your existing NavigateToCardDetail code...
                var details = await Task.Run(() =>
                {
                    return new List<CardDetailViewModel>
                {
                    new CardDetailViewModel
                    {
                        Id = selectedCard.Id ?? 0,
                        Name = selectedCard.Model,
                        Rarity = selectedCard.Rarity,
                        Category = selectedCard.Category,
                        ImageUrl = selectedCard.Image,
                        buyList = selectedCard.Buylist ?? 0,
                        siteCredit = selectedCard.Sitecredit ?? 0,
                        IsFirstEdition = selectedCard.IsFirstEdition ?? false,
                        IsReverse = selectedCard.IsReverse ?? false,
                        Game = selectedCard.Game,
                        Languages = selectedCard.Languages,
                        Conditions = selectedCard.Conditions,
                        SelectedLanguage = selectedCard.Language,
                        selectedCondition = selectedCard.Condition,
                        Quantity = selectedCard.Quantity ?? 0,
                        IsEditMode = true,
                        varinats=selectedCard.varinats
                    }
                };
                });

                var detailsJson = JsonConvert.SerializeObject(details);
                await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(detailsJson)}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error navigating to card detail: {e.Message}");
            }
            finally
            {
                isOperationInProgress = false;
            }
        }

        public async Task getlist()
        {
            try
            {
                // Validate user is logged in
                //if (!App.IsUserLoggedIn)
                //{
                //    throw new Exception("User is not logged in.");
                //}

                // Retrieve JWT token

                // Get items from database
                var myListService = new SQLiteService();
                // var items = await myListService.GetAllItemsAsync(username) ?? new List<ProductList>();
                var items = await myListService.GetAllItemsAsync() ?? new List<ProductList>();

                // Ensure CardItems is initialized
                if (CardItems == null)
                {
                    CardItems = new ObservableCollection<ProductListViewModel>();
                }
                CardItems.Clear();



                // Map items to CardItems
                foreach (var item in items)
                {
                    var languageFlag = item.Language?.ToLower() switch
                    {
                        "italiano" => "italianlngimage.png",
                        "english" => "gb.png",
                        _ => "gb.png" // Default image for unsupported or null values
                    };

                    var card = new ProductListViewModel
                    {
                        Id = item.Id ?? 0,
                        ProductId = item.ProductId ?? 0,
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
                     //   UserName = item.Username ?? string.Empty,
                        Language = item.Language ?? string.Empty,
                        Condition = item.Condition ?? string.Empty,
                        Buylist = item.Buylist ?? 0,
                        Quantity = item.Quantity ?? 0,
                        Languageflag = languageFlag,

                        Languages = !string.IsNullOrEmpty(item.Languagejsonlst)
                    ? new HashSet<string>(JsonConvert.DeserializeObject<List<string>>(item.Languagejsonlst)).ToList()
                    : new List<string>(),

                        Conditions = !string.IsNullOrEmpty(item.Conditionjsonlst)
                        ? new HashSet<string>(JsonConvert.DeserializeObject<List<string>>(item.Conditionjsonlst)).ToList()
                        : new List<string>(),
                    };

                    CardItems.Add(card);

                }
                ShowEmptyMessage = CardItems.Count == 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in getlist: {e.Message}");
            }
        }



        public async Task LoadDataAsync()
        {
            await getlist();
        }

        private async void DeleteCard(ProductListViewModel selectedCard)
        {
            if (isOperationInProgress) return;

            try
            {
                isOperationInProgress = true;

                if (selectedCard != null)
                {
                    bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
                        DeleteTitle,
                       DeleteMsg,
                        YesMsg,
                        "No"
                    );

                    if (isConfirmed)
                    {
                        var myListService = new SQLiteService();
                        var product = selectedCard.MapToProductList();
                        await myListService.DeleteItemAsync(product);
                        await getlist();

                        if (CardItems.Count == 0)
                        {
                            ShowEmptyMessage = true;

                            await Shell.Current.Navigation.PopToRootAsync();
                            await Shell.Current.GoToAsync("//SearchPage");
                        }
                    }
                }
            }
            finally
            {
                isOperationInProgress = false;
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

