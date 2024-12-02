using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameCorner.Models;
using Newtonsoft.Json;

namespace CardGameCorner.ViewModels
{
    public class ProductListViewModel
    {
        public int? Id { get; set; }
        public string? Game { get; set; }
        public string? Model { get; set; }
        public string? ModelEn { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }
        public string? Rarity { get; set; }
        public string? Category { get; set; }
        public decimal? Buylist { get; set; }
        public decimal? Sitecredit { get; set; }
        public int? Quantity { get; set; }
        public string Language { get; set; }

        // Store as string (JSON) in SQLite
        public string? Languagejsonlst { get; set; }
        public string? Conditionjsonlst { get; set; }

        public string? Condition { get; set; }
        public bool? IsFirstEdition { get; set; }
        public bool? IsReverse { get; set; }
        public string? Languageflag { get; set; }


        // Deserialize the JSON string to List<string> when accessing
        
        public List<string>? Languages;
       

       
        public List<string> ?Conditions;


        public ProductList MapToProductList()
        {
            return new ProductList
            {
                Id = this.Id,
                Model = this.Model,
                Rarity = this.Rarity,
                Category = this.Category,
                Image = this.Image,
                Sitecredit = this.Sitecredit,
                Game = this.Game,
                Language = this.Language,
                Condition = this.Condition,
                Buylist = this.Buylist,
                Quantity = this.Quantity,
               
            };
        }

    }
}
