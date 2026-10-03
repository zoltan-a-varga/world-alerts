# 004 --- Persistence and Alert API

**Record type:** Reconstructed interaction summary\
**Stage:** Persistence and CRUD implementation

> This file summarizes the actual AI-assisted implementation discussion.
> It is not presented as a verbatim historical prompt.

## Development request

Add persistence and a minimal REST API for alert configuration.

Prefer the smallest solution that can be run locally without external
infrastructure.

## Agreed direction

-   Entity Framework Core 8.
-   SQLite.
-   Repository abstraction defined in Core and implemented in
    Infrastructure.
-   Alert CRUD exposed from the API.
-   Cancellation tokens propagated through asynchronous operations.
-   Swagger/OpenAPI used for manual API exploration.
-   EF Core migrations committed to the repository.

## Persistence trade-off

Notification channel selections were stored using an EF Core value
converter as a comma-separated representation.

A normalized child table would be cleaner for a larger relational model,
but the converter was accepted for this prototype because:

-   only two fixed channel values were required,
-   channel values are loaded with the alert,
-   no channel-specific database queries were required,
-   it reduced implementation overhead.

## Runtime validation

Persistence was checked by creating an alert, reading it through the
API, restarting the application, and confirming that the alert remained
available.

## Course correction

During Swagger validation, the UI reported that the generated API
definition did not contain a valid version field.

The generated JSON was inspected directly and did contain
`openapi: 3.0.4`. This ruled out the initial configuration hypothesis.
Package inspection showed an older Swashbuckle version, and upgrading
Swashbuckle resolved the rendering problem.

This was retained as an example of validating an AI-generated diagnosis
instead of repeatedly changing valid configuration.
