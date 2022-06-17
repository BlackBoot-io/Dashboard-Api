namespace BlackBoot.Services.Interfaces;

public interface INotificationService : IScopedDependency
{
    Task<IActionResponse<List<Notification>>> GetAllAsync(Guid userId,CancellationToken cancellationToken);
    Task<IActionResponse<int>> AddAsync(Notification notification, CancellationToken cancellation);
    Task<IActionResponse> DeleteAsync(Guid userId, int Id, CancellationToken cancellation);
    Task<IActionResponse> CountAsync(Guid userId, CancellationToken cancellation);
}
