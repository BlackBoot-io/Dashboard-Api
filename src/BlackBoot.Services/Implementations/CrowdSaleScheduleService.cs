using BlackBoot.Shared.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackBoot.Services.Implementations;

public class CrowdSaleScheduleService : ICrowdSaleScheduleService
{
    private readonly BlackBootDBContext _context;
    private readonly DbSet<CrowdSaleSchedule> _crowdSaleSchedules;

    public CrowdSaleScheduleService(BlackBootDBContext context)
    {
        _context = context;
        _crowdSaleSchedules = context.Set<CrowdSaleSchedule>();
    }
    public async Task<List<CrowdSaleSchedule>> GetAllAsync(CancellationToken cancellationToken) => await _crowdSaleSchedules.AsNoTracking().ToListAsync(cancellationToken);
    public async Task<int> AddAsync(CrowdSaleSchedule crowdSaleSchedule, CancellationToken cancellationToken)
    {
        await _crowdSaleSchedules.AddAsync(crowdSaleSchedule, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return crowdSaleSchedule.CrowdSaleScheduleId;
    }
    public async Task<int> UpdateAsync(CrowdSaleSchedule crowdSaleSchedule, CancellationToken cancellationToken)
    {
        var model = await _crowdSaleSchedules.FindAsync(new object[] { crowdSaleSchedule.CrowdSaleScheduleId }, cancellationToken);
        if (model is null) throw new NotFoundException();

        //Todo:must be determined update fields

        await _context.SaveChangesAsync(cancellationToken);
        return model.CrowdSaleScheduleId;
    }
}
