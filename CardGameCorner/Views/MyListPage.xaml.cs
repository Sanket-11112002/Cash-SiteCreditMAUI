using CardGameCorner.Services;
using CardGameCorner.ViewModels;

namespace CardGameCorner.Views;

public partial class MyListPage : ContentPage
{
    private readonly MyListViewModel _viewModel;
    public MyListPage()
    {
        InitializeComponent();
        _viewModel = new MyListViewModel();
        BindingContext = _viewModel;
    }
   

}