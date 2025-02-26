//using CardGameCorner.Models;

//public class GameDetailsResponse
//{
//    public Data Data { get; set; }

//    public List<Banner> Banners { get; set; }

//}

//public class Banner
//{
//    public string Game { get; set; }
//    public string Title { get; set; }
//    public string Image { get; set; }
//    public string Url { get; set; }
//}


//public class Data
//{
//    public List<Result> Results { get; set; }

//}



//public class Result
//{
//    public Image image { get; set; }
//    public Model model { get; set; }
//    public Modelen modelen { get; set; }
//    public Novita novita { get; set; }
//    public Maxprice maxprice { get; set; }
//    public Minprice minprice { get; set; }

//    public Url url { get; set; }
//    public Url urlen { get; set; }
//}


//public class Url
//{
//    public string raw { get; set; }
//}

//public class Image
//{
//    public string raw { get; set; }
//}

//public class Model
//{
//    public string snippet { get; set; }
//}
//public class Modelen
//{
//    public string snippet { get; set; }
//}
//public class Novita
//{
//    public string raw { get; set; }
//}

//public class Maxprice
//{
//    public decimal raw { get; set; }
//}

//public class Minprice
//{
//    public decimal raw { get; set; }
//}


using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CardGameCorner.Models
{
    public class GameDetailsResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("banners")]
        public List<Banner> Banners { get; set; }
    }

    public class Banner
    {
        [JsonProperty("game")]
        public string Game { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Data
    {
        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public class Value
    {
        [JsonProperty("products")]
        public List<GameProduct> Products { get; set; } // Changed from `Product` to `GameProduct`
    }

    public class GameProduct // Renamed from `Product` to `GameProduct`
    {
        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("modelEn")]
        public string ModelEn { get; set; }

        [JsonProperty("minPrice")]
        public decimal MinPrice { get; set; }

        [JsonProperty("maxPrice")]
        public decimal MaxPrice { get; set; }
    }
}


