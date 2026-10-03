# Validation and AI Output Review

This document records how AI-generated suggestions were reviewed,
tested, accepted, modified, or rejected during implementation.

The goal was not to accept generated code blindly, but to validate
important assumptions through compilation, automated tests, database
migrations, and manual end-to-end testing.

## AI decision summary

  -----------------------------------------------------------------------
  AI suggestion           Decision                Reason
  ----------------------- ----------------------- -----------------------
  Category + severity     Accepted                Appropriate for the
  based matching                                  prototype and covered
                                                  by tests

  Severity comparison     Accepted after          Simple, but relies on
  through enum ordering   validation              explicit enum ordering

  Immutable alert         Modified                Existing alerts need to
  configuration (`init`)                          be editable

  SQLite for persistence  Accepted                Zero external
                                                  infrastructure and
                                                  sufficient for the
                                                  prototype

  Comma-separated         Accepted with           Keeps the prototype
  notification channel    limitation              small; normalization
  persistence                                     would be preferable at
                                                  scale

  Server-side ordering of Rejected after runtime  SQLite provider cannot
  `DateTimeOffset` in     test                    translate the
  SQLite                                          expression

  Swagger configuration   Rejected after          Generated OpenAPI
  as initial cause of     investigation           document was valid;
  rendering failure                               dependency
                                                  compatibility was the
                                                  issue

  Logging-based Email and Accepted as prototype   Demonstrates dispatch
  Slack adapters          scope                   and extensibility
                                                  without external
                                                  credentials
  -----------------------------------------------------------------------

## 1. Alert severity comparison

### AI suggestion

The alert matcher compares severity values using enum ordering:

``` csharp
return worldEvent.Severity >= alert.MinimumSeverity;
```

This assumes the enum values have a meaningful ordering:

-   Low = 1
-   Medium = 2
-   High = 3
-   Critical = 4

### Review

This approach is intentionally simple and appropriate for the prototype,
but the ordering assumption is implicit in the implementation.

### Validation

Unit tests were added for:

-   equal severity,
-   event severity above the configured minimum,
-   event severity below the configured minimum,
-   category mismatch,
-   disabled alerts.

### Decision

Accepted for the prototype after behavioral validation.

For a more complex severity model, explicit comparison rules could avoid
depending on enum numeric ordering.

------------------------------------------------------------------------

## 2. Mutable alert configuration

### Initial AI suggestion

The initial domain model used `init` properties for alert configuration.

During implementation of the PUT endpoint, this prevented the existing
alert configuration from being updated naturally.

### Review

Alert identity and ownership should remain immutable, while
configuration such as name, category, minimum severity, enabled state,
and notification channels is expected to change during the alert
lifecycle.

### Decision

Changed mutable configuration properties from `init` to `set`.

`Id` and `UserId` remain immutable.

This was a correction to the initial generated domain model rather than
working around the problem in the controller.

------------------------------------------------------------------------

## 3. Swagger / OpenAPI compatibility issue

### Problem

Swagger UI displayed:

> Unable to render this definition. The provided definition does not
> specify a valid version field.

The generated `/swagger/v1/swagger.json` was inspected directly and
contained:

``` json
{
  "openapi": "3.0.4"
}
```

Therefore the generated OpenAPI document itself contained a valid
version.

### Investigation

The initial hypothesis was an incorrect Swagger endpoint or
configuration.

That hypothesis was rejected after:

1.  opening the generated Swagger JSON directly,
2.  confirming the OpenAPI version field was present,
3.  confirming Swagger UI was loading the expected API definition,
4.  inspecting package versions.

The project was using Swashbuckle.AspNetCore 6.6.2.

### Decision

Swashbuckle was upgraded to 8.1.4.

After the upgrade, the same OpenAPI document rendered correctly.

### Lesson

A valid generated document does not guarantee compatibility with the
UI/tool used to consume it. Dependency versions were therefore included
in the investigation instead of continuing to modify otherwise-correct
Swagger configuration.

------------------------------------------------------------------------

## 4. Missing dependency injection registration

### Problem

The solution compiled successfully, but:

``` text
POST /api/events
```

returned HTTP 500 with:

``` text
Unable to resolve service for type
'WorldAlerts.Core.Events.EventProcessingService'
```

### Review

The controller depended on `EventProcessingService`, but the service had
not been registered with the ASP.NET Core dependency injection
container.

