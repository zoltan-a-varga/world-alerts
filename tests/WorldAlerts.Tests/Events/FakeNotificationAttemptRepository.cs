using WorldAlerts.Core.Notifications;

namespace WorldAlerts.Tests.Events;

public sealed class FakeNotificationAttemptRepository
    : INotificationAttemptRepository
{
    public List<NotificationAttempt> Attempts { get; } = [];

    public Task AddAsync(
        NotificationAttempt attempt,
        CancellationToken cancellationToken)
    {
        Attempts.Add(attempt);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<NotificationAttempt>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<NotificationAttempt> result =
            Attempts.ToArray();

        return Task.FromResult(result);
    }
}