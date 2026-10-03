using WorldAlerts.Core.Common;

namespace WorldAlerts.Core.Notifications;

public sealed class NotificationAttempt
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid AlertId { get; init; }

    public Guid EventId { get; init; }

    public required string UserId { get; init; }

    public NotificationChannelType Channel { get; init; }

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}