namespace BlackBoot.Api.Controllers;

public class CrowdSaleScheduleController : BaseController
{
    private readonly ICrowdSaleScheduleService _crowdSaleScheduleService;

    public CrowdSaleScheduleController(ICrowdSaleScheduleService crowdSaleScheduleService)
        => _crowdSaleScheduleService = crowdSaleScheduleService;

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(await _crowdSaleScheduleService.GetAllAsync(cancellationToken));

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetCurrentSaleAsync()
       => Ok(await _crowdSaleScheduleService.GetCurrentSaleAsync());
}