
using CardGameCorner.Models;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;

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
        public List<string> conditions;  // Collection of languages

        //[ObservableProperty]
        //public string selectedLanguage;  // The selected language

    
        [ObservableProperty]
        public string sku;

       // The selected language


        [ObservableProperty]
        public string selectedCondition;


        private string _selectedLanguage;
        private bool _isFoil;
        private bool _isFirstEdition;
        private bool _isReverse;
        private decimal _buylist;
        private decimal _siteCredit;

        // The selected language

        private ObservableCollection<CardDetailViewModel> _cards;
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


        public CardDetailViewModel()
        {
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
        }
       
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;

                    UpdateConditions(); // Update the conditions when language changes
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
                    //  UpdateConditions(); // Update the conditions when language changes
                     OnPropertyChanged();
                }
            }
        }

        // This method is called when the SelectedLanguage changes.
        // It updates the Conditions based on the selected language.
        public void UpdateConditions()
        {
            if (SelectedLanguage == "Italiano")
            {
                Conditions = new List<string> { "NM", "HP" };
            }
            else if (SelectedLanguage == "English")
            {
                Conditions = new List<string> { "NM" };
            }
            else
            {
                Conditions = new List<string>(); // Default empty list if language is not selected
            }
            OnPropertyChanged(nameof(Conditions)); // Notify UI that Conditions has been updated
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
                    FetchPricesAsync(); // Call the API when First Edition toggle is changed
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

        public async void ExecuteGoBack()
        {
            //  await Application.Current.MainPage.Navigation.PopAsync();
            await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack
            await Shell.Current.GoToAsync("//SearchPage");

        }

        public async void ExecuteDone()
        {
            try
            {
                // Reset navigation stack to ScanPage
                await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack
                await Shell.Current.GoToAsync("//MyListPage"); // Navigate to ScanPage tab
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
    }
}
