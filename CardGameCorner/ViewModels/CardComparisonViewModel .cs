using System.ComponentModel;
using System.Runtime.CompilerServices;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;


namespace CardGameCorner.ViewModels
{
    public partial class CardComparisonViewModel : ObservableObject
    {
        private readonly IScanCardService scanservice;

        [ObservableProperty]
        private string cardName;

        [ObservableProperty]
        private string cardRarity;

        [ObservableProperty]
        private string cardSet;

        [ObservableProperty]
        private string searchResultImage;


        [ObservableProperty]
        public ImageSource scannedImage;


        [ObservableProperty]
        public CardSearchResponseViewModel responseContent;

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        [ObservableProperty]
        private string yourPicture;


        [ObservableProperty]
        private string searchResult;

        [ObservableProperty]
        private string cardComfiredMsg;

        [ObservableProperty]
        private string yesAddList;

        [ObservableProperty]
        private string noTryAgain;

        [ObservableProperty]
        private string cardComaprision;

        [ObservableProperty]
        private string success;

        [ObservableProperty]
        private string successmsg;

        [ObservableProperty]
        private string gameImageUrl;

        [ObservableProperty]
        private int language;

        [ObservableProperty]
        private int condition;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (SetProperty(ref _isLoading, value))
                {
                    OnPropertyChanged(nameof(IsNotLoading));
                }
            }
        }
        public bool IsNotLoading => !IsLoading;


        //public CardComparisonViewModel()
        //{
        //    // Initialize default values
        //    CardName = "Blue-Eyes White Dragon";
        //    CardRarity = "Secret Rare";
        //    CardSet = "RAGE OF THE ABYSS";

        //}
        //public CardComparisonViewModel(ApiResponse_Card responseContent, ImageSource scannedImage)
        //{
        //    // Assign the passed data to properties
        //    ResponseContent = responseContent;
        //    ScannedImage = scannedImage;

        //    // Map data from the response to individual properties
        //    if (ResponseContent?.Result != null)
        //    {
        //        CardName = ResponseContent.Result.Title;
        //        CardRarity = ResponseContent.Result.Rarity;
        //        CardSet = ResponseContent.Result.Set;
        //    }
        //}

        public CardComparisonViewModel()
        {
            // Initialize with current language
            UpdateLocalizedStrings();
            UpdateGameImage();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

        }

        // New method to handle property changes
        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
            {
                UpdateLocalizedStrings();
              //  UpdateGameImage();
            }
        }

        private void UpdateGameImage()
        {
            switch (GlobalSettings.SelectedGame)
            {
                case "pokemon":
                    GameImageUrl = "https://api.magiccorner.it/12/public/assets/app/pokemon/pokemon.png";
                    break;
                case "onepiece":
                    GameImageUrl = "https://api.magiccorner.it/12/public/assets/app/onepiece/onepiece.png";
                    break;
                case "magic":
                    GameImageUrl = "https://api.magiccorner.it/12/public/assets/app/magic/magic.png";
                    break;
                case "yugioh":
                    GameImageUrl = "https://api.magiccorner.it/12/public/assets/app/yugioh/yugioh.png";
                    break;
                default:
                    GameImageUrl = "banner.jpg";
                    break;
            }
        }

        private void UpdateLocalizedStrings()
        {
            // Ensure these are called on the main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                YourPicture = AppResources.Your_Picture;
                SearchResult = AppResources.Search_Result;
                CardComfiredMsg = AppResources.Is_this_Your_Card_;
                YesAddList = AppResources.Yes__Add_to_my_list;
                NoTryAgain = AppResources.No__I_will_try_again;
                CardComaprision = AppResources.Card_Comparision;
                Success = AppResources.Success;
                Successmsg = AppResources.successmsg;

                // Trigger property changed events to update UI
                OnPropertyChanged(nameof(YourPicture));
                OnPropertyChanged(nameof(SearchResult));
                OnPropertyChanged(nameof(CardComfiredMsg));
                OnPropertyChanged(nameof(YesAddList));
                OnPropertyChanged(nameof(NoTryAgain));
                OnPropertyChanged(nameof(CardComaprision));
                OnPropertyChanged(nameof(Success));
                OnPropertyChanged(nameof(Successmsg));
            });
        }

        // Ensure the PropertyChanged event is properly implemented
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public void Initialize(ApiResponse_Card response, ImageSource image)
        //{
        //    ResponseContent = response;
        //    ScannedImage = image;

        //    if (ResponseContent?.Result != null)
        //    {
        //        CardName = ResponseContent.Result.Title;
        //        CardRarity = ResponseContent.Result.Rarity;
        //        CardSet = ResponseContent.Result.Set;


        //    }
        //}

        //public void Initialize(CardSearchResponseViewModel response, ImageSource image)
        //{
        //    responseContent = response;
        //    scannedImage = image;

        //    if (responseContent?.Products[0] != null)
        //    {
        //        CardName = responseContent.Products[0].Model;
        //        CardRarity = responseContent.Products[0].Rarity;
        //        CardSet = responseContent.Products[0].Category;

        //        searchResultImage= "https://www.cardgamecorner.com"+responseContent.Products[0].Image;

        //    }


        //}

        public void Initialize(CardSearchResponseViewModel response, ImageSource image)
        {
            responseContent = response;
            scannedImage = image;

            // Iterate through all products in the response
            if (responseContent?.Products != null && responseContent.Products.Count > 0)
            {
                foreach (var product in responseContent.Products)
                {
                    // Set card information based on the first product (you can adjust this as needed)
                    CardName = product.Model;
                    CardRarity = product.Rarity;
                    CardSet = product.Category;

                    // Assuming you want the first image if there are multiple images
                    searchResultImage = "https://www.cardgamecorner.com" + product.Image;
                    language = product.Language;
                    Condition = product.Condition;

                }
            }
        }

        //[RelayCommand]
        //private async Task CaptureImage()
        //{
        //    try
        //    {
        //        var result = await MediaPicker.CapturePhotoAsync();
        //        if (result != null)
        //        {
        //            // Handle the captured image
        //            var stream = await result.OpenReadAsync();
        //            // Send to your card scanning service
        //            await ProcessCapturedImage(stream);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error",
        //            $"Failed to capture image: {ex.Message}", "OK");
        //    }
        //}

        //[RelayCommand]
        //private async Task ConfirmCard()
        //{
        //    try
        //    {
        //        // Ensure the responseContent is not null
        //        if (responseContent?.Products?.Count > 0)
        //        {
        //            // Get the first product
        //            var product = responseContent.Products[0];

        //            // Deserialize the Variants JSON string into a list of ProductVariant1 objects
        //            var variants = product.Variants;

        //            if (variants != null)
        //            {
        //                // Extract distinct languages and conditions
        //                var distinctLanguages = variants.Select(v => v.Language).Distinct().ToList();
        //                var distinctConditions = variants.Select(v => v.Condition).Distinct().ToList();

        //                // For testing, log the results to the console
        //                Console.WriteLine("Languages:");
        //                foreach (var lang in distinctLanguages)
        //                {
        //                    Console.WriteLine(lang);
        //                }

        //                Console.WriteLine("Conditions:");
        //                foreach (var condition in distinctConditions)
        //                {
        //                    Console.WriteLine(condition);
        //                }

        //                var details = new CardDetailViewModel
        //                {
        //                    Languages = distinctLanguages,
        //                    Conditions = distinctConditions,
        //                    Name = product.Model,
        //                    Rarity = product.Rarity,
        //                    Category = product.Category,
        //                    ImageUrl = "https://www.cardgamecorner.com" + product.Image,
        //                    game = product.Game
        //                };

        //                // Navigate to the CardDetailsPage


        //                await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(details));




        //            }

        //            // Show success confirmation
        //            await Application.Current.MainPage.DisplayAlert("Success",
        //                "Card added to your collection!", "OK");
        //        }
        //        else
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error",
        //                "No product data found in the response!", "OK");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle errors gracefully
        //        await Application.Current.MainPage.DisplayAlert("Error",
        //            $"An error occurred: {ex.Message}", "OK");
        //    }
        //}

        //[RelayCommand]
        //private async Task ConfirmCard()
        //{
        //    try
        //    {
        //        var detaillst = new List<CardDetailViewModel>();
        //        // Ensure the responseContent is not null
        //        if (responseContent?.Products?.Count > 0)
        //        {
        //            // Iterate through all products in the list
        //            foreach (var product in responseContent.Products)
        //            {
        //                // Deserialize the Variants JSON string into a list of ProductVariant1 objects
        //                var variants = product.Variants;

        //                if (variants != null)
        //                {
        //                    // Extract distinct languages and conditions
        //                    var distinctLanguages = variants.Select(v => v.Language).Distinct().ToList();
        //                    var distinctConditions = variants.Select(v => v.Condition).Distinct().ToList();

        //                    // For testing, log the results to the console
        //                    Console.WriteLine("Languages:");
        //                    foreach (var lang in distinctLanguages)
        //                    {
        //                        Console.WriteLine(lang);
        //                    }

        //                    Console.WriteLine("Conditions:");
        //                    foreach (var condition in distinctConditions)
        //                    {
        //                        Console.WriteLine(condition);
        //                    }

        //                    // Prepare the card details for navigation (for each product)
        //                    var details = new CardDetailViewModel()
        //                    {
        //                        Languages = distinctLanguages,
        //                       // Conditions = distinctConditions,
        //                        Conditions = distinctConditions,
        //                        Name = product.Model, 
        //                        Rarity = product.Rarity,
        //                        Category = product.Category,
        //                        ImageUrl = "https://www.cardgamecorner.com" + product.Image,
        //                        game = product.Game,
        //                        idMetaProductId=product.IdMetaproduct,
        //                        IdCategory=product.IdCategory,
        //                        IsFoil = product.IsFoil,


        //                    };
        //                    detaillst.Add(details);


        //                    string detailsJson = JsonConvert.SerializeObject(detaillst);

        //                    // Store the serialized string in SecureStorage
        //                    await SecureStorage.SetAsync("CardDetailsObject", detailsJson);

        //                    // Navigate to the CardDetailsPage for each product
        //                    // await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(details));



        //                    // Navigate using GoToAsync with the serialized data as a query parameter
        //                    await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(detailsJson)}");
        //                }
        //            }

        //            var detailsJsoncarddetails = JsonConvert.SerializeObject(detaillst);  // Ensure you have 'Newtonsoft.Json' or other serializer for this


        //            // Navigate using GoToAsync with the serialized data as a query parameter
        //            await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(detailsJsoncarddetails)}");
        //            // await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(detaillst));


        //            // Show success confirmation
        //            await Application.Current.MainPage.DisplayAlert(AppResources.Success,
        //                AppResources.successmsg,AppResources.OK);
        //        }
        //        else
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error",
        //                "No product data found in the response!", "OK");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle errors gracefully
        //        await Application.Current.MainPage.DisplayAlert("Error",
        //            $"An error occurred: {ex.Message}", "OK");
        //    }
        //}


        [RelayCommand]
        private async Task ConfirmCard()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                var response = new ListBoxService();
                var detaillst = new List<CardDetailViewModel>();
                var conditinlst = await response.GetConditionsAsync();
                var lnglst = await response.GetLanguagesAsync();

                // Ensure responseContent and its Products are not null or empty
                if (responseContent?.Products?.Count > 0)
                {
                    // Iterate through all products in the list
                    foreach (var product in responseContent.Products)
                    {
                        var variants = product.Variants;

                        if (variants != null)
                        {
                            // Map languages to their respective distinct conditions
                            var languageConditionsMap = variants
                                .GroupBy(v => v.Language) // Group variants by Language
                                .ToDictionary(
                                    group => group.Key,                     // Language as key
                                    group => group.Select(v => v.Condition) // Select Conditions for each language
                                                 .Distinct()               // Get distinct conditions
                                                 .ToList()                 // Convert to List
                                );
                            var distinctLanguages = variants.Select(v => v.Language).Distinct().ToList();
                            var distinctConditions = variants.Select(v => v.Condition).Distinct().ToList();

                            // Log the results to the console for testing
                            Console.WriteLine("Language-Condition Mapping:");
                            foreach (var kvp in languageConditionsMap)
                            {
                                Console.WriteLine($"Language: {kvp.Key}");
                                Console.WriteLine("Conditions:");
                                foreach (var condition in kvp.Value)
                                {
                                    Console.WriteLine($"  {condition}");
                                }
                            }

                            var details = new CardDetailViewModel()
                            {
                                // id= int.Parse(product.Id),
                                Languages = distinctLanguages,
                                Conditions = distinctConditions,
                                Name = product.Model,
                                Rarity = product.Rarity,
                                Category = product.Category,
                                ImageUrl = "https://www.cardgamecorner.com" + product.Image,
                                game = product.Game,
                                LanguageConditionsMap = languageConditionsMap,
                                //IsFirstEdition=product.Variants.Any(v => !string.IsNullOrEmpty(v.FirstEdition)), // Check if any variant is marked as FirstEdition
                                SelectedLanguage = lnglst.Where(item => item.Id == product.Language).Select(item => item.Language).FirstOrDefault(),
                                selectedCondition = conditinlst.Where(item => item.Id == product.Condition).Select(item => item.Condition).FirstOrDefault(),
                                varinats = variants.ToList()


                            };
                            detaillst.Add(details);
                        }
                    }

                    // Serialize the details list into JSON
                    string detailsJson = JsonConvert.SerializeObject(detaillst);

                    // Store the serialized string in SecureStorage
                    await SecureStorage.SetAsync("CardDetailsObject", detailsJson);

                    // Navigate to the CardDetailsPage with the serialized data
                    await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(detailsJson)}");

                    // Show success confirmation
                    await Application.Current.MainPage.DisplayAlert(AppResources.Success,
                        AppResources.successmsg, AppResources.OK);
                }
                else
                {
                    // Handle case where no product data is found
                    await Application.Current.MainPage.DisplayAlert("Error",
                        "No product data found in the response!", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"An error occurred: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        //private async Task ProcessCapturedImage(Stream imageStream)
        //{
        //    try
        //    {
        //        //var scanService = new ScanCardService();
        //        //await scanService.UploadImageAsync(imageStream);
        //        // Handle the response and update the UI
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error",
        //            $"Failed to process image: {ex.Message}", "OK");
        //    }
        //}
    }
}