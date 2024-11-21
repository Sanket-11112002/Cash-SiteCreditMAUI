
namespace CardGameCorner.Views;

public partial class ScanPage : ContentPage
{
	public ScanPage()
	{
		InitializeComponent();


	}
    private void My_Camera_captured(object sender, CommunityToolkit.Maui.Views.MediaCapturedEventArgs e)
    {
        // Handle detected barcode
        MyImage.Source = ImageSource.FromStream(() => e.Media);


    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Permission Denied", "Camera permission is required.", "OK");
            await Navigation.PopAsync(); // Navigate back if permission is not granted
        }
    }

}