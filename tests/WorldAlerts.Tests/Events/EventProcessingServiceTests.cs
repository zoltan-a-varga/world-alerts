using WorldAlerts.Core.Alerts;
using WorldAlerts.Core.Common;
using WorldAlerts.Core.Events;
using WorldAlerts.Core.Notifications;

namespace WorldAlerts.Tests.Events;

public sealed class EventProcessingServiceTests
{
    [Fact]
    public async Task ProcessAsync_WhenAlertMatches_SendsToAllConfiguredChannels()
    {
        var alert = CreateAlert(
            EventCategory.NaturalDisaster,
            EventSeverity.High,
            NotificationChannelType.Email,
            NotificationChannelType.Slack);

        var repository = new FakeAlertRepository([alert]);

        var email = new FakeNotificationChannel(NotificationChannelType.Email);
        var slack = new FakeNotificationChannel(NotificationChannelType.Slack);

        var sut = new EventProcessingService(
            repository,
            new AlertMatcher(),
            [email, slack]);

        var worldEvent = CreateEvent(
            EventCategory.NaturalDisaster,
            EventSeverity.Critical);

        var count = await sut.ProcessAsync(
            worldEvent,
            CancellationToken.None);

        Assert.Equal(2, count);
        Assert.Single(email.SentNotifications);
        Assert.Single(slack.SentNotifications);
    }

    [Fact]
    public async Task ProcessAsync_WhenAlertDoesNotMatch_SendsNoNotification()
    {
        var alert = CreateAlert(
            EventCategory.NaturalDisaster,
            EventSeverity.High,
            NotificationChannelType.Email);

        var repository = new FakeAlertRepository([alert]);
        var email = new FakeNotificationChannel(NotificationChannelType.Email);

        var sut = new EventProcessingService(
            repository,
            new AlertMatcher(),
            [email]);

        var worldEvent = CreateEvent(
            EventCategory.NaturalDisaster,
            EventSeverity.Low);

        var count = await sut.ProcessAsync(
            worldEvent,
            CancellationToken.None);

        Assert.Equal(0, count);
        Assert.Empty(email.SentNotifications);
    }

    private static Alert CreateAlert(
        EventCategory category,
        EventSeverity minimumSeverity,
        params NotificationChannelType[] channels)
    {
        return new Alert
        {
            UserId = "test-user",
            Name = "Test alert",
            EventCategory = category,
            MinimumSeverity = minimumSeverity,
            Enabled = true,
            NotificationChannels = channels
        };
    }

    private static WorldEvent CreateEvent(
        EventCategory category,
        EventSeverity severity)
    {
        return new WorldEvent
        {
            Title = "Test event",
            Description = "Test description",
            Category = category,
            Severity = severity,
            OccurredAt = DateTimeOffset.UtcNow
        };
    }

    private sealed class FakeNotificationChannel : INotificationChannel
    {
        public FakeNotificationChannel(NotificationChannelType type)
        {
            Type = type;
        }

        public NotificationChannelType Type { get; }

        public List<Notification> SentNotifications { get; } = [];

        public Task SendAsync(
            Notification notification,
            CancellationToken cancellationToken)
        {
            SentNotifications.Add(notification);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeAlertRepository : IAlertRepository
    {
        private readonly IReadOnlyCollection<Alert> _alerts;

        public FakeAlertRepository(IReadOnlyCollection<Alert> alerts)
        {
            _alerts = alerts;
        }

        public Task<IReadOnlyCollection<Alert>> GetEnabledAsync(
            CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Alert> result =
                _alerts.Where(x => x.Enabled).ToArray();

            return Task.FromResult(result);
        }

        public Task<IReadOnlyCollection<Alert>> GetAllAsync(
            CancellationToken cancellationToken)
            => Task.FromResult(_alerts);

        public Task<Alert?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
            => Task.FromResult(_alerts.FirstOrDefault(x => x.Id == id));

        public Task AddAsync(
            Alert alert,
            CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task UpdateAsync(
            Alert alert,
            CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task DeleteAsync(
            Alert alert,
            CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}