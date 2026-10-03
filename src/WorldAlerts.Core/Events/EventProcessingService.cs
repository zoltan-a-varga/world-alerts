using WorldAlerts.Core.Alerts;
using WorldAlerts.Core.Notifications;

namespace WorldAlerts.Core.Events;

public sealed class EventProcessingService
{
    private readonly IAlertRepository _alertRepository;
    private readonly AlertMatcher _alertMatcher;
    private readonly IReadOnlyDictionary<
        Common.NotificationChannelType,
        INotificationChannel> _notificationChannels;

    public EventProcessingService(
        IAlertRepository alertRepository,
        AlertMatcher alertMatcher,
        IEnumerable<INotificationChannel> notificationChannels)
    {
        _alertRepository = alertRepository;
        _alertMatcher = alertMatcher;

        _notificationChannels = notificationChannels
            .ToDictionary(x => x.Type);
    }

    public async Task<int> ProcessAsync(
        WorldEvent worldEvent,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(worldEvent);

        var alerts =
            await _alertRepository.GetEnabledAsync(cancellationToken);

        var notificationCount = 0;

        foreach (var alert in alerts)
        {
            if (!_alertMatcher.IsMatch(alert, worldEvent))
            {
                continue;
            }

            var notification = new Notification
            {
                AlertId = alert.Id,
                EventId = worldEvent.Id,
                UserId = alert.UserId,
                Subject = worldEvent.Title,
                Message = worldEvent.Description
            };

            foreach (var channelType in alert.NotificationChannels)
            {
                if (!_notificationChannels.TryGetValue(
                        channelType,
                        out var channel))
                {
                    continue;
                }

                await channel.SendAsync(
                    notification,
                    cancellationToken);

                notificationCount++;
            }
        }

        return notificationCount;
    }
}