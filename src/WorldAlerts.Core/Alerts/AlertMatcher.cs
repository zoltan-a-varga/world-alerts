using WorldAlerts.Core.Events;

namespace WorldAlerts.Core.Alerts;

public sealed class AlertMatcher
{
    public bool IsMatch(Alert alert, WorldEvent worldEvent)
    {
        ArgumentNullException.ThrowIfNull(alert);
        ArgumentNullException.ThrowIfNull(worldEvent);

        if (!alert.Enabled)
        {
            return false;
        }

        if (alert.EventCategory != worldEvent.Category)
        {
            return false;
        }

        return worldEvent.Severity >= alert.MinimumSeverity;
    }
}