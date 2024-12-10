
using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CardGameCorner.ViewModels
{
    public partial class CardDetailViewModel : ObservableObject,INotifyPropertyChanged
    {
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

        [ObservableProperty]
        public decimal cashPrice;

        [ObservableProperty]
        public decimal siteCredit;

        [ObservableProperty]
        public bool isFirstEdition;

        [ObservableProperty]
        public bool isReverse;

        [ObservableProperty]
        public string game;

        [ObservableProperty]
        public List<string> languages;

        [ObservableProperty]
        public List<string> conditions; 

        [ObservableProperty]
        public string selectedLanguage;  

        [ObservableProperty]
        public decimal buylist;  

        [ObservableProperty]
        public string selectedCondition;

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

        [ObservableProperty]
        public string errorRetrieveTokenTitle;

        [ObservableProperty]
        public string errorRetrieveTokenMessage;

        [ObservableProperty]
        public string successAddedToListTitle;

        [ObservableProperty]
        public string successAddedToListMessage;

        [ObservableProperty]
        public string errorAddProductTitle;

        [ObservableProperty]
        public string errorRetrieveProductTitle;

        public CardDetailViewModel()
        {
            // Initialize with current language
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

            //   AddToListCommand = new Command(ExecuteAddToList);
            GoBackCommand = new Command(ExecuteGoBack);
            DoneCommand = new Command(ExecuteDone);

            
            // Initialize the list of languages (this could come from a service or API)
           
            selectedLanguage = "Choose Lanugage";
            InitializeLocalizedErrorMessages();
        }

        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
            {
                // Update localized strings when language changes
                UpdateLocalizedStrings();
                InitializeLocalizedErrorMessages();
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

            });
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
            try
            {
                 await Shell.Current.Navigation.PopToRootAsync(); // navigate to root page
                //await Application.Current.MainPage.Navigation.PopAsync();
               // await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Fallback to a specific route
                await Shell.Current.GoToAsync("//search");
                Console.WriteLine($"Navigation error: {ex.Message}");
            }
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
