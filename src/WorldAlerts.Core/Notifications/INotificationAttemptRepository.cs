namespace WorldAlerts.Core.Notifications;

public interface INotificationAttemptRepository
{
    Task AddAsync(
        NotificationAttempt attempt,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<NotificationAttempt>> GetAllAsync(
        CancellationToken cancellationToken);
}