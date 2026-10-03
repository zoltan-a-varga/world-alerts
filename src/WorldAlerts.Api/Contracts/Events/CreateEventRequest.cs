using System.ComponentModel.DataAnnotations;
using WorldAlerts.Core.Common;

namespace WorldAlerts.Api.Contracts.Events;

public sealed class CreateEventRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; init; } = string.Empty;

    public EventCategory Category { get; init; }

    public EventSeverity Severity { get; init; }

    public DateTimeOffset? OccurredAt { get; init; }
}