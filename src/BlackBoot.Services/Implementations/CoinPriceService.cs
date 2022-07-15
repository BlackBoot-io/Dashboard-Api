using BlackBoot.Shared.Core.Api;
using Microsoft.Extensions.Configuration;

namespace BlackBoot.Services.Implementations;

public class CoinPriceService : ICoinPriceService
{
    private readonly IConfiguration _configuration;
    public CoinPriceService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<ActionResponse<List<CoinPriceDto>>> GetPrice(string symbol)
    {
        var url = _configuration.GetSection("CoinListProvider:PriceUrl").Value;
        var parameters = new Dictionary<string, string>()
                {
                    {"vs_currency", "usd" },
                    {"ids", symbol },
                    {"order", "market_cap_desc" },
                    {"per_page", "100" },
                    {"page", "1" },
                    {"sparkline", "false" }
                };
        var inquiryCoinPrice = await HttpRequest.GetAsync<List<CoinPriceDto>>(url, parameters);
        return new ActionResponse<List<CoinPriceDto>>(inquiryCoinPrice);
    }
}
