using System.Text.Json;
using System.Text.Json.Serialization;

public class ScannedCardDetails
{
    public int Id { get; set; }
    public string ExtId { get; set; }
    public int IdSet { get; set; }
    public string Num { get; set; }
    public string Title { get; set; }

    [JsonIgnore]
    public Extra Extra { get; set; }

    [JsonPropertyName("extra")]
    public string ExtraJson
    {
        get => JsonSerializer.Serialize(Extra);
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                Extra = JsonSerializer.Deserialize<Extra>(value);
            }
        }
    }

    public string Set { get; set; }
    public string Rarity { get; set; }
    public string Type { get; set; }
    public string Lang { get; set; }
    public string LocalTitle { get; set; }
    public int Matches { get; set; }
    public int DetectSet { get; set; }
    public int Foil { get; set; }
    public string Condition { get; set; }
    public string Price { get; set; }
    public string PriceTrend { get; set; }
    public int EcommerceId { get; set; }
}

public class Extra
{
    public List<string> Types { get; set; }
    public string Rarity { get; set; }
    public List<string> Reprints { get; set; }
    public string TcgName { get; set; }
    public string Supertype { get; set; }
}

public class ApiResponse_Card
{
    public ScannedCardDetails Result { get; set; }
}
