namespace WorldAlerts.Core.Notifications;

public sealed class Notification
{
    public required Guid AlertId { get; init; }

    public required Guid EventId { get; init; }

    public required string UserId { get; init; }

    public required string Subject { get; init; }

    public required string Message { get; init; }
}