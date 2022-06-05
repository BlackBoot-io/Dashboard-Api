namespace BlackBoot.Api.Controllers;

public class NotificationController : BaseController
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
        => _notificationService = notificationService;

    [HttpGet]
    public async Task<ActionResult<List<Notification>>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
        => Ok(await _notificationService.GetAllAsync(userId, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<List<Notification>>> AddAsync(Notification notification, CancellationToken cancellationToken)
        => Ok(await _notificationService.AddAsync(notification, cancellationToken));

    [HttpDelete]
    public async Task<ActionResult<List<Notification>>> DeleteAsync(int id, CancellationToken cancellationToken)
        => Ok(await _notificationService.DeleteAsync(id, cancellationToken));
}