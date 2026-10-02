using WorldAlerts.Core.Common;

namespace WorldAlerts.Core.Events;

public sealed class WorldEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Title { get; init; }

    public required string Description { get; init; }

    public EventCategory Category { get; init; }

    public EventSeverity Severity { get; init; }

    public DateTimeOffset OccurredAt { get; init; }
}