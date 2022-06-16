namespace BlackBoot.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly BlackBootDBContext _context;
    private readonly DbSet<Notification> _notifications;

    public NotificationService(BlackBootDBContext context)
    {
        _context = context;
        _notifications = context.Set<Notification>();
    }
    
    public async Task<IActionResponse<List<Notification>>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        var notifications = await _notifications.AsNoTracking()
           .Where(x => x.Target == MessageTarget.All || x.UserId == userId)
           .ToListAsync(cancellationToken);
        return new ActionResponse<List<Notification>>(notifications);
    }
    public async Task<IActionResponse<int>> AddAsync(Notification notification, CancellationToken cancellation)
    {
        notification.Date = DateTime.UtcNow;
        await _notifications.AddAsync(notification, cancellation);
        var dbResult = await _context.SaveChangesAsync(cancellation);
        if (!dbResult.ToSaveChangeResult())
            return new ActionResponse<int>(ActionResponseStatusCode.ServerError);

        return new ActionResponse<int>(notification.NotificationId);
    }
    public async Task<IActionResponse> DeleteAsync(Guid userId, int Id, CancellationToken cancellation)
    {
        var notification = await _notifications.FirstOrDefaultAsync(X => X.NotificationId == Id && X.UserId == userId, cancellation);
        if (notification is null)
            return new ActionResponse(ActionResponseStatusCode.NotFound);

        _notifications.Remove(notification);
        var dbResult = await _context.SaveChangesAsync(cancellation);
        if (!dbResult.ToSaveChangeResult())
            return new ActionResponse<Notification>(ActionResponseStatusCode.ServerError);

        return new ActionResponse();
    }
}
