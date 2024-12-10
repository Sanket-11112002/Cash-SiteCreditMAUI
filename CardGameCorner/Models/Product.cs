using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace CardGameCorner.Models
{
    public class Product: ObservableObject
    {
       
        public string Id { get; set; }
        public string Game { get; set; }
        public string Model { get; set; }
        public string ModelEn { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public string Rarity { get; set; }
        public string Category { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public string SetCode { get; set; }
        private bool _isFavorite;

        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                SetProperty(ref _isFavorite, value);
                OnPropertyChanged(nameof(FavoriteIcon));
            }
        }

        public string FavoriteIcon => IsFavorite ? "filled_heart.png" : "heart_img.png";
    }


    public class ApiResponse
    {
        public List<Product> Products { get; set; }
        public int Total { get; set; }
    }

      
        // Other properties like ModelEn, Image, etc.
    }
