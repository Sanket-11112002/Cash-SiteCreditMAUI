using System.ComponentModel;
using System.Diagnostics;
using CardGameCorner.Resources.Language;
using System.Runtime.CompilerServices;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CardGameCorner.ViewModels
{
    [QueryProperty(nameof(OrderId), "orderId")]
    public partial class OrderDetailViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly IOrderService _orderService;
        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        [ObservableProperty]
        private int orderId;

        [ObservableProperty]
        private OrderDetail orderDetail;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage;

        // Localized text properties
        [ObservableProperty]
        private string orderDetailTitle;

        [ObservableProperty]
        private string orderInfoTitle;

        [ObservableProperty]
        private string orderIdField;

        [ObservableProperty]
        private string orderDateField;

        [ObservableProperty]
        private string statusField;

        [ObservableProperty]
        private string shippingDetailsTitle;

        [ObservableProperty]
        private string fNamefield;

        [ObservableProperty]
        private string lNamefield;

        [ObservableProperty]
        private string emailfield;

        [ObservableProperty]
        private string addressfield;

        [ObservableProperty]
        private string cityfield;

        [ObservableProperty]
        private string provincefield;

        [ObservableProperty]
        private string fiscalfield;

        [ObservableProperty]
        private string zipfield;

        [ObservableProperty]
        private string countryfield;

        [ObservableProperty]
        private string phonefield;

        [ObservableProperty]
        private string evaluationtitle;

        [ObservableProperty]
        private string firsteditiontitle;

        [ObservableProperty]
        private string pricetitle;

        [ObservableProperty]
        private string alteredbystafftitle;

        [ObservableProperty]
        private string quantitytitle;

        [ObservableProperty]
        private string confirmedtitle;

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public OrderDetailViewModel(IOrderService orderService)
        {
            _orderService = orderService;

            // Initialize with current language
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;
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
                OrderDetailTitle = AppResources.OrderDetails;
                OrderInfoTitle = AppResources.OrderInfo;
                OrderIdField = AppResources.OrderId;
                OrderDateField = AppResources.OrderDate;
                StatusField = AppResources.Status;
                ShippingDetailsTitle = AppResources.ShippingDetails;
                FNamefield = AppResources.FirstName;
                LNamefield = AppResources.Last_Name;
                Emailfield = AppResources.Email;
                Addressfield = AppResources.Address;
                Cityfield = AppResources.City;
                Provincefield = AppResources.Province;
                Fiscalfield = AppResources.Fiscal_Code;
                Zipfield = AppResources.ZIP;
                Countryfield = AppResources.Country;
                Phonefield = AppResources.Phone;
                Evaluationtitle = AppResources.Evalution;
                Firsteditiontitle = AppResources.Edition;
                Pricetitle = AppResources.Price;
                Alteredbystafftitle = AppResources.AlteredByStaff;
                Quantitytitle = AppResources.Quantity;
                Confirmedtitle = AppResources.Confirmed;
            });
        }

        partial void OnOrderIdChanged(int value)
        {
            if (value > 0)
            {
                // Don't use ConfigureAwait(false) in UI-related code
                MainThread.BeginInvokeOnMainThread(async () => {
                    await LoadOrderDetailAsync();
                });
            }
        }

        public async Task LoadOrderDetailAsync()
        {
            if (OrderId <= 0) return;

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                Debug.WriteLine($"Loading order details for OrderId: {OrderId}");

                var result = await _orderService.GetOrderDetailAsync(OrderId);

                if (result != null)
                {
                    Debug.WriteLine($"Order details loaded successfully. Name: {result.Name}, Cards Count: {result.Cards?.Count ?? 0}");
                    OrderDetail = result;
                }
                else
                {
                    Debug.WriteLine("Order details returned null");
                    ErrorMessage = "No order details found.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading order details: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ErrorMessage = $"Failed to load order details: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}