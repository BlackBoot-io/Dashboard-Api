namespace BlackBoot.Services.Implementations;

public class CrowdSaleScheduleService : ICrowdSaleScheduleService
{
    private readonly DbSet<CrowdSaleSchedule> _crowdSaleSchedules;

    public CrowdSaleScheduleService(BlackBootDBContext context)
        => _crowdSaleSchedules = context.Set<CrowdSaleSchedule>();

    public async Task<IActionResponse<List<CrowdSaleSchedule>>> GetAllAsync(CancellationToken cancellationToken)
        => new ActionResponse<List<CrowdSaleSchedule>>(await _crowdSaleSchedules.AsNoTracking().ToListAsync(cancellationToken));

    public async Task<CrowdSaleSchedule> GetCurrentSale()
      => await _crowdSaleSchedules.FirstOrDefaultAsync(X => X.IsActive);

}
