using CardGameCorner.ViewModels;

namespace CardGameCorner.Views;

public partial class CardDetailPage : ContentPage
{
    public CardDetailPage()
    {
        InitializeComponent();
        BindingContext = new CardDetailViewModel();
    }
}