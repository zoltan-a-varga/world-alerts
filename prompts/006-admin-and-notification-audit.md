# 006 --- Admin API and Notification Audit

**Record type:** Reconstructed interaction summary\
**Stage:** Minimal administrative visibility

> This file summarizes the actual AI-assisted implementation discussion.
> It is not presented as a verbatim historical prompt.

## Development request

Implement the smallest backend interpretation of the vague requirement:

> We need an admin view too.

No frontend should be added.

## Agreed scope

Administrative visibility is provided through REST endpoints:

``` text
GET /api/admin/alerts
GET /api/admin/notifications
```

Swagger acts as the demonstration interface.

To support notification inspection, a persisted `NotificationAttempt`
record and repository were added.

An audit record is created after a notification channel's `SendAsync`
operation completes successfully.

## Database change

Adding `NotificationAttempt` changed the EF Core model, so a new
migration was created:

``` text
AddNotificationAttempts
```

## Runtime issue found

The initial repository implementation attempted to sort notification
attempts in SQLite using:

``` csharp
.OrderByDescending(x => x.CreatedAt)
```

where `CreatedAt` is a `DateTimeOffset`.

The code compiled, but EF Core's SQLite provider threw
`NotSupportedException` at runtime because that ordering expression
could not be translated.

## Course correction

For the deliberately small prototype dataset, the records are loaded
first and then sorted with LINQ to Objects.

This is explicitly treated as an MVP trade-off rather than a
production-scale solution. A production implementation should use a
persistence representation and query strategy that supports efficient
server-side ordering and pagination.
