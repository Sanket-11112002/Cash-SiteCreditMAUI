using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CardGameCorner.Models
{
    public partial class OrderModel : ObservableObject
    {
        [ObservableProperty]
        private int orderId;

        [ObservableProperty]
        [JsonPropertyName("orderDate")]
        private DateTime orderDate;

        [ObservableProperty]
        private string game;

        [ObservableProperty]
        private string status;

        [ObservableProperty]
        private string payment;

        [ObservableProperty]
        [JsonPropertyName("paymentAccount")]
        private string paymentAccount;

        [ObservableProperty]
        private string contact;

        [ObservableProperty]
        private bool pickup;

        [ObservableProperty]
        [JsonPropertyName("pickupCost")]
        private decimal pickupCost;
    }
}