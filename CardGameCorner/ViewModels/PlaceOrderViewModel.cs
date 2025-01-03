
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CardGameCorner.ViewModels
{
    public partial class PlaceOrderViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly IListboxService _listboxService;
        private readonly HttpClient _httpClient;
        private const string API_URL = "https://api.magiccorner.it/api/mcBuylistNewOrder";

        public event PropertyChangedEventHandler PropertyChanged;
        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        private bool _isInitializing = false;
        private bool _suppressPropertyChange = false;

        [ObservableProperty]
        private string contactedl;

        [ObservableProperty]
        private string getPaidl;

        [ObservableProperty]
        private string staffMsgl;

        [ObservableProperty]
        private string clickHerel;

        [ObservableProperty]
        private string placeOrderTitle;

        [ObservableProperty]
        private string pStaffMsgl;

        [ObservableProperty]
        private string paypalEmaill;

        [ObservableProperty]
        private string pPaypalEmaill;

        [ObservableProperty]
        private string iBNcodel;

        [ObservableProperty]
        private string pIBNcodel;

        [ObservableProperty]
        private string placeOrder;
        private string _selectedContactInfo;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string error;

        [ObservableProperty]
        private string error_LoginRequired;

        [ObservableProperty]
        private string error_GameNotSelected;

        [ObservableProperty]
        private string error_NoCardsFound;

        [ObservableProperty]
        private string success_OrderPlaced;

        [ObservableProperty]
        private string success;

        [ObservableProperty]
        private string error_OrderFailed;

        public ICommand PlaceOrderCommand { get; }
        public string SelectedContactInfo
        {
            get => _selectedContactInfo;
            set
            {
                if (!_isInitializing) // Only update if not initializing
                {
                    _selectedContactInfo = value;
                    OnPropertyChanged(nameof(SelectedContactInfo));
                }
            }
        }

        private string _selectedPaymentOption;
        public string SelectedPaymentOption
        {
            get => _selectedPaymentOption;
            set
            {
                if (!_isInitializing) // Only update if not initializing
                {
                    _selectedPaymentOption = value;
                    OnPropertyChanged(nameof(SelectedPaymentOption));
                    UpdateConditionalFields();
                }
            }
        }

        private string _ibanCode;
        public string IBANCode
        {
            get => _ibanCode;
            set
            {
                _ibanCode = value;
                OnPropertyChanged(nameof(IBANCode));
            }
        }

        private string _paypalEmail;
        public string PayPalEmail
        {
            get => _paypalEmail;
            set
            {
                _paypalEmail = value;
                OnPropertyChanged(nameof(PayPalEmail));
            }
        }

        private bool _showIBANField;
        public bool ShowIBANField
        {
            get => _showIBANField;
            set
            {
                _showIBANField = value;
                OnPropertyChanged(nameof(ShowIBANField));
            }
        }

        private bool _showPayPalField;
        public bool ShowPayPalField
        {
            get => _showPayPalField;
            set
            {
                _showPayPalField = value;
                OnPropertyChanged(nameof(ShowPayPalField));
            }
        }

        private string _staffMessage;
        public string StaffMessage
        {
            get => _staffMessage;
            set
            {
                _staffMessage = value;
                OnPropertyChanged(nameof(StaffMessage));
            }
        }


        public ObservableCollection<string> ContactInfo { get; set; }
        public ObservableCollection<string> PaymentMethod { get; set; }

        public PlaceOrderViewModel()
        {
            UpdateLocalizedStrings();

            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

            _httpClient = new HttpClient();
            PlaceOrderCommand = new Command(async () => await ExecutePlaceOrder());

            _listboxService = new ListBoxService();
            ContactInfo = new ObservableCollection<string>();
            PaymentMethod = new ObservableCollection<string>();

           // InitializeDropdowns();
        }

        private async Task ExecutePlaceOrder()
        {
            try
            {
                IsLoading = true;

                // Get JWT token and verify
                var token = await SecureStorage.GetAsync("jwt_token");
                if (string.IsNullOrEmpty(token))
                {
                    await Application.Current.MainPage.DisplayAlert(Error, Error_LoginRequired, "OK");
                    return;
                }

                // Decode JWT and get username
               // var username = DecodeJwtAndGetUsername(token);
                //if (string.IsNullOrEmpty(username))
                //{
                //    await Application.Current.MainPage.DisplayAlert(Error, "Invalid authentication token", "OK");
                //    return;
                //}

                // Get selected game from global settings
                var selectedGame = GlobalSettings.SelectedGame?.ToLower();
                if (string.IsNullOrEmpty(selectedGame))
                {
                    await Application.Current.MainPage.DisplayAlert(Error, Error_GameNotSelected, "OK");
                    return;
                }

                // Get cards from SQLite with game and username filter
                var sqliteService = new SQLiteService();
                var allCards = await sqliteService.GetAllItemsAsync();

                // Filter cards by game and username
                var filteredCards = allCards
                    .Where(card =>
                        card.Game?.ToLower() == selectedGame)
                        // && card.Username == username)
                    .ToList();

                if (!filteredCards.Any())
                {
                    await Application.Current.MainPage.DisplayAlert(
                       Error,
                        $"{Error_NoCardsFound} {selectedGame}",
                        "OK"
                    );
                    return;
                }

                var orderRequest = new
                {
                    UIc = GlobalSettings.SelectedLanguage.ToLower() == "italian" ? "it" : "en",
                    contactType = GetApiContactTypeValue(SelectedContactInfo),
                    paymentType = GetApiPaymentTypeValue(SelectedPaymentOption),
                    paymentAccount = ShowPayPalField ? PayPalEmail : IBANCode,
                    message = StaffMsgl,
                    game = selectedGame,
                    Cards = filteredCards.Select(card => new
                    {
                        idProduct = card.ProductId,  // Using ProductId instead of Id
                        language = card.Language,
                        condition = card.Condition,
                        foil = card.IsFoil ?? card.IsReverse,  // Using IsFoil with fallback to IsReverse
                        firstEdition = card.IsFirstEdition,
                        qty = card.Quantity,
                        buylist = card.Buylist,
                        credit = card.Sitecredit,
                        evaluation = card.Evalution ?? false
                    }).ToList()
                };

                // Set up request
                var request = new HttpRequestMessage(HttpMethod.Post, API_URL);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(orderRequest),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                // Send request
                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<OrderResponse>(content);

                    // Delete only the ordered cards
                    foreach (var card in filteredCards)
                    {
                        await sqliteService.DeleteItemAsync(card);
                    }

                    await Application.Current.MainPage.DisplayAlert(
                        Success,
                        $"{result.OrderId} {Success_OrderPlaced}",
                        "OK"
                    );

                    await Shell.Current.GoToAsync("//MyListPage");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Error,
                        Error_OrderFailed,
                        "OK"
                    );
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Error,
                    "An error occurred while placing the order.",
                    "OK"
                );
                Console.WriteLine($"Order placement error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private int GetApiContactTypeValue(string selectedContact)
        {
            if (GlobalSettings.SelectedLanguage == "Italian")
            {
                return selectedContact switch
                {
                    "Non contattarmi" => 0,
                    "Si, contattami" => 1,
                    _ => 0
                };
            }

            return selectedContact switch
            {
                "Do not contact me" => 0,
                "Yes, please contact me" => 1,
                _ => 0
            };
        }

        // Helper method to get the correct API payment type value
        private int GetApiPaymentTypeValue(string selectedPayment)
        {
            if (GlobalSettings.SelectedLanguage == "Italian")
            {
                return selectedPayment switch
                {
                    "Credito per acquisti sul sito" => 0,
                    "Bonifico bancario" => 1,
                    "Trasferimento PayPal" => 2,
                    _ => 0
                };
            }

            return selectedPayment switch
            {
                "Credit for purchases on the website" => 0,
                "Cash (Wire transfer)" => 1,
                "Paypal money transfer" => 2,
                _ => 0
            };
        }

        //private string DecodeJwtAndGetUsername(string jwtToken)
        //{
        //    try
        //    {
        //        var parts = jwtToken.Split('.');
        //        if (parts.Length != 3)
        //            throw new Exception("Invalid JWT format");

        //        var payload = parts[1];
        //        var jsonBytes = Convert.FromBase64String(PadBase64String(payload));
        //        var jsonString = System.Text.Encoding.UTF8.GetString(jsonBytes);
        //        var payloadJson = JObject.Parse(jsonString);

        //        // Extract the username using the claim name
        //        var usernameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        //        return payloadJson[usernameClaim]?.ToString() ?? throw new Exception("Username claim not found in JWT");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error decoding JWT: {ex.Message}");
        //        return null;
        //    }
        //}

        private string PadBase64String(string base64)
        {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }

        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
            {
                // Update localized strings when language changes
                UpdateLocalizedStrings();
                // Reinitialize dropdowns when language changes
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(100); // Small delay to prevent immediate dropdown opening
                    InitializeDropdowns();
                });
            }
        }

        private void UpdateLocalizedStrings()
        {
            // Ensure these are called on the main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                GetPaidl = AppResources.GetPaid;
                Contactedl = AppResources.Contacted;
                PaypalEmaill = AppResources.PaypalEmail;
                IBNcodel = AppResources.IBNCode;
                ClickHerel = AppResources.ClickHere;
                StaffMsgl = AppResources.StaffMsg;
                PIBNcodel = AppResources.PIBNCode;
                PPaypalEmaill = AppResources.PPaypalEmail;
                PStaffMsgl = AppResources.PStaffMsg;
                PlaceOrderTitle = AppResources.PlaceOrder;
                Error = AppResources.ErrorTitle;
                Error_LoginRequired = AppResources.Error_LoginRequired;
                Error_GameNotSelected = AppResources.Error_GameNotSelected;
                Error_NoCardsFound = AppResources.Error_NoCardsFound;
                Success = AppResources.Success;
                success_OrderPlaced = AppResources.Success_OrderPlaced;
                Error_OrderFailed = AppResources.Error_OrderFailed;

                OnPropertyChanged(nameof(GetPaidl));
                OnPropertyChanged(nameof(Contactedl));
                OnPropertyChanged(nameof(PaypalEmaill));
                OnPropertyChanged(nameof(IBNcodel));
                OnPropertyChanged(nameof(ClickHerel));
                OnPropertyChanged(nameof(PStaffMsgl));
                OnPropertyChanged(nameof(StaffMsgl));
                OnPropertyChanged(nameof(PIBNcodel));
                OnPropertyChanged(nameof(PPaypalEmaill));
                OnPropertyChanged(nameof(PlaceOrderTitle));
                OnPropertyChanged(nameof(Error));
                OnPropertyChanged(nameof(Error_LoginRequired));
                OnPropertyChanged(nameof(Error_GameNotSelected));
                OnPropertyChanged(nameof(Error_NoCardsFound));
                OnPropertyChanged(nameof(Success));
                OnPropertyChanged(nameof(Success_OrderPlaced));
                OnPropertyChanged(nameof(Error_OrderFailed));
            });
        }
        public async void InitializeDropdowns()
        {
            try
            {
                _isInitializing = true; // Set flag before initialization

                // Clear existing items
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ContactInfo.Clear();
                    PaymentMethod.Clear();
                });

                // Fetch API values
                var listboxes = await _listboxService.GetPlaceOrderDetailsAsync();

                var contactType = listboxes?.FirstOrDefault(lb => lb.Filter == "contactType");
                var paymentType = listboxes?.FirstOrDefault(lb => lb.Filter == "paymentType");

                // Populate ContactInfo
                if (contactType?.Options != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var option in contactType.Options)
                        {
                            if (!ContactInfo.Contains(option.Name))
                            {
                                ContactInfo.Add(option.Name);
                            }
                        }
                        _selectedContactInfo = ContactInfo.FirstOrDefault(); // Directly set the backing field
                        OnPropertyChanged(nameof(SelectedContactInfo));
                    });
                }

                // Populate PaymentMethod
                if (paymentType?.Options != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var option in paymentType.Options)
                        {
                            if (!PaymentMethod.Contains(option.Name))
                            {
                                PaymentMethod.Add(option.Name);
                            }
                        }
                        _selectedPaymentOption = PaymentMethod.FirstOrDefault(); // Directly set the backing field
                        OnPropertyChanged(nameof(SelectedPaymentOption));
                    });
                }

                UpdateConditionalFields();
            }
            finally
            {
                _isInitializing = false; // Reset flag after initialization
            }
        }


        private void UpdateConditionalFields()
        {
            if (GlobalSettings.SelectedLanguage == "Italian")
            {
                ShowIBANField = SelectedPaymentOption == "Pagamento in denaro (Bonifico)";
                ShowPayPalField = SelectedPaymentOption == "Pagamento PayPal";
            }
            else
            {
                ShowIBANField = SelectedPaymentOption == "Cash (Wire transfer)";
                ShowPayPalField = SelectedPaymentOption == "Paypal money transfer";
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

