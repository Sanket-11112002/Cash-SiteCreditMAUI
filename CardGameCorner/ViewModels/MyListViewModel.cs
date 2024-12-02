using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameCorner.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CardGameCorner.ViewModels
{
    public class MyListViewModel : ObservableObject
    {
        private List<CardItem> _cardItems;

        public List<CardItem> CardItems
        {
            get { return _cardItems; }
            set
            {
                _cardItems = value;
                OnPropertyChanged(nameof(CardItems));
            }
        }

        public MyListViewModel()
        {
            // Initialize the card items
            CardItems = new List<CardItem>
        {
            new CardItem
            {
                Image = "pokemon_card.jpg",
                Name = "Mr. Mime",
                Model = "151",
                Language = "NM",
                Edition = "1ST Ed.",
                Rarity = "Illustration Rare",
                Cash = 13.20m,
                Quantity = 11,
                SiteCredit = 11.00m
            },
            new CardItem
            {
                Image = "pokemon_card.jpg",
                Name = "Mr. Mime",
                Model = "151",
                Language = "NM",
                Edition = "1ST Ed.",
                Rarity = "Illustration",
                Cash = 11.11m,
                Quantity = 4,
                SiteCredit = 17.00m
            },
            new CardItem
            {
                Image = "pokemon_card.jpg",
                Name = "Mr. Mime",
                Model = "151",
                Language = "NM",
                Edition = "1ST Ed.",
                Rarity = "Illustration Rare",
                Cash = 13.20m,
                Quantity = 111,
                SiteCredit = 121.00m
            }
        };
        }
    }
}


     