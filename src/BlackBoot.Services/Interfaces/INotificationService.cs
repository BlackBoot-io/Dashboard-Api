namespace BlackBoot.Services.Interfaces;

public interface INotificationService : IScopedDependency
{
    Task<IActionResponse<List<Notification>>> GetAllAsync(CancellationToken cancellationToken);
    Task<IActionResponse<Notification>> AddAsync(Notification notification, CancellationToken cancellation);
    Task<IActionResponse> DeleteAsync(int Id, CancellationToken cancellation);
}
