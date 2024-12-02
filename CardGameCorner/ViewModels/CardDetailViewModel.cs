
using CardGameCorner.Models;
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
        public List<string> conditions;  // Collection of languages

        [ObservableProperty]
        public string selectedLanguage;  // The selected language

        [ObservableProperty]
        public decimal buylist;  // The selected language


        [ObservableProperty]
        public string selectedCondition;


 // The selected language


        public CardDetailViewModel()
        {
           

            // Initialize commands
         //   AddToListCommand = new Command(ExecuteAddToList);
            GoBackCommand = new Command(ExecuteGoBack);
            DoneCommand = new Command(ExecuteDone);

            
            // Initialize the list of languages (this could come from a service or API)
           

            selectedLanguage = "Choose Lanugage";
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
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async void ExecuteDone()
        {
            try
            {
                // Reset navigation stack to ScanPage
              //  await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack
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
