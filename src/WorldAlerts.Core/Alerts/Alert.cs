using WorldAlerts.Core.Common;

namespace WorldAlerts.Core.Alerts;

public sealed class Alert
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string UserId { get; init; }

    public required string Name { get; init; }

    public EventCategory EventCategory { get; init; }

    public EventSeverity MinimumSeverity { get; init; }

    public bool Enabled { get; set; } = true;

    public IReadOnlyCollection<NotificationChannelType> NotificationChannels { get; init; }
        = Array.Empty<NotificationChannelType>();
}