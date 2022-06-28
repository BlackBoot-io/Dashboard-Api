namespace BlackBoot.Services.Interfaces;

public interface ICoinPriceService : IScopedDependency
{
    Task<ActionResponse<CoinPriceDto>> GetPrice(string symbol);
}

