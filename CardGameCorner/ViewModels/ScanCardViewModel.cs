//using CardGameCorner.Models;
//using CardGameCorner.Services;
//using CommunityToolkit.Maui.Views;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;


//namespace CardGameCorner.Views
//{
//    public partial class ScanCardViewModel : ObservableObject
//    {
//        private readonly IScanCardService _scanCardService;

//        [ObservableProperty]
//        private ImageSource capturedImageSource;

//        [ObservableProperty]
//        private bool isImageVisible;

//        [ObservableProperty]
//        private string apiResponse;

//        public ScanCardViewModel(IScanCardService scanCardService)
//        {
//            _scanCardService = scanCardService;
//        }

//        [RelayCommand]
//        public async Task HandleImageCaptured(MediaCapturedEventArgs e)
//        {
//            if (e?.Media == null) return;

//            try
//            {
//                // Convert stream to byte array
//                using var memoryStream = new MemoryStream();
//                await e.Media.CopyToAsync(memoryStream);
//                byte[] imageData = memoryStream.ToArray();

//                // Update UI with captured image
//                capturedImageSource = ImageSource.FromStream(() => new MemoryStream(imageData));
//                isImageVisible = true;

//                // Upload to API
//                apiResponse = await _scanCardService.UploadImageAsync(imageData);

//                await Application.Current.MainPage.DisplayAlert("Success", "Image processed successfully!", "OK");
//            }
//            catch (Exception ex)
//            {
//                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to process image: {ex.Message}", "OK");
//            }
//        }
//    }
//}

using CardGameCorner.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CardGameCorner.ViewModels
{
    public partial class ScanCardViewModel : ObservableObject
    {
        private readonly IScanCardService _scancardServcie;

        [ObservableProperty]
        private ImageSource capturedImageSource;

        [ObservableProperty]
        private bool isImageVisible;

        public ScanCardViewModel(IScanCardService scancardServcie)
        {
            _scancardServcie = scancardServcie;
            IsImageVisible = false;
        }

        [RelayCommand]
        public async Task CaptureImage(MediaCapturedEventArgs e)
        {
            try
            {
                if (e.Media != null)
                {
                    CapturedImageSource = ImageSource.FromStream(() => e.Media);
                    IsImageVisible = true;

                    // Upload image to API
                    await _scancardServcie.UploadImageAsync(e.Media);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to process image: {ex.Message}", "OK");
            }
        }
    }
}