### Decision

Added:

``` csharp
builder.Services.AddScoped<EventProcessingService>();
```

and verified the endpoint again through Swagger.

### Lesson

Compilation and unit tests alone do not verify the runtime dependency
graph. Manual API testing exposed a configuration problem that static
compilation did not detect.

------------------------------------------------------------------------

## 5. SQLite DateTimeOffset ordering limitation

### Initial AI suggestion

The notification audit repository used:

``` csharp
.OrderByDescending(x => x.CreatedAt)
.ToListAsync(cancellationToken);
```

where `CreatedAt` is a `DateTimeOffset`.

### Problem

The code compiled successfully, but the SQLite EF Core provider threw a
`NotSupportedException` at runtime because it cannot translate this
`DateTimeOffset` ORDER BY expression.

### Decision

The records are retrieved first and ordered using LINQ to Objects:

``` csharp
var attempts = await _dbContext.NotificationAttempts
    .AsNoTracking()
    .ToListAsync(cancellationToken);

return attempts
    .OrderByDescending(x => x.CreatedAt)
    .ToArray();
```

### Trade-off

Client-side ordering would not be appropriate for a large audit table.

It is acceptable for this prototype because the dataset is deliberately
small. A production implementation would use a database representation
and query strategy that supports efficient server-side ordering and
pagination.

------------------------------------------------------------------------

## 6. Notification channel persistence

Alert notification channels are stored in SQLite as a comma-separated
value using an EF Core value converter.

### Review

A normalized child table such as `AlertNotificationChannel` would
provide a cleaner relational representation and better querying
capabilities.

### Decision

The value converter was retained because:

-   only two fixed channel types are required,
-   channel membership is loaded together with an alert,
-   no database queries by individual channel are required,
-   it keeps the prototype small.

A normalized representation would be preferred if channel configuration
became more complex.

------------------------------------------------------------------------

## 7. Email and Slack delivery

The prototype contains separate implementations of:

-   `EmailNotificationChannel`
-   `SlackNotificationChannel`

Both implement the common `INotificationChannel` abstraction.

### Important limitation

The implementations currently simulate delivery through application
logging. They do not connect to a real email provider or Slack API.

### Decision

This was an intentional scope decision.

Real delivery would require provider credentials and configuration and
would add external dependencies that are not necessary to demonstrate:

-   alert matching,
-   channel selection,
-   dispatch orchestration,
-   channel extensibility,
-   notification auditing.

A production implementation would replace these adapters with real
provider integrations without changing the core event-processing
workflow.

------------------------------------------------------------------------

## 8. Notification delivery semantics

A notification audit record is created after the channel's `SendAsync`
operation completes successfully.

This provides useful prototype visibility but does not implement
production delivery guarantees.

The current solution does not provide:

-   retries,
-   durable message queues,
-   idempotency,
-   exactly-once delivery,
-   dead-letter handling,
-   transactional outbox behavior.

These were deliberately kept outside the prototype scope.

A production design would likely separate event matching from
asynchronous notification delivery using durable messaging.

------------------------------------------------------------------------

## 9. Validation performed

The implementation was validated using:

-   `dotnet build`
-   automated unit tests
-   EF Core migrations
-   SQLite persistence across application restarts
-   Swagger/OpenAPI inspection
-   manual alert CRUD requests
-   manual event submission
-   matching and non-matching event scenarios
-   Email and Slack dispatch logging
-   notification audit inspection through the admin API
-   alert inspection through the admin API

The most important finding was that successful compilation and unit
tests were not sufficient. Runtime testing exposed both
dependency-injection configuration and SQLite-provider-specific issues.

------------------------------------------------------------------------

## 10. Final assessment

The implementation meets the intended prototype scope:

-   alerts can be configured and persisted,
-   events can be submitted,
-   events are matched against enabled alerts,
-   severity thresholds are supported,
-   matching alerts dispatch through configured notification channels,
-   Email and Slack channels share an extensible abstraction,
-   notification attempts are auditable,
-   administrative information is exposed through REST endpoints,
-   the core matching and processing behavior is covered by automated
    tests.

The implementation is intentionally not production-ready.

The main production gaps are authentication/authorization, real external
event sources, real notification providers, durable asynchronous
processing, idempotency/retry handling, pagination, and operational
monitoring.
