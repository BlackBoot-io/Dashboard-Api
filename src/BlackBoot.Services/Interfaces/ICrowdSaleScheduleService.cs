namespace BlackBoot.Services.Interfaces;

public interface ICrowdSaleScheduleService : IScopedDependency
{
    Task<List<CrowdSaleSchedule>> GetAllAsync(CancellationToken cancellationToken);
    Task<int> AddAsync(CrowdSaleSchedule crowdSaleSchedule, CancellationToken cancellationToken);
    Task<int> UpdateAsync(CrowdSaleSchedule crowdSaleSchedule, CancellationToken cancellationToken);
}
