namespace BlackBoot.Api.Controllers;

public class CrowdSaleScheduleController : BaseController
{
    private readonly ICrowdSaleScheduleService _crowdSaleScheduleService;

    public CrowdSaleScheduleController(ICrowdSaleScheduleService crowdSaleScheduleService)
        => _crowdSaleScheduleService = crowdSaleScheduleService;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(await _crowdSaleScheduleService.GetAllAsync(cancellationToken));
}