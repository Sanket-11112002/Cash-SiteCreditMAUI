using System;
using System.Collections.Generic;
using Newtonsoft.Json;  // You will need to install the Newtonsoft.Json NuGet package
using SQLite;
namespace CardGameCorner.Models
{
    public class ProductList
    {
        [PrimaryKey, AutoIncrement]
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

        // public string LoggedInToken { get; set; }
         public string Username { get; set; }
        public string? Languagejsonlst { get; set; }
        public string? Conditionjsonlst { get; set; }

        public string? Condition { get; set; }
        public bool? IsFirstEdition { get; set; }
        public bool? IsReverse { get; set; }
       
        //// Deserialize the JSON string to List<string> when accessing
        //[JsonIgnore]
        //public List<string> Languagelst
        //{
        //    get => string.IsNullOrEmpty(Languagejsonlst) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(Languagejsonlst);
        //    set => Languagejsonlst = JsonConvert.SerializeObject(value);
        //}

        //[JsonIgnore]
        //public List<string> Conditionlst
        //{
        //    get => string.IsNullOrEmpty(Conditionjsonlst) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(Conditionjsonlst);
        //    set => Conditionjsonlst = JsonConvert.SerializeObject(value);
        //}
    }
}
