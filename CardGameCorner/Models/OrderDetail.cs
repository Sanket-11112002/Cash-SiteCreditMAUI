using System.Text.Json.Serialization;

public class CardDetail
{
    [JsonPropertyName("idProduct")]
    public int IdProduct { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("condition")]
    public string Condition { get; set; }

    [JsonPropertyName("confirmedCondition")]
    public string ConfirmedCondition { get; set; }

    [JsonPropertyName("firstEdition")]
    public string FirstEdition { get; set; }

    [JsonPropertyName("foil")]
    public string Foil { get; set; }

    [JsonPropertyName("qty")]
    public int Qty { get; set; }

    [JsonPropertyName("confirmedQty")]
    public int ConfirmedQty { get; set; }

    [JsonPropertyName("confirmedPrice")]
    public decimal ConfirmedPrice { get; set; }

    [JsonPropertyName("evaluated")]
    public bool Evaluated { get; set; }

    [JsonPropertyName("alteredByStaff")]
    public bool AlteredByStaff { get; set; }

    public string LanguageFlag => Language?.ToUpper() switch
    {
        "IT" => "italianlngimage.png",
        "EN" => "gb.png",
        _ => "default_flag.png" // Default flag image
    };
}

public class OrderDetail
{
    [JsonPropertyName("orderId")]
    public int OrderId { get; set; }

    [JsonPropertyName("orderDate")]
    public DateTime OrderDate { get; set; }

    [JsonPropertyName("game")]
    public string Game { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("payment")]
    public string Payment { get; set; }

    [JsonPropertyName("paymentAccount")]
    public string PaymentAccount { get; set; }

    [JsonPropertyName("contact")]
    public string Contact { get; set; }

    [JsonPropertyName("pickup")]
    public bool Pickup { get; set; }

    [JsonPropertyName("pickupCost")]
    public decimal PickupCost { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    [JsonPropertyName("fiscalCode")]
    public string FiscalCode { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("zip")]
    public string Zip { get; set; }

    [JsonPropertyName("province")]
    public string Province { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("cards")]
    public List<CardDetail> Cards { get; set; }
}