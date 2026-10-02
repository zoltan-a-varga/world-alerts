# Decision Log

## D-001 — Backend-only implementation

**Decision:** Implement administrative functionality through REST API endpoints rather than a graphical frontend.

**Reason:** The exercise is being approached as a backend engineering task. Introducing an unfamiliar frontend stack would add implementation and validation risk without improving the demonstration of backend behaviour.

---

## D-002 — No real event provider

**Decision:** Accept normalized events through the API instead of integrating a news, market or disaster provider.

**Reason:** The brief does not specify a provider. Choosing one arbitrarily would introduce credentials, provider-specific behaviour and external dependencies unrelated to the core alert workflow.

---

## D-003 — Simple alert conditions

**Decision:** Initially support category and minimum severity rather than building a generic rules engine.

**Reason:** A generic expression/rules system would add substantial complexity without a concrete requirement.

This is deliberately designed as a replaceable limitation of the prototype.

---

## D-004 — Notification abstraction

**Decision:** Email and Slack implementations use a shared notification-channel interface.

**Reason:** Supporting additional channels later is an explicit requirement in the brief.

---

## D-005 — SQLite persistence

**Decision:** Use SQLite with Entity Framework Core.

**Reason:** It provides real relational persistence while keeping the project self-contained and easy for a reviewer to run.

PostgreSQL or SQL Server would add setup cost without materially improving the prototype.

---

## D-006 — Lightweight project structure

**Decision:** Use API, Core and Infrastructure projects rather than a larger Clean Architecture project structure.

**Reason:** Separating domain/business concerns from infrastructure is useful, but introducing additional Application, Domain, Contracts and multiple test projects would be disproportionate for the size of this exercise.

---

## D-007 — No production delivery guarantees

**Decision:** Do not implement queues, distributed processing or exactly-once delivery.

**Reason:** These requirements are not specified and would substantially increase the scope.

Duplicate delivery and failure behaviour will still be considered during validation.