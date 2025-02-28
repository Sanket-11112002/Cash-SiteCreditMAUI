using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualStudio.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.ViewModels;

public partial class MyOrdersViewModel : ObservableObject, INotifyPropertyChanged
{
    private readonly INavigationService _navigationService;
    public ICommand OrderTappedCommand { get; }

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISecureStorage _secureStorage;
    public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

    [ObservableProperty]
    private ObservableCollection<OrderModel> orders;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string errorMessage;
    [ObservableProperty]
    private string loginRequiredTitle;

    [ObservableProperty]
    private string loginRequiredMessage;

    [ObservableProperty]
    private string loginText;

    [ObservableProperty]
    private string continueText;

    [ObservableProperty]
    private bool showEmptyMessage;

    [ObservableProperty]
    private string orderTitle;

    [ObservableProperty]
    private string orderEmptyTitle;

    [ObservableProperty]
    private string orderIdField;

    [ObservableProperty]
    private string dateField;

    [ObservableProperty]
    private string gameField;
    [ObservableProperty]
    private string statusField;

    [ObservableProperty]
    private string paymentField;

    [ObservableProperty]
    private string paymentAccountField;

    [ObservableProperty]
    private string contactField;
    [ObservableProperty]
    private string pickupField;

    [ObservableProperty]
    private string pickupCostField;

    [ObservableProperty]
    private string emptyMessage = "You do not have any orders!";

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasOrders => Orders?.Count > 0;

    public MyOrdersViewModel(IHttpClientFactory httpClientFactory, ISecureStorage secureStorage, INavigationService navigationService)
    {
        UpdateLocalizedStrings();
        _httpClientFactory = httpClientFactory;
        _secureStorage = secureStorage;
        _navigationService = navigationService;
        GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;
        OrderTappedCommand = new Command<int>(OnOrderTapped);
        Orders = new ObservableCollection<OrderModel>();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

            OrderTitle= AppResources.OrderTitle;
            OrderEmptyTitle = AppResources.OrderEmpty;
            OrderIdField = AppResources.OrderId;
            DateField = AppResources.Date;
            GameField = AppResources.Game;
            StatusField = AppResources.Status;
            PaymentField = AppResources.Payment;
            PaymentAccountField = AppResources.Payment_Account;
            ContactField = AppResources.Contact;
            PickupField = AppResources.Pickup;
            PickupCostField = AppResources.Pickup_Cost;

            OnPropertyChanged(nameof(LoginRequiredTitle));
            OnPropertyChanged(nameof(LoginRequiredMessage));
            OnPropertyChanged(nameof(ContinueText));
            OnPropertyChanged(nameof(LoginText));

            OnPropertyChanged(nameof(OrderTitle));
            OnPropertyChanged(nameof(OrderEmptyTitle));
            OnPropertyChanged(nameof(OrderIdField));
            OnPropertyChanged(nameof(DateField));
            OnPropertyChanged(nameof(GameField));
            OnPropertyChanged(nameof(StatusField));
            OnPropertyChanged(nameof(PaymentField));
            OnPropertyChanged(nameof(PaymentAccountField));
            OnPropertyChanged(nameof(ContactField));
            OnPropertyChanged(nameof(PickupField));
            OnPropertyChanged(nameof(PickupCostField));
            
        });
    }
    private async void OnOrderTapped(int orderId)
    {
        if (orderId > 0)
        {
            await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}?orderId={orderId}");
        }
    }

    public async Task LoadOrdersAsync()
    {
        try
        {
            IsLoading = true;
            OnPropertyChanged(nameof(IsLoading)); 

            ErrorMessage = string.Empty;
            ShowEmptyMessage = false;

            var token = await _secureStorage.GetAsync("jwt_token");
            if (string.IsNullOrEmpty(token))
            {
                ErrorMessage = "Not authorized. Please log in.";
                return;
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://api.magiccorner.it/api/mcBuylistMyOrders");

            if (response.IsSuccessStatusCode)
            {
                var ordersList = await response.Content.ReadFromJsonAsync<List<OrderModel>>();
                Orders.Clear();

                if (ordersList != null && ordersList.Any())
                {
                    foreach (var order in ordersList)
                    {
                        Orders.Add(order);
                    }
                    ShowEmptyMessage = false;
                }
                else
                {
                    ShowEmptyMessage = true;
                }
            }
            else
            {
                ErrorMessage = "Failed to load orders. Please try again.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "An error occurred while loading orders.";
            Debug.WriteLine($"Error loading orders: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(IsLoading));
            OnPropertyChanged(nameof(HasError));
            OnPropertyChanged(nameof(HasOrders));
        }
    }
}