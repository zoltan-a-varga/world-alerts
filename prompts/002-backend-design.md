# Prompt 002 — Backend Design

Design a small backend-only solution for the alerting system described in the original brief and the documented requirements and assumptions.

Constraints:

- C# / .NET 8
- ASP.NET Core Web API
- backend only
- no authentication implementation
- no graphical admin UI
- no real external event provider
- email and Slack notification channels
- notification channels must be extensible
- persistence is required
- implementation should remain small enough for a time-limited technical exercise
- avoid unnecessary enterprise patterns and infrastructure

Propose:

1. Core domain model.
2. Alert condition representation.
3. Event-to-alert matching workflow.
4. Notification abstraction.
5. REST API endpoints.
6. Persistence approach.
7. Project structure.
8. Minimal administrative API.
9. Testing strategy.
10. Important limitations or risks.

Prefer simple, testable solutions over production-scale infrastructure.

Clearly identify trade-offs and do not introduce patterns unless they solve a concrete problem.