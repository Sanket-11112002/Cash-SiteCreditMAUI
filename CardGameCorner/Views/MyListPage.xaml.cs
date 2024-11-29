
using CardGameCorner.Services;

namespace CardGameCorner.Views;

public partial class MyListPage : ContentPage
{

    public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;
    public MyListPage()
    {
        InitializeComponent();
    }

    //protected async override void OnAppearing()
    //{
    //    base.OnAppearing();


    //    if (GlobalSettings.SelectedGame != null)
    //    {
    //        InitializeComponent();

    //    }
    //    else
    //    {



    //        await Application.Current.MainPage.DisplayAlert("Error", "No game selected. Please select a game before accessing the search page.", "OK");
    //        await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack

    //        await Shell.Current.GoToAsync("//home");

    //    }
    //}

    private async void getlist(object sender, EventArgs e)
    {
        var myListService = new SQLiteService();
        var items = await myListService.GetAllItemsAsync();

        // Display the items (you can display in a ListView, for example)
        foreach (var item in items)
        {
            
        }

    }
}
