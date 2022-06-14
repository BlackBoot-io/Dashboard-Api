namespace BlackBoot.Services.Interfaces;

public interface ICrowdSaleScheduleService : IScopedDependency
{
    Task<IActionResponse<CrowdSaleSchedule>> GetCurrentSaleAsync();
    Task<IActionResponse<List<CrowdSaleSchedule>>> GetAllAsync(CancellationToken cancellationToken);
}
