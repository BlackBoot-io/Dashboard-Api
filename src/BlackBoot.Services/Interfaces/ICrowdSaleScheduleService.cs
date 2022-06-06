namespace BlackBoot.Services.Interfaces;

public interface ICrowdSaleScheduleService : IScopedDependency
{
    Task<CrowdSaleSchedule> GetCurrentSale();
    Task<IActionResponse<List<CrowdSaleSchedule>>> GetAllAsync(CancellationToken cancellationToken);
}
