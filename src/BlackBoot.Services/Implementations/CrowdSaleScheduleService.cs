using BlackBoot.Shared.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackBoot.Services.Implementations;

public class CrowdSaleScheduleService : ICrowdSaleScheduleService
{
    private readonly DbSet<CrowdSaleSchedule> _crowdSaleSchedules;

    public CrowdSaleScheduleService(BlackBootDBContext context) => _crowdSaleSchedules = context.Set<CrowdSaleSchedule>();
    public async Task<IActionResponse<List<CrowdSaleSchedule>>> GetAllAsync(CancellationToken cancellationToken) => new ActionResponse<List<CrowdSaleSchedule>>(await _crowdSaleSchedules.AsNoTracking().ToListAsync(cancellationToken));
}
