# Backend Design

## Overview

The implementation is a small ASP.NET Core backend demonstrating the complete alert workflow:

```text
Event
  |
  v
Alert matching
  |
  v
Notification dispatch
  |
  +----> Email
  |
  +----> Slack
```

The design deliberately avoids production-scale infrastructure and focuses on a testable vertical slice.

## Domain Model

### Event

Represents a normalized external event.

Initial properties:

- Id
- Title
- Category
- Severity
- OccurredAt
- Description

The prototype does not attempt to determine whether an event is objectively important. Importance is defined by whether the event matches a user's configured alert.

### Alert

Represents a user's notification rule.

Initial properties:

- Id
- UserId
- Name
- EventCategory
- MinimumSeverity
- Enabled
- NotificationChannels

For the prototype, alert matching intentionally remains simple.

An event matches an alert when:

1. the alert is enabled;
2. the event category matches the configured category;
3. the event severity is equal to or greater than the configured minimum severity.

A more general rule engine is deliberately not introduced.

## Notification Channels

Notification delivery is represented by a common abstraction:

```csharp
public interface INotificationChannel
{
    NotificationChannelType Type { get; }

    Task SendAsync(
        Notification notification,
        CancellationToken cancellationToken);
}
```

Initial implementations:

- Email
- Slack

The alert evaluation logic depends on the abstraction rather than individual channel implementations.

This allows another channel to be added without modifying the matching logic.

## Event Processing

The main workflow is:

```text
Receive Event
      |
      v
Load Enabled Alerts
      |
      v
Evaluate Conditions
      |
      v
Matching Alerts
      |
      v
Create Notification
      |
      v
Dispatch to configured channels
```

External event-provider integration is outside the scope of the prototype.

An API endpoint will allow events to be submitted for demonstration and testing.

## REST API

Initial endpoints:

### Alerts

```text
POST   /api/alerts
GET    /api/alerts
GET    /api/alerts/{id}
PUT    /api/alerts/{id}
DELETE /api/alerts/{id}
```

### Events

```text
POST /api/events
```

Submitting an event starts alert evaluation.

### Administration

```text
GET /api/admin/alerts
GET /api/admin/notifications
```

The administrative API is intentionally minimal. Its purpose is to expose system state without introducing a frontend application.

## Persistence

SQLite with Entity Framework Core will be used.

Reasons:

- no external database installation is required;
- the repository remains easy to run;
- EF Core behaviour can still be demonstrated;
- relational persistence is sufficient for the prototype.

Moving to PostgreSQL or SQL Server would primarily be an infrastructure change and is not necessary to demonstrate the core design.

## Project Structure

The solution will use three production projects:

```text
src/
    WorldAlerts.Api
    WorldAlerts.Core
    WorldAlerts.Infrastructure

tests/
    WorldAlerts.Tests
```

### WorldAlerts.Core

Contains:

- domain models;
- business rules;
- interfaces required by the core workflow.

### WorldAlerts.Infrastructure

Contains:

- EF Core persistence;
- notification channel implementations;
- infrastructure-specific services.

### WorldAlerts.Api

Contains:

- HTTP endpoints/controllers;
- request/response models;
- dependency injection configuration;
- application startup.

### WorldAlerts.Tests

Contains automated tests for the core behaviour and selected API/infrastructure behaviour.

The structure intentionally avoids additional layers until a concrete need for them appears.

## Testing

Priority will be given to business behaviour rather than maximizing test count.

Important scenarios:

- matching event triggers notification;
- category mismatch does not trigger notification;
- severity below threshold does not trigger notification;
- disabled alert is ignored;
- multiple configured channels are dispatched;
- invalid API input is rejected.

## Known Limitations

The prototype does not provide:

- authentication or authorization;
- real external event ingestion;
- production email/Slack credentials or guaranteed delivery;
- distributed processing;
- retry queues;
- exactly-once notification delivery;
- sophisticated alert expressions;
- graphical administration UI.

These are conscious scope decisions rather than assumed production characteristics.