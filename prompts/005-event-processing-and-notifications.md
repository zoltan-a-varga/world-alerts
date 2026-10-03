# 005 --- Event Processing and Notifications

**Record type:** Reconstructed interaction summary\
**Stage:** End-to-end event processing

> This file summarizes the actual AI-assisted implementation discussion.
> It is not presented as a verbatim historical prompt.

## Development request

Implement the vertical slice:

``` text
Submit event
    -> load enabled alerts
    -> evaluate matching rules
    -> dispatch through configured channels
```

Support both Email and Slack while keeping the channel mechanism
extensible.

## Agreed implementation

`EventProcessingService` orchestrates the workflow.

It:

1.  loads enabled alerts,
2.  evaluates them with `AlertMatcher`,
3.  creates a notification for each matching alert,
4.  resolves the configured `INotificationChannel` implementations,
5.  calls each selected channel.

`EmailNotificationChannel` and `SlackNotificationChannel` are
Infrastructure implementations.

For the prototype they simulate delivery through structured application
logging rather than connecting to external providers.

## Why simulated delivery was accepted

Real providers would require credentials, secret/configuration handling,
and external dependencies. The exercise can demonstrate the
architecture, matching behavior, channel selection, and extensibility
deterministically without those dependencies.

The limitation is explicitly documented and is not represented as real
Email or Slack delivery.

## Testing

Tests were added for:

-   a matching alert configured for both channels,
-   a non-matching event producing no notification.

Small fake implementations were used rather than introducing a mocking
framework.

## Runtime issue found

The application compiled, but the event endpoint initially failed with
HTTP 500 because `EventProcessingService` had not been registered with
the ASP.NET Core DI container.

The missing registration was added and the endpoint was retested through
Swagger.

This demonstrated that successful compilation does not validate the
complete runtime dependency graph.
