using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

using CardGameCorner.Models;
using CardGameCorner.Services;
using CardGameCorner.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;



namespace CardGameCorner.ViewModels {
    public class MyListViewModel : ObservableObject
    {
        // Observable collection to hold card items
        public ObservableCollection<ProductListViewModel> CardItems { get; set; } = new ObservableCollection<ProductListViewModel>();


        private ICommand _navigateToCardDetailCommand;
        private ICommand _deleteCardCommand;

        public ICommand NavigateToCardDetailCommand
        {
            get => _navigateToCardDetailCommand;
            set => SetProperty(ref _navigateToCardDetailCommand, value);
        }
        public ICommand DeleteCardCommand
        {
            get => _deleteCardCommand;
            set => SetProperty(ref _deleteCardCommand, value);
        }

        public MyListViewModel()
        {
            CardItems = new ObservableCollection<ProductListViewModel>();
            NavigateToCardDetailCommand = new Command<ProductListViewModel>(NavigateToCardDetail);
            DeleteCardCommand = new Command<ProductListViewModel>(DeleteCard);
            getlist(); // Get the card data asynchronously
        }

        // Method to handle navigation to card details
        private async void NavigateToCardDetail(ProductListViewModel selectedCard)
        {
            if (selectedCard == null) return;

            // Create a new instance of CardDetailViewModel and populate it with data from selectedCard
            var cardDetailViewModel = new CardDetailViewModel
            {
                Id=selectedCard.Id ?? 0,
                Name = selectedCard.Model,
                Rarity = selectedCard.Rarity,
                Category = selectedCard.Category,
                ImageUrl = selectedCard.Image,
                //CashPrice = selectedCard.CashPrice,
                SiteCredit = selectedCard.Sitecredit ?? 0,
              //  IsFirstEdition = selectedCard.IsFirstEdition,
               // IsReverse = selectedCard.IsReverse,
                Game = selectedCard.Game,
                 Languages = selectedCard.Languages,
                Conditions = selectedCard.Conditions,
                SelectedLanguage = selectedCard.Language,
                SelectedCondition = selectedCard.Condition,
                Buylist = selectedCard.Buylist?? 0,
                Quantity = selectedCard.Quantity ?? 0,
            };

            // Pass the ViewModel instance as a navigation parameter
    //        var navigationParameter = new Dictionary<string, object>
    //{
    //    { "CardDetailViewModel", cardDetailViewModel }
    //};

         //   await Shell.Current.GoToAsync(nameof(CardDetailPage), navigationParameter);

            await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(cardDetailViewModel));
        }

        public async Task getlist()
        {
            var myListService = new SQLiteService();
            var items = await myListService.GetAllItemsAsync();

            if (items != null)
            {
                CardItems.Clear();
                foreach (var item in items)
                {
                    var card = new ProductListViewModel
                    {
                        Id = item.Id ?? 0,
                        Model = item.Model,
                        Rarity = item.Rarity,
                        Category = item.Category,
                        Image = item.Image,
                        //CashPrice = selectedCard.CashPrice,
                        Sitecredit = item.Sitecredit ?? 0,
                          IsFirstEdition = item.IsFirstEdition,
                         IsReverse = item.IsReverse,
                        Game = item.Game,
                      
                        Language = item.Language,
                        Condition = item.Condition,
                        Buylist = item.Buylist ?? 0,
                        Quantity = item.Quantity ?? 0,
                        Languageflag= "italianlng.svg"
                    };

                    if(item.Languagejsonlst!=null && item.Conditionjsonlst != null)
                    {
                        card.Languages = JsonConvert.DeserializeObject<List<string>>(item.Languagejsonlst);
                        card.Conditions = JsonConvert.DeserializeObject<List<string>>(item.Conditionjsonlst);
                    }

                    CardItems.Add(card);
                }
            }
        }

        public async Task LoadDataAsync()
        {
            await getlist();
        }

        private async void DeleteCard(ProductListViewModel selectedCard)
        {
            if (selectedCard != null)
            {
                // Show a confirmation dialog
                bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Delete", // Title of the alert
                    "Are you sure you want to delete this card?", // Message
                    "Yes", // Confirmation button
                    "No" // Cancel button
                );

                if (isConfirmed)
                {
                    var myListService = new SQLiteService();
                    var product = selectedCard.MapToProductList();

                    // Call your SQLite service to delete the item
                    await myListService.DeleteItemAsync(product);

                    // Refresh the list or perform necessary actions after deletion
                    await getlist();
                }
            }
        }

    }
}

