using CardGameCorner.ViewModels;

namespace CardGameCorner.Views;

public partial class CardComparisonPage : ContentPage
{
	public CardComparisonPage(CardComparisonViewModel viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }
}