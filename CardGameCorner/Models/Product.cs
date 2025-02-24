using System;
using System.Collections.Generic;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CardGameCorner.Models
{
    public class Product : ObservableObject
    {
        // Basic Product Information
        public string Id { get; set; }
        public string Game { get; set; }
        public string Model { get; set; }
        public string ModelEn { get; set; }
        public string ModelSeo { get; set; }
        public string ModelSeoEn { get; set; }
        public string Image { get; set; }

        // Product Characteristics
        public string Color { get; set; }
        public string ColorLong { get; set; }
        public string Rarity { get; set; }

        // Category Information
        public string CategorySeo { get; set; }
        public string Category { get; set; }
        public int IdCategory { get; set; }
        public string Icon { get; set; }
        public int CategorySortOrder { get; set; }
        public string SetCode { get; set; }

        // Inventory and Stock
        public int Quantity { get; set; }
        public string Variants { get; set; }  // JSON string containing variant details

        // Pricing Information
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinAcquisto { get; set; }
        public decimal MaxAcquisto { get; set; }

        // BuyList Related Properties
        public bool BuyListLock { get; set; }
        public bool HotBuyList { get; set; }
        public bool NoBuyList { get; set; }

        // Additional Metadata
        public int SerialNumber { get; set; }
        public string SerialNumber_keyword { get; set; }
        public int Language { get; set; }
        public int Condition { get; set; }
        public int Qty { get; set; }
        public bool Modern { get; set; }
        public int idMetaproduct { get; set; }
        public bool Evaluation { get; set; }
        public bool isFoil { get; set; }

        // Favorite Feature
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

        // Helper method to parse variants
        public List<ProductVariant> GetVariants()
        {
            if (string.IsNullOrEmpty(Variants))
                return new List<ProductVariant>();

            return JsonSerializer.Deserialize<List<ProductVariant>>(Variants,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }

    public class ProductVariant
    {
        public int IdProduct { get; set; }
        public string FirstEdition { get; set; }
        public string Condition { get; set; }
        public string Foil { get; set; }
        public string Language { get; set; }
        public decimal Price { get; set; }
        public decimal BuyList { get; set; }
        public decimal Credit { get; set; }
        public bool Evaluation { get; set; }
        public int BuyListLock { get; set; }
        public bool NoBuyList { get; set; }
        public int Quantity { get; set; }
    }

    public class ApiResponse
    {
        public List<Product> Products { get; set; }
        public int Total { get; set; }
    }
}