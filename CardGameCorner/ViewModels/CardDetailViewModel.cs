
using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CardGameCorner.ViewModels
{
    public partial class CardDetailViewModel : ObservableObject,INotifyPropertyChanged
    {
        private readonly IScanCardService _service;
        private CardModel _card;
        private int _quantity = 1;
        private ICommand _addToListCommand;
        private ICommand _goBackCommand;
        private ICommand _doneCommand;


        [ObservableProperty]
        public int id;


        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string rarity;

        [ObservableProperty]
        public string category;

        [ObservableProperty]
        public string imageUrl;

        //[ObservableProperty]
        //public decimal cashPrice;

        //[ObservableProperty]
        //public decimal siteCredit;

        [ObservableProperty]
        public int idMetaProductId;

        [ObservableProperty]
        public int idCategory;

        [ObservableProperty]
        public string game;

        [ObservableProperty]
        public List<string> languages;

        [ObservableProperty]
        public List<string> conditions; 

        //[ObservableProperty]
        //public string selectedLanguage;  



        //[ObservableProperty]
        //public string selectedCondition;

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;


        [ObservableProperty]
        public string back;

        [ObservableProperty]
        public string done;
        
        [ObservableProperty]
        public string chooseLang;

        [ObservableProperty]
        public string chooseCondition;

        [ObservableProperty]
        public string edition;

        [ObservableProperty]
        public string reverse;

        [ObservableProperty]
        public string cash;

        [ObservableProperty]
        public string credit;

        [ObservableProperty]
        public string quantityLang;

        [ObservableProperty]
        public string addToList;

        [ObservableProperty]
        public string cardDetails;

        [ObservableProperty]
        public string loginRequiredTitle;

        [ObservableProperty]
        public string loginRequiredMessage;

        [ObservableProperty]
        public string loginText;

    
        [ObservableProperty]
        public string continueText;
        public string sku;

        [ObservableProperty]
        public string errorRetrieveTokenTitle;


        [ObservableProperty]
        public string errorRetrieveTokenMessage;

        [ObservableProperty]
        public string successAddedToListTitle;

        private string _selectedLanguage;
        private string _selectedCondition;
        private bool _isFoil;
        private bool _isFirstEdition;
        private bool _isReverse;
        private decimal _buylist;
        private decimal _siteCredit;

        // The selected language
        [ObservableProperty]
        public string successAddedToListMessage;

        [ObservableProperty]
        public string errorAddProductTitle;
        private ObservableCollection<CardDetailViewModel> ?_cards;
        private CardDetailViewModel _selectedCard;

        public ObservableCollection<CardDetailViewModel> Cards
        {
            get => _cards;
            set => SetProperty(ref _cards, value);
            
        }

        public CardDetailViewModel SelectedCard
        {
            get => _selectedCard;
            set => SetProperty(ref _selectedCard, value);
        }


        [ObservableProperty]
        public string errorRetrieveProductTitle;

        [ObservableProperty]
        private string gameImageUrl;

        public CardDetailViewModel()
        {
            UpdateLocalizedStrings();
            UpdateGameImage();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

            var scanserivc = new ScanCardService();
            _service = scanserivc;

            
            Languages = new List<string> { "Italiano", "English" };
            Conditions = new List<string>(); // Initially empty
            // Initialize commands
            //   AddToListCommand = new Command(ExecuteAddToList);
            GoBackCommand = new Command(ExecuteGoBack);
            DoneCommand = new Command(ExecuteDone);

            
            // Initialize the list of languages (this could come from a service or API)
           
            SelectedLanguage = "Choose Lanugage";
            InitializeLocalizedErrorMessages();
        }

        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
            {
                // Update localized strings when language changes
                UpdateLocalizedStrings();
                InitializeLocalizedErrorMessages();
                UpdateGameImage();
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
        private void InitializeLocalizedErrorMessages()
        {
            // Populate localized error messages
            LoginRequiredTitle = AppResources.LoginRequiredTitle;
            LoginRequiredMessage = AppResources.LoginRequiredMessage;
            LoginText = AppResources.Login;
            ContinueText = AppResources.Continue;
            ErrorRetrieveTokenTitle = AppResources.ErrorTitle;
            ErrorRetrieveTokenMessage = AppResources.ErrorRetrieveToken;
            SuccessAddedToListTitle = AppResources.Success;
            SuccessAddedToListMessage = AppResources.ProductAddedToList;
            ErrorAddProductTitle = AppResources.ErrorAddProduct;
            ErrorRetrieveProductTitle = AppResources.ErrorRetrieveProduct;
        }

        private void UpdateLocalizedStrings()
        {
            // Ensure these are called on the main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Back = AppResources.Back;
                Done = AppResources.Done;
                ChooseLang = AppResources.Choose_Language;
                ChooseCondition = AppResources.Choose_condition_grade;
                Edition = AppResources.Edition;
                Reverse = AppResources.Reverse;
                Cash = AppResources.Cash;
                Credit = AppResources.Site_credit;
                QuantityLang = AppResources.Quantity;
                AddToList = AppResources.AddToList;
                CardDetails = AppResources.Card_Details;


                // Trigger property changed events to update UI
                OnPropertyChanged(nameof(Back));
                OnPropertyChanged(nameof(Done));
                OnPropertyChanged(nameof(ChooseLang));
                OnPropertyChanged(nameof(ChooseCondition));
                OnPropertyChanged(nameof(Edition));
                OnPropertyChanged(nameof(Reverse));
                OnPropertyChanged(nameof(Cash));
                OnPropertyChanged(nameof(Credit));
                OnPropertyChanged(nameof(QuantityLang));
                OnPropertyChanged(nameof(AddToList));
                OnPropertyChanged(nameof(CardDetails));
                OnPropertyChanged(nameof(LoginRequiredTitle));
                OnPropertyChanged(nameof(LoginRequiredMessage));
                OnPropertyChanged(nameof(LoginText));
                OnPropertyChanged(nameof(ContinueText));

                OnPropertyChanged(nameof(ErrorRetrieveTokenTitle));
                OnPropertyChanged(nameof(ErrorRetrieveTokenMessage));
                OnPropertyChanged(nameof(SuccessAddedToListTitle));
                OnPropertyChanged(nameof(SuccessAddedToListMessage));
                OnPropertyChanged(nameof(ErrorAddProductTitle));
                OnPropertyChanged(nameof(ErrorRetrieveProductTitle));
                // Initialize the list of languages (this could come from a service or API)
                SelectedLanguage = "Choose Lanugage";
            });
        }

       
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;

                    UpdateConditions();
                    // Update the conditions when language changes
                    OnPropertyChanged();
                }
            }
        }


        public string selectedCondition
        {
            get => _selectedCondition;
            set
            {
                if (_selectedCondition != value)
                {
                    _selectedCondition = value;

                   // UpdateConditions();
                    FetchPricesAsync();
                    OnPropertyChanged();
                }
            }
        }

        public decimal buyList
        {
            get => _buylist;
            set
            {
                if (_buylist != value)
                {
                    _buylist = value;
                  //  UpdateConditions(); // Update the conditions when language changes
                    OnPropertyChanged();
                }
            }
        }
        public decimal siteCredit
        {
            get => _siteCredit;
            set
            {
                if (_siteCredit != value)
                {
                    _siteCredit = value;
                    //  UpdateConditions();
                     OnPropertyChanged();
                }
            }
        }

        public void UpdateConditions()
        {
            //if (SelectedLanguage == "Italiano")
            //{
            //    Conditions = new List<string> { "NM", "HP" };
            //}
            //else if (SelectedLanguage == "English")
            //{
            //    Conditions = new List<string> { "NM" };
            //}
            //else
            //{
            //    Conditions = new List<string>(); // Default empty list if language is not selected
            //}
            //OnPropertyChanged(nameof(Conditions)); // Notify UI that Conditions has been updated
        }

        public bool IsFirstEdition
        {
            get => _isFirstEdition;
            set
            {
                if (_isFirstEdition != value)
                {
                    _isFirstEdition = value;
                    OnPropertyChanged();
                    FetchPricesAsync(); 
                }
            }
        }

        public bool IsReverse
        {
            get => _isReverse;
            set
            {
                if (_isReverse != value)
                {
                    _isReverse = value;
                    OnPropertyChanged();
                    FetchPricesAsync(); // Call the API when Reverse toggle is changed
                }
            }
        }

        public bool IsFoil
        {
            get => _isFoil;
            set
            {
                if (_isFoil != value)
                {
                    _isFoil = value;
                    OnPropertyChanged();
                    FetchPricesAsync(); // Call the API when Foil toggle is changed
                }
            }
            
        }

        public ICommand GoBackCommand
        {
            get => _goBackCommand;
            set => SetProperty(ref _goBackCommand, value);
        }

        public ICommand DoneCommand
        {
            get => _doneCommand;
            set => SetProperty(ref _doneCommand, value);
        }

        //public async void ExecuteGoBack()
        //{
        //    await Application.Current.MainPage.Navigation.PopAsync();
        //}
        public async void ExecuteGoBack()
        {
            //  await Application.Current.MainPage.Navigation.PopAsync();
            //  await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack
              //await Shell.Current.GoToAsync(nameof(SearchQueryPage));
            await Shell.Current.GoToAsync("..", true);
            // await Shell.Current.Navigation.PopAsync();

        }

        public async void ExecuteDone()
        {
            try
            {
                // Reset navigation stack to ScanPage
               // await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack
                await Shell.Current.GoToAsync("//MyListPage");
               // await Shell.Current.GoToAsync(nameof(CardDetailPage));// Navigate to ScanPage tab
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
            }
        }


        public CardModel Card
        {
            get => _card;
            set
            {
                if (_card != value)
                {
                    _card = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    // Refresh command can execute state when quantity changes
                   // (AddToListCommand as Command)?.ChangeCanExecute();
                }
            }
        }

        public ICommand AddToListCommand
        {
            get => _addToListCommand;
            set
            {
                if (_addToListCommand != value)
                {
                    _addToListCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private async Task FetchPricesAsync()
        {
            var newvariable = new ScanCardService();
           // _service = newvariable;
            var cardetailrequest = new cardDetailRequest();

            var response = new ListBoxService();

        

            // Fetch Language List
            var lnglst = await response.GetLanguagesAsync();
            if (lnglst == null || !lnglst.Any())
            {
                Console.WriteLine("Language list is null or empty.");
                return;
            }

            // Fetch Condition List
            var conditinlst = await response.GetConditionsAsync();
            if (conditinlst == null || !conditinlst.Any())
            {
                Console.WriteLine("Condition list is null or empty.");
                return;
            }

            // Find Language ID
            var selectedLangId = lnglst
                .Where(item => item.Language == SelectedLanguage)
                .Select(item => item.Id)
                .FirstOrDefault();

            // Find Condition ID
            var selectedconditinId = conditinlst
                .Where(item => item.Condition == selectedCondition)
                .Select(item => item.Id)
                .FirstOrDefault();

            cardetailrequest.condition = int.TryParse(selectedconditinId, out int conditionId) ? conditionId : 0;
            cardetailrequest.language = int.TryParse(selectedLangId, out int langId) ? langId : 0;
            cardetailrequest.IsFirstEdition = IsFirstEdition;
            cardetailrequest.idCategory = idCategory;
            cardetailrequest.idMetaproduct = idMetaProductId;
            cardetailrequest.IsFoil = "50";
            cardetailrequest.sku = "MEW0199";
            //cardetailrequest.condition = 1362;
            //cardetailrequest.language = 71;
            //cardetailrequest.IsFirstEdition = false;
            //cardetailrequest.idCategory = 3534;
            //cardetailrequest.idMetaproduct = 421085;
            //cardetailrequest.IsFoil = "50";
            //cardetailrequest.sku = "MEW0199";

            // Debugging: Print the request object to validate values
            Console.WriteLine($"Request: {System.Text.Json.JsonSerializer.Serialize(cardetailrequest)}");

          

            var result = await newvariable.FetchBuyListPriceAsync(cardetailrequest);
            if (result != null)
            {
                Console.WriteLine("Result: " + result);

                buyList = 89;
                siteCredit = result.SiteCredit;

                if (result.evaluation==true)
                {
                    await Application.Current.MainPage.DisplayAlert("",
                      "You can add the card to the shopping cart, we will make an evaluation and send you a message", "OK");
                }
                else if(result.evaluation=false && result.buylist == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("",
                     "We buy this Card in Bulk (low value cards)", "OK");
                }
                else
                {

                }
            }
            else
            {
                Console.WriteLine("No result returned from FetchBuyListPriceAsync.");
            }
        }

        // Implementing INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Helper method to update multiple properties
        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }


        public async void OnUploadButtonClicked(List<CardDetailViewModel> lst)
        {

            var detailsJson = JsonConvert.SerializeObject(lst);  // Ensure you have 'Newtonsoft.Json' or other serializer for this


            // Navigate using GoToAsync with the serialized data as a query parameter
              await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(detailsJson)}");

        }


    }

}
