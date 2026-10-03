using WorldAlerts.Core.Common;

namespace WorldAlerts.Core.Alerts;

public sealed class Alert
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string UserId { get; init; }

    public required string Name { get; set; }

    public EventCategory EventCategory { get; set; }

    public EventSeverity MinimumSeverity { get; set; }

    public bool Enabled { get; set; } = true;

    public IReadOnlyCollection<NotificationChannelType> NotificationChannels { get; set; }
        = Array.Empty<NotificationChannelType>();
}