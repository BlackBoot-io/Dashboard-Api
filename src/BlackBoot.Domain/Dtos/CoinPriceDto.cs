namespace BlackBoot.Domain.Dtos;

public class CoinPriceDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    public double current_price { get; set; }
}
