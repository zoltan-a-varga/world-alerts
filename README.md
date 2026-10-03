# World Alerts

Backend-focused implementation of the **Feature Design & Build from a
Vague Brief** technical exercise.

The project turns an intentionally vague product request into a small
working .NET backend while documenting the decisions, assumptions,
AI-assisted development process, validation, and course corrections made
along the way.

## Product brief

The requested system allows users to configure alerts for important
world events, such as breaking news, market movements, and natural
disasters, and receive notifications through Email or Slack. The design
should also make additional notification channels easy to add later and
provide administrative visibility.

The original brief is preserved in
[`docs/original-brief.md`](docs/original-brief.md).

## Scope

This implementation deliberately focuses on a small, demonstrable
backend prototype.

Included:

-   alert creation, retrieval, update, and deletion,
-   SQLite persistence,
-   category and minimum-severity alert conditions,
-   event submission through an API,
-   matching events against enabled alerts,
-   Email and Slack notification channel abstractions,
-   simulated Email and Slack delivery through application logging,
-   notification attempt auditing,
-   administrative REST endpoints,
-   Swagger/OpenAPI,
-   automated tests for the core matching and processing behavior.

Deliberately out of scope:

-   graphical frontend or admin UI,
-   authentication and production user identity,
-   real news, market, or disaster data providers,
-   real Email or Slack provider credentials/integration,
-   message queues and distributed infrastructure,
-   retries, idempotency, exactly-once delivery, and transactional
    outbox patterns,
-   production deployment and operational monitoring.

The administrative requirement is represented by REST endpoints exposed
through Swagger rather than a separate frontend.

## Core workflow

``` text
Configure Alert
      |
      v
SQLite persistence

Submit WorldEvent
      |
      v
Load enabled alerts
      |
      v
AlertMatcher
      |
      v
Matching alert
      |
      v
INotificationChannel
     / \
    /   \
 Email  Slack
    \   /
     \ /
Notification audit
```

An event matches an alert when:

1.  the alert is enabled,
2.  the event category equals the configured category,
3.  the event severity is greater than or equal to the configured
    minimum severity.

Severity ordering is:

``` text
Low < Medium < High < Critical
```

## Architecture

The solution is intentionally a lightweight modular monolith rather than
a distributed system.

``` text
src/
  WorldAlerts.Api
  WorldAlerts.Core
  WorldAlerts.Infrastructure

tests/
  WorldAlerts.Tests
```

### WorldAlerts.Core

Contains the application/domain concepts and abstractions:

-   `Alert`
-   `WorldEvent`
-   `AlertMatcher`
-   `EventProcessingService`
-   `Notification`
-   `NotificationAttempt`
-   `IAlertRepository`
-   `INotificationAttemptRepository`
-   `INotificationChannel`

The Core project does not depend on infrastructure implementations.

### WorldAlerts.Infrastructure

Contains infrastructure concerns:

-   EF Core / SQLite persistence,
-   repository implementations,
-   database migrations,
-   `EmailNotificationChannel`,
-   `SlackNotificationChannel`.

The Email and Slack implementations currently log delivery rather than
contacting external providers. This keeps the exercise deterministic and
credential-free while still demonstrating channel selection and
extensibility.

### WorldAlerts.Api

Contains:

-   REST controllers,
-   API request contracts,
-   dependency injection configuration,
-   Swagger/OpenAPI configuration.

## Technology

-   .NET 8
-   ASP.NET Core Web API
-   Entity Framework Core 8
-   SQLite
-   Swashbuckle / Swagger
-   xUnit

## Running the application

### Prerequisites

-   .NET 8 SDK
-   `dotnet-ef` 8.x

Restore and build the solution:

``` powershell
dotnet restore
dotnet build
```

Apply the database migrations:

``` powershell
dotnet ef database update `
  --project src/WorldAlerts.Infrastructure `
  --startup-project src/WorldAlerts.Api
```

Run the API:

``` powershell
dotnet run --project src/WorldAlerts.Api
```

Open the Swagger UI using the local URL printed by ASP.NET Core, for
example:

``` text
http://localhost:5071/swagger
```

The exact port can vary by local configuration.

The SQLite database is created locally and is intentionally excluded
from source control.

## API overview

### Alerts

``` text
GET    /api/alerts
GET    /api/alerts/{id}
POST   /api/alerts
PUT    /api/alerts/{id}
DELETE /api/alerts/{id}
```

Example alert:

``` json
{
  "userId": "user-001",
  "name": "Critical natural disasters",
  "eventCategory": 3,
  "minimumSeverity": 3,
  "notificationChannels": [1, 2]
}
```

For the current enums:

``` text
EventCategory:
1 = BreakingNews
2 = MarketMovement
3 = NaturalDisaster

EventSeverity:
1 = Low
2 = Medium
3 = High
4 = Critical

NotificationChannelType:
1 = Email
2 = Slack
```

