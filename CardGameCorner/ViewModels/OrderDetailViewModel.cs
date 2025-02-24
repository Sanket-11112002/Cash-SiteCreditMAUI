using System.Diagnostics;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CardGameCorner.ViewModels
{
    [QueryProperty(nameof(OrderId), "orderId")]
    public partial class OrderDetailViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;

        [ObservableProperty]
        private int orderId;

        [ObservableProperty]
        private OrderDetail orderDetail;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage;

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public OrderDetailViewModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        partial void OnOrderIdChanged(int value)
        {
            if (value > 0)
            {
                LoadOrderDetailAsync().ConfigureAwait(false);
            }
        }

        private async Task LoadOrderDetailAsync()
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