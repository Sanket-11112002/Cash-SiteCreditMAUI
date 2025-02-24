using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CardGameCorner.ViewModels
{


    public class ProductVariant1
    {
        public int IdProduct { get; set; }
        public string FirstEdition { get; set; }
        public string Condition { get; set; }
        public string Foil { get; set; }
        public string Language { get; set; }
        public decimal Price { get; set; }
        //  public int Price { get; set; }
        public decimal BuyList { get; set; }
        public decimal Credit { get; set; }
        //   public int BuyList { get; set; }
        public int BuyListLock { get; set; }
        public bool NoBuyList { get; set; }
        public int Quantity { get; set; }
        public bool Evaluation { get; set; }
    }

    public class Product1
    {
        public string Id { get; set; }
        public string Game { get; set; }
        public string Model { get; set; }
        public string ModelEn { get; set; }
        public string ModelSeo { get; set; }
        public string ModelSeoEn { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public string ColorLong { get; set; }
        public string Rarity { get; set; }
        public string CategorySeo { get; set; }
        public string Category { get; set; }
        public int IdCategory { get; set; }
        public string Icon { get; set; }
        public int Quantity { get; set; }
        // public List<ProductVariant1> Variants { get; set; } // Parsed from JSON

        [JsonConverter(typeof(ProductVariantConverter))]
        public List<ProductVariant1> Variants { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double MinAcquisto { get; set; }
        public double MaxAcquisto { get; set; }
        public bool BuyListLock { get; set; }
        public bool HotBuyList { get; set; }
        public bool NoBuyList { get; set; }
        public int SerialNumber { get; set; }
        public string SerialNumberKeyword { get; set; }
        public int Language { get; set; }
        public int Condition { get; set; }
        public int Qty { get; set; }
        public bool Modern { get; set; }
        public int IdMetaproduct { get; set; }
        public bool Evaluation { get; set; }
        public bool IsFoil { get; set; }
        public int CategorySortOrder { get; set; }
        public string SetCode { get; set; }
    }

    public class CardSearchResponseViewModel
    {
        public List<Product1> Products { get; set; }
        public int Total { get; set; }
    }




    public class ProductVariantConverter : JsonConverter<List<ProductVariant1>>
    {
        public override List<ProductVariant1> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // If the reader token is a string, parse the string as JSON.
            if (reader.TokenType == JsonTokenType.String)
            {
                string json = reader.GetString();
                return JsonSerializer.Deserialize<List<ProductVariant1>>(json, options);
            }

            // Otherwise, directly deserialize as a list.
            return JsonSerializer.Deserialize<List<ProductVariant1>>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, List<ProductVariant1> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); // Only needed if you need to serialize back.
        }
    }

}
