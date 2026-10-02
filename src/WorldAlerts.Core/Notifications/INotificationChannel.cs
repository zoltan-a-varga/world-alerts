using WorldAlerts.Core.Common;

namespace WorldAlerts.Core.Notifications;

public interface INotificationChannel
{
    NotificationChannelType Type { get; }

    Task SendAsync(
        Notification notification,
        CancellationToken cancellationToken);
}