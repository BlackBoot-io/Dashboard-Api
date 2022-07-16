namespace BlackBoot.Services.Interfaces;

public interface ICoinPriceService : IScopedDependency
{
    Task<ActionResponse<List<CoinPriceDto>>> GetPrice(string symbol);
}