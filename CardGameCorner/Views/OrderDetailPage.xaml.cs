using CardGameCorner.ViewModels;
using System.Diagnostics;

namespace CardGameCorner.Views
{
    public partial class OrderDetailPage : ContentPage
    {
        private readonly OrderDetailViewModel _viewModel;

        public OrderDetailPage(OrderDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Debug.WriteLine($"OrderDetailPage.OnAppearing - OrderId: {_viewModel.OrderId}");

            // Force refresh the order details when the page appears
            if (_viewModel.OrderId > 0)
            {
                MainThread.BeginInvokeOnMainThread(async () => {
                    await _viewModel.LoadOrderDetailAsync();
                });
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Good practice to clean up when page disappears
            Debug.WriteLine("OrderDetailPage.OnDisappearing");
        }
    }
}