namespace BlackBoot.Api.Controllers;

public class CoinPriceController : BaseController
{
    private readonly ICoinPriceService _coinPriceService;
    public CoinPriceController(ICoinPriceService coinPriceService)
        => _coinPriceService = coinPriceService;

    [HttpGet]
    public async Task<IActionResult> GetAsync(string symbol)
    => Ok(await _coinPriceService.GetPrice(symbol));

}

