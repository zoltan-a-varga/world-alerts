using Microsoft.Extensions.Logging;
using WorldAlerts.Core.Common;
using WorldAlerts.Core.Notifications;

namespace WorldAlerts.Infrastructure.Notifications;

public sealed class SlackNotificationChannel : INotificationChannel
{
    private readonly ILogger<SlackNotificationChannel> _logger;

    public SlackNotificationChannel(
        ILogger<SlackNotificationChannel> logger)
    {
        _logger = logger;
    }

    public NotificationChannelType Type =>
        NotificationChannelType.Slack;

    public Task SendAsync(
        Notification notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "SLACK notification sent. UserId={UserId}, AlertId={AlertId}, EventId={EventId}, Subject={Subject}",
            notification.UserId,
            notification.AlertId,
            notification.EventId,
            notification.Subject);

        return Task.CompletedTask;
    }
}