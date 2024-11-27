using CardGameCorner.Models;
using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CardGameCorner.ViewModels
{
    public partial class CardDetailViewModel : ObservableObject
    {
        private CardModel _card;
        private int _quantity = 1;
        private ICommand _addToListCommand;
        private ICommand _goBackCommand;
        private ICommand _doneCommand;


        [ObservableProperty]
        private List<string> languages;  // Collection of languages

        [ObservableProperty]
        private string selectedLanguage;  // The selected language

        

        public CardDetailViewModel()
        {
            // Initialize with sample data
            Card = new CardModel
            {
                Name = "Blue-Eyes White Dragon",
                Rarity = "Secret Rare",
                Category = "RAGE OF THE ABYSS",
                ImageUrl = "pokemon_card.jpg",
                CashPrice = 13.90m,
                SiteCredit = 17.00m,
                IsFirstEdition = true,
                IsReverse = false
            };

            // Initialize commands
            AddToListCommand = new Command(ExecuteAddToList);
            GoBackCommand = new Command(ExecuteGoBack);
            DoneCommand = new Command(ExecuteDone);


            // Initialize the list of languages (this could come from a service or API)
            languages = new List<string>
            {
                "English",
                "Spanish",
                "French",
                "German",
                "Chinese"
            };
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

        private async void ExecuteGoBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void ExecuteDone()
        {
            // Implement your done logic here
            await Application.Current.MainPage.Navigation.PopAsync();
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
                    (AddToListCommand as Command)?.ChangeCanExecute();
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

        private void ExecuteAddToList()
        {
            try
            {
                // Add your logic here to handle adding the card to the list
                // For example:
                // await cardService.AddToList(Card, Quantity);

                // You can also show a success message using Page.DisplayAlert
                // await Application.Current.MainPage.DisplayAlert("Success", "Added to your list", "OK");
            }
            catch (Exception ex)
            {
                // Handle any errors
                // await Application.Current.MainPage.DisplayAlert("Error", "Failed to add to list", "OK");
                System.Diagnostics.Debug.WriteLine($"Error adding to list: {ex.Message}");
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