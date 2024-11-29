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

    //public class MyListViewModel : BaseViewModel
    //{
    //    private string _name;
    //    private string _language;
    //    private string _condition;
    //    private string _cash;
    //    private string _siteCredit;
    //    private int _quantity;

    //    public string Name
    //    {
    //        get => _name;
    //        set => SetProperty(ref _name, value);
    //    }

    //    public string Language
    //    {
    //        get => _language;
    //        set => SetProperty(ref _language, value);
    //    }

    //    public string Condition
    //    {
    //        get => _condition;
    //        set => SetProperty(ref _condition, value);
    //    }

    //    public string Cash
    //    {
    //        get => _cash;
    //        set => SetProperty(ref _cash, value);
    //    }

    //    public string SiteCredit
    //    {
    //        get => _siteCredit;
    //        set => SetProperty(ref _siteCredit, value);
    //    }

    //    public int Quantity
    //    {
    //        get => _quantity;
    //        set => SetProperty(ref _quantity, value);
    //    }

    //    private readonly IList<MyListViewModel> _cards;

    //    public MyListViewModel()
    //    {
    //        _cards = new List<MyListViewModel>
    //    {
    //        new MyListViewModel
    //        {
    //            Name = "Mr. Mime",
    //            Language = "NM",
    //            Condition = "1ST Ed.",
    //            Cash = "13.20 EUR",
    //            SiteCredit = "17.00 EUR",
    //            Quantity = 4
    //        },
    //        new MyListViewModel
    //        {
    //            Name = "Mr. Mime",
    //            Language = "NM",
    //            Condition = "1ST Ed.",
    //            Cash = "13.20 EUR",
    //            SiteCredit = "17.00 EUR",
    //            Quantity = 4
    //        },
    //        new MyListViewModel
    //        {
    //            Name = "Mr. Mime",
    //            Language = "NM",
    //            Condition = "1ST Ed.",
    //            Cash = "13.20 EUR",
    //            SiteCredit = "17.00 EUR",
    //            Quantity = 4
    //        }
    //    };
    //    }

    //    public IReadOnlyList<MyListViewModel> Cards => (IReadOnlyList<MyListViewModel>)_cards;
    //}

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
                Quantity = 4,
                SiteCredit = 17.00m
            }
        };
        }
    }
}


     