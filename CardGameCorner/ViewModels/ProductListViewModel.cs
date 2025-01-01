using CardGameCorner.Models;
using Newtonsoft.Json;

public class ProductListViewModel
{
    public int? Id { get; set; }
    public string? Game { get; set; }
    public string? Model { get; set; }
    public string? ModelEn { get; set; } // Add missing field
    public string? Image { get; set; }
    public string? Color { get; set; } // Add missing field
    public string? Rarity { get; set; }
    public string? Category { get; set; }
    public decimal? Buylist { get; set; }
    public decimal? Sitecredit { get; set; }
    public int? Quantity { get; set; }
    public string Language { get; set; }
    public string UserName { get; set; }
    public string? Languagejsonlst { get; set; }
    public string? Conditionjsonlst { get; set; }
    public string? Condition { get; set; }
    public bool? IsFirstEdition { get; set; }
    public bool? IsReverse { get; set; }
    public bool? IsFoil { get; set; }
    public string? Languageflag { get; set; }
    public int? ProductId { get; set; }
    // Deserialize JSON strings
    public List<string>? Languages { get; set; }
    public List<string>? Conditions { get; set; }
    public bool? Evalution { get; set; }

    // Map back to ProductList
    public ProductList MapToProductList()
    {
        return new ProductList
        {
            Id = this.Id,
            Game = this.Game,
            Model = this.Model,
            ModelEn = this.ModelEn, // Map missing field
            Image = this.Image,
            Color = this.Color, // Map missing field
            Rarity = this.Rarity,
            Category = this.Category,
            Buylist = this.Buylist,
            Sitecredit = this.Sitecredit,
            Quantity = this.Quantity,
            Language = this.Language,
            Username = this.UserName,
            Languagejsonlst = JsonConvert.SerializeObject(this.Languages),
            Conditionjsonlst = JsonConvert.SerializeObject(this.Conditions),
            Condition = this.Condition,
            IsFirstEdition = this.IsFirstEdition,
            IsReverse = this.IsReverse,
            ProductId = this.ProductId,
            Evalution = this.Evalution,
        };
    }
}