### Events

``` text
POST /api/events
```

Example matching event:

``` json
{
  "title": "Major earthquake",
  "description": "A major earthquake was detected.",
  "category": 3,
  "severity": 4
}
```

For an alert configured with both Email and Slack, a matching event
returns a response similar to:

``` json
{
  "eventId": "generated-guid",
  "matchedNotifications": 2
}
```

The Email and Slack prototype adapters write delivery information to the
application log.

### Admin

``` text
GET /api/admin/alerts
GET /api/admin/notifications
```

These endpoints provide the backend interpretation of the requested
admin view.

`/api/admin/notifications` exposes notification attempts recorded after
a notification channel completes successfully.

## Tests

Run all automated tests with:

``` powershell
dotnet test
```

Tests cover the most important business behavior, including:

-   exact severity matching,
-   severity threshold matching,
-   severity below the configured threshold,
-   category mismatch,
-   disabled alerts,
-   dispatch to multiple configured notification channels,
-   no dispatch for a non-matching event,
-   notification audit creation during successful processing.

Manual end-to-end validation was also performed through Swagger against
the SQLite-backed application.

## AI-assisted development process

AI was used as a development assistant rather than as an unchecked code
generator.

The process was:

``` text
Vague brief
    |
    v
Requirements and assumptions
    |
    v
Backend design
    |
    v
Incremental implementation
    |
    v
Build + automated tests
    |
    v
Manual runtime validation
    |
    v
Review and course correction
```

Prompt history is kept in [`prompts`](prompts/).

Planning, requirements, design decisions, and validation evidence are
kept in [`docs`](docs/).

Several generated suggestions were modified or rejected after
validation. Examples include:

-   changing mutable alert configuration from `init` to `set`,
-   diagnosing a Swagger UI/OpenAPI compatibility problem rather than
    continuing to change valid Swagger configuration,
-   adding a missing runtime dependency injection registration,
-   replacing unsupported SQLite server-side `DateTimeOffset` ordering
    with client-side ordering for the small prototype dataset.

See
[`docs/05-validation-and-review.md`](docs/05-validation-and-review.md)
for the detailed review.

## Key design decisions

### Simple alert rules

The prototype supports category equality and minimum severity rather
than introducing a generic rules engine.

This is enough to demonstrate the requested behavior without adding
speculative complexity.

### Extensible notification channels

Notification delivery is accessed through `INotificationChannel`.

`EventProcessingService` works with the abstraction and selects
implementations by `NotificationChannelType`, so another channel can be
added without changing the core alert-matching logic.

### SQLite

SQLite was selected to provide real persistence without requiring
external infrastructure.

One provider-specific limitation was discovered during runtime testing:
SQLite cannot translate the `DateTimeOffset` ordering used by the
notification audit query. For this small prototype, audit records are
loaded and then ordered with LINQ to Objects. This would need a
different persistence/query strategy at production scale.

### Notification channel storage

Configured channel values are stored using an EF Core value converter
rather than a normalized child table.

This keeps the prototype small. A normalized model would be preferable
if channel configuration or querying became more complex.

## Production considerations

The current implementation demonstrates the feature flow but is not
intended as a production-ready notification platform.

A production implementation would need to address, among other things:

-   authentication and authorization,
-   user/contact destination management,
-   real external event ingestion,
-   real Email and Slack provider integrations,
-   secret management,
-   asynchronous durable message processing,
-   retries and failure handling,
-   idempotency and duplicate-event handling,
-   transactional consistency/outbox patterns,
-   pagination for administrative queries,
-   structured monitoring, metrics, and tracing,
-   rate limiting and provider-specific delivery constraints.

These were intentionally not implemented because the exercise
prioritizes a clear, working prototype and documented reasoning over
speculative infrastructure.

## Documentation

The repository contains the development artifacts used during the
exercise:

-   [`docs/original-brief.md`](docs/original-brief.md) --- original
    product brief
-   [`docs/01-plan.md`](docs/01-plan.md) --- implementation plan
-   [`docs/02-requirements-and-assumptions.md`](docs/02-requirements-and-assumptions.md)
    --- requirements and explicit assumptions
-   [`docs/03-design.md`](docs/03-design.md) --- backend design
-   [`docs/04-decision-log.md`](docs/04-decision-log.md) --- key
    technical decisions
-   [`docs/05-validation-and-review.md`](docs/05-validation-and-review.md)
    --- validation, rejected/modified AI suggestions, and course
    corrections
-   [`prompts/`](prompts/) --- AI prompt history

## Status

The planned backend prototype is complete.

The implementation has been built, tested, exercised through Swagger,
and validated against SQLite. Known limitations and production gaps are
documented rather than hidden behind additional prototype complexity.
