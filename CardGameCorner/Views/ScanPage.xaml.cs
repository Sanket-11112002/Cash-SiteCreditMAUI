using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;

namespace CardGameCorner.Views
{
    public partial class ScanPage : ContentPage
    {
        private readonly IScanCardService _scanCardService;
        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        private string LastSelectedGame;
        private bool IsToolbarNavigation = false;
        public ScanPage()
        {
            InitializeComponent();
            _scanCardService = new ScanCardService();
            BindingContext = new ScanCardViewModel(_scanCardService);
            LastSelectedGame = GlobalSettings.SelectedGame;
        }

        private async Task CapturePhotoAsync()
        {
            var viewModel = BindingContext as ScanCardViewModel;
            if (viewModel == null) return;

            try
            {
                viewModel.IsLoading = true;
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo != null)
                {
                    using var originalStream = await photo.OpenReadAsync();
                    var compressedImageStream = await viewModel.CompressImageAsync(originalStream, 100 * 1024);

                    if (compressedImageStream != null)
                    {
                        var displayStream = new MemoryStream();
                        compressedImageStream.Position = 0;
                        await compressedImageStream.CopyToAsync(displayStream);
                        displayStream.Position = 0;

                        capturedImage.Source = ImageSource.FromStream(() => displayStream);
                        capturedImage.IsVisible = true;

                        var uploadStream = new MemoryStream();
                        compressedImageStream.Position = 0;
                        await compressedImageStream.CopyToAsync(uploadStream);
                        uploadStream.Position = 0;

                        var apiResponse = await viewModel.UploadImageAsync(uploadStream);
                        if (apiResponse != null)
                        {
                            var cardRequest = new CardSearchRequest
                                {
                                    //Title = apiResponse.Result.Title,
                                    //Set = apiResponse.Result.Set,
                                    //Game = GlobalSettings.SelectedGame,
                                    //Lang = apiResponse.Result.Lang,
                                    //Foil = apiResponse.Result.Foil,
                                    //FirstEdition = 0

                                    Title = "Angel of Mercy",
                                    Game = "magic",
                                    Set = "IMA",
                                    Lang = "en",
                                    Foil = 0,
                                    FirstEdition = 0

                                };

                            var data = await viewModel.SearchCardAsync(cardRequest,
                      ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));

                            if (data != null)
                            {
                                await Navigation.PushAsync(new CardComparisonPage(data));
                            }
                            else
                            {
                                await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                viewModel.IsLoading = false;
            }
        }
        private async void OnCaptureButtonClicked(object sender, EventArgs e)
        {
            await CapturePhotoAsync();
        }

        private async void OnUploadButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var viewModel = BindingContext as ScanCardViewModel;
                viewModel.IsLoading = true;

                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using var originalStream = await result.OpenReadAsync();

                    var compressedImageStream = await viewModel.CompressImageAsync(originalStream, 100 * 1024);

                    if (compressedImageStream != null)
                    {
                        var displayStream = new MemoryStream();
                        compressedImageStream.Position = 0;
                        await compressedImageStream.CopyToAsync(displayStream);
                        displayStream.Position = 0;

                        capturedImage.Source = ImageSource.FromStream(() => displayStream);
                        capturedImage.IsVisible = true;

                        var uploadStream = new MemoryStream();
                        compressedImageStream.Position = 0;
                        await compressedImageStream.CopyToAsync(uploadStream);
                        uploadStream.Position = 0;

                        var apiResponse = await viewModel.UploadImageAsync(uploadStream);

                        if (apiResponse != null)
                        {


                            var cardRequest = new CardSearchRequest
                            {
                                Title = apiResponse.Result.Title,
                                Set = apiResponse.Result.Set,
                                Game = GlobalSettings.SelectedGame,
                                Lang = apiResponse.Result.Lang,
                                Foil = apiResponse.Result.Foil,
                                FirstEdition = 0
                            };

                            var data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));

                            if (data != null)
                            {
                                await Navigation.PushAsync(new CardComparisonPage(data));
                            }
                            else
                            {
                                await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Cancelled", "No image selected.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to select or upload image: {ex.Message}", "OK");
            }
            finally
            {
                var viewModel = BindingContext as ScanCardViewModel;
                viewModel.IsLoading = false;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Reset image and visibility when returning to the page
          capturedImage.Source = null;
          capturedImage.IsVisible = false;

            // Automatically trigger photo capture
            //if(LastSelectedGame == GlobalSettings.SelectedGame)
            //{
            //    await CapturePhotoAsync();
            //}
           
        }

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    LastSelectedGame = GlobalSettings.SelectedGame;
        //}

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            LastSelectedGame = GlobalSettings.SelectedGame;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Clear the Shell navigation stack
                if (Shell.Current != null)
                {
                    await Shell.Current.Navigation.PopToRootAsync(false);
                }
            });

            
        }

        protected override bool OnBackButtonPressed()
        {

            Task<bool> answer = DisplayAlert(AppResources.Exit, AppResources.ExitApp, AppResources.YesMsg, "No");
            answer.ContinueWith(task =>
            {
                if (task.Result)
                {
                    Application.Current.Quit();
                }
            });
            return true;
        }
    }
}