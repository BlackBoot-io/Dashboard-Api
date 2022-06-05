﻿using Microsoft.AspNetCore.Http;

namespace BlackBoot.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly BlackBootDBContext _context;
    private readonly DbSet<Notification> _notifications;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotificationService(BlackBootDBContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _notifications = context.Set<Notification>();
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResponse<List<Notification>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var currentUserId = _httpContextAccessor?.HttpContext?.User?.Identity?.GetUserIdAsGuid();
        var notifications = await _notifications.AsNoTracking().Where(x => x.Target == MessageTarget.All || x.UserId == currentUserId).ToListAsync(cancellationToken);
        return new ActionResponse<List<Notification>>(notifications);
    }
    public async Task<IActionResponse<Notification>> AddAsync(Notification notification, CancellationToken cancellation)
    {
        await _notifications.AddAsync(notification, cancellation);
        await _context.SaveChangesAsync(cancellation);

        return new ActionResponse<Notification>(notification);
    }

    public async Task<IActionResponse> DeleteAsync(int Id, CancellationToken cancellation)
    {
        var notification = await _notifications.FindAsync(new object[] { Id }, cancellation);
        if (notification is null)
            return new ActionResponse(ActionResponseStatusCode.NotFound);

        _notifications.Remove(notification);
        await _context.SaveChangesAsync(cancellation);

        return new ActionResponse();
    }
}
