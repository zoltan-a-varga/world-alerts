# 003 --- Core Domain and Alert Matching

**Record type:** Reconstructed interaction summary\
**Stage:** Core implementation

> This file summarizes the actual AI-assisted implementation discussion.
> It is not presented as a verbatim historical prompt.

## Development request

Implement the smallest useful core domain for the agreed backend design
in .NET 8.

The implementation should cover:

-   alerts,
-   normalized world events,
-   event categories,
-   severity levels,
-   Email and Slack channel types,
-   alert matching,
-   notification abstractions.

Keep the Core project independent of infrastructure concerns and avoid
unnecessary enterprise patterns.

## Constraints carried forward

-   Backend only.
-   No authentication implementation; `UserId` is a placeholder.
-   No external event provider.
-   Matching rules are deliberately simple.
-   An alert matches when it is enabled, the category matches, and event
    severity is at least the configured minimum.
-   Notification delivery must be abstracted so additional channels can
    be added later.

## Resulting implementation

The discussion resulted in core types including:

-   `Alert`
-   `WorldEvent`
-   `EventCategory`
-   `EventSeverity`
-   `NotificationChannelType`
-   `AlertMatcher`
-   `Notification`
-   `INotificationChannel`
-   `IAlertRepository`

Unit tests were added around the matching behavior.

## Human review / course correction

The severity comparison relies on the numeric ordering of the severity
enum. Rather than accepting that assumption silently, tests were added
for equal, higher, and lower severity values.

The initial alert model also used `init` properties for configuration
that later needed to be changed through the PUT endpoint. Mutable alert
configuration was therefore changed to `set`, while identity-related
values remained immutable.

See `docs/05-validation-and-review.md` for the detailed validation
record.
