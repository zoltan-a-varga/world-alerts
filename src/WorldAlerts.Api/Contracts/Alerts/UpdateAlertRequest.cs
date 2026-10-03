using System.ComponentModel.DataAnnotations;
using WorldAlerts.Core.Common;

namespace WorldAlerts.Api.Contracts.Alerts;

public sealed class UpdateAlertRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    public EventCategory EventCategory { get; init; }

    public EventSeverity MinimumSeverity { get; init; }

    public bool Enabled { get; init; }

    [MinLength(1)]
    public IReadOnlyCollection<NotificationChannelType> NotificationChannels { get; init; }
        = Array.Empty<NotificationChannelType>();
}