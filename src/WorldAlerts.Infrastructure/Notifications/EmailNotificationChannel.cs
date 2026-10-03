using Microsoft.Extensions.Logging;
using WorldAlerts.Core.Common;
using WorldAlerts.Core.Notifications;

namespace WorldAlerts.Infrastructure.Notifications;

public sealed class EmailNotificationChannel : INotificationChannel
{
    private readonly ILogger<EmailNotificationChannel> _logger;

    public EmailNotificationChannel(
        ILogger<EmailNotificationChannel> logger)
    {
        _logger = logger;
    }

    public NotificationChannelType Type =>
        NotificationChannelType.Email;

    public Task SendAsync(
        Notification notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "EMAIL notification sent. UserId={UserId}, AlertId={AlertId}, EventId={EventId}, Subject={Subject}",
            notification.UserId,
            notification.AlertId,
            notification.EventId,
            notification.Subject);

        return Task.CompletedTask;
    }
}