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

public class BuyListPriceResponse
{
    public decimal buylist { get; set; }
    public decimal SiteCredit { get; set; }
    [JsonConverter(typeof(EvaluationConverter))]
    public bool evaluation { get; set; }
}

public class cardDetailRequest
{
    public int ?idMetaproduct { get; set; }
    public int ?idCategory { get; set; }
    public int ?language { get; set; }
    public string ?sku { get; set; }
    public int ?condition { get; set; }
    public bool ?IsFirstEdition { get; set; }
   // public bool IsReverse { get; set; }
    //public string IsFoil { get; set; }
    public string? IsFoil { get; set; }


      
}

public class EvaluationConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            return value?.ToLower() == "true";  // Convert "true" string to boolean true
        }
        else if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
        {
            return reader.GetBoolean();
        }

        throw new JsonException("Unexpected token type for Evaluation");
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
