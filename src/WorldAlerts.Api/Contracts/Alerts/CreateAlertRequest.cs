using System.ComponentModel.DataAnnotations;
using WorldAlerts.Core.Common;

namespace WorldAlerts.Api.Contracts.Alerts;

public sealed class CreateAlertRequest
{
    [Required]
    [MaxLength(100)]
    public string UserId { get; init; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    public EventCategory EventCategory { get; init; }

    public EventSeverity MinimumSeverity { get; init; }

    [MinLength(1)]
    public IReadOnlyCollection<NotificationChannelType> NotificationChannels { get; init; }
        = Array.Empty<NotificationChannelType>();
}