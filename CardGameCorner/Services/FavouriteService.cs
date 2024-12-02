using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameCorner.Models;
using Newtonsoft.Json;

namespace CardGameCorner.Services
{
    public class FavouriteService
    {

        private const string FavoriteKey = "FavoriteProducts"; // Key to store the list of favorite products


        public FavouriteService()
        {




        }
        // Save the list of favorite products to Preferences
        public static void StoreFavorites(List<Product> favoriteProducts)
        {
            // Serialize the product list to JSON string
            var favoriteProductsJson = JsonConvert.SerializeObject(favoriteProducts);
            Preferences.Set(FavoriteKey, favoriteProductsJson);
        }

        // Get the list of favorite products from Preferences
        public static List<Product> GetFavorites()
        {
            var favoriteProductsJson = Preferences.Get(FavoriteKey, string.Empty); // Get the stored JSON string
            if (string.IsNullOrEmpty(favoriteProductsJson))
            {
                return new List<Product>(); // Return an empty list if no favorites exist
            }

            // Deserialize the JSON string back to a list of Product objects
            return JsonConvert.DeserializeObject<List<Product>>(favoriteProductsJson);
        }
    }
}
