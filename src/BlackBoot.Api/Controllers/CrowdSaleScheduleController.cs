

using BlackBoot.Domain.Entities;

namespace BlackBoot.Api.Controllers;

[AllowAnonymous]
public class CrowdSaleScheduleController : BaseController
{
    private readonly ICrowdSaleScheduleService _crowdSaleScheduleService;

    public CrowdSaleScheduleController(ICrowdSaleScheduleService crowdSaleScheduleService) => _crowdSaleScheduleService = crowdSaleScheduleService;

    [HttpGet]
    public async Task<ActionResult<List<CrowdSaleSchedule>>> GetAllAsync(CancellationToken cancellationToken) => Ok(await _crowdSaleScheduleService.GetAllAsync(cancellationToken));
    [HttpPost]
    public async Task<ActionResult<int>> AddAsync(CrowdSaleSchedule crowdSaleSchedule, CancellationToken cancellationToken) => Ok(await _crowdSaleScheduleService.AddAsync(crowdSaleSchedule, cancellationToken));

    [HttpPut("{crowdSaleScheduleId}")]
    public async Task<ActionResult<int>> UpdateAsync(int crowdSaleScheduleId, CrowdSaleSchedule crowdSaleSchedule, CancellationToken cancellationToken)
    {
        if (crowdSaleScheduleId != crowdSaleSchedule.CrowdSaleScheduleId)
            return BadRequest();

        return Ok(await _crowdSaleScheduleService.UpdateAsync(crowdSaleSchedule, cancellationToken));
    }
}