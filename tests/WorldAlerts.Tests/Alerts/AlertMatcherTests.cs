using WorldAlerts.Core.Alerts;
using WorldAlerts.Core.Common;
using WorldAlerts.Core.Events;

namespace WorldAlerts.Tests.Alerts;

public sealed class AlertMatcherTests
{
    private readonly AlertMatcher _sut = new();

    [Fact]
    public void IsMatch_WhenCategoryAndSeverityMatch_ReturnsTrue()
    {
        var alert = CreateAlert(
            EventCategory.NaturalDisaster,
            EventSeverity.High);

        var worldEvent = CreateEvent(
            EventCategory.NaturalDisaster,
            EventSeverity.High);

        var result = _sut.IsMatch(alert, worldEvent);

        Assert.True(result);
    }

    [Fact]
    public void IsMatch_WhenEventSeverityIsHigherThanMinimum_ReturnsTrue()
    {
        var alert = CreateAlert(
            EventCategory.NaturalDisaster,
            EventSeverity.Medium);

        var worldEvent = CreateEvent(
            EventCategory.NaturalDisaster,
            EventSeverity.Critical);

        var result = _sut.IsMatch(alert, worldEvent);

        Assert.True(result);
    }

    [Fact]
    public void IsMatch_WhenEventSeverityIsBelowMinimum_ReturnsFalse()
    {
        var alert = CreateAlert(
            EventCategory.MarketMovement,
            EventSeverity.High);

        var worldEvent = CreateEvent(
            EventCategory.MarketMovement,
            EventSeverity.Medium);

        var result = _sut.IsMatch(alert, worldEvent);

        Assert.False(result);
    }

    [Fact]
    public void IsMatch_WhenCategoryDoesNotMatch_ReturnsFalse()
    {
        var alert = CreateAlert(
            EventCategory.NaturalDisaster,
            EventSeverity.Low);

        var worldEvent = CreateEvent(
            EventCategory.BreakingNews,
            EventSeverity.Critical);

        var result = _sut.IsMatch(alert, worldEvent);

        Assert.False(result);
    }

    [Fact]
    public void IsMatch_WhenAlertIsDisabled_ReturnsFalse()
    {
        var alert = CreateAlert(
            EventCategory.NaturalDisaster,
            EventSeverity.High,
            enabled: false);

        var worldEvent = CreateEvent(
            EventCategory.NaturalDisaster,
            EventSeverity.Critical);

        var result = _sut.IsMatch(alert, worldEvent);

        Assert.False(result);
    }

    private static Alert CreateAlert(
        EventCategory category,
        EventSeverity minimumSeverity,
        bool enabled = true)
    {
        return new Alert
        {
            UserId = "test-user",
            Name = "Test alert",
            EventCategory = category,
            MinimumSeverity = minimumSeverity,
            Enabled = enabled,
            NotificationChannels =
            [
                NotificationChannelType.Email
            ]
        };
    }

    private static WorldEvent CreateEvent(
        EventCategory category,
        EventSeverity severity)
    {
        return new WorldEvent
        {
            Title = "Test event",
            Description = "Test event description",
            Category = category,
            Severity = severity,
            OccurredAt = DateTimeOffset.UtcNow
        };
    }
}