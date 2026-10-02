# Implementation Plan

## 1. Goal

Build a small but working backend prototype from the provided product brief while documenting how the initial ambiguity is resolved and how AI-generated suggestions and code are evaluated.

The implementation will focus on the backend. The goal is not to build a production-ready alerting platform, but to demonstrate the core workflow end-to-end with a design that can reasonably be extended later.

## 2. Initial interpretation

The brief describes a system where:

- users can configure alerts for important events;
- events may represent news, market movements, natural disasters, or similar occurrences;
- matching events trigger notifications;
- email and Slack must be supported;
- additional notification channels should be possible later;
- administrative functionality is required.

Several important details are intentionally unspecified, including:

- what qualifies as an "important" event;
- where events originate;
- how alert conditions are represented;
- how users are identified;
- what the admin functionality should contain;
- whether notification delivery needs production-level reliability guarantees.

I will make these decisions explicitly rather than silently inventing requirements.

## 3. Scope

### In scope

- ASP.NET Core REST API
- creating and managing alerts
- configurable alert conditions
- receiving or simulating events
- evaluating events against alerts
- email notification channel
- Slack notification channel
- an abstraction that allows additional notification channels
- persistence
- validation and error handling
- backend administrative endpoints
- automated tests
- OpenAPI/Swagger documentation

### Out of scope

- graphical frontend
- graphical admin application
- production authentication/identity integration
- integration with real news, financial-market, or disaster data providers
- production-scale distributed infrastructure
- deployment infrastructure

The "admin view" requirement will therefore be represented by administrative API endpoints that can be exercised through the API documentation.

## 4. Approach

I will work in the following stages.

### Stage 1 — Clarify the problem

Extract explicit requirements from the brief and identify ambiguities.

For each important ambiguity, either:

- make and document an assumption;
- defer the decision behind an abstraction; or
- explicitly mark it as out of scope.

**Output:** requirements and assumptions.

### Stage 2 — Design the backend

Define:

- core domain concepts;
- alert conditions;
- event representation;
- notification model;
- REST API;
- persistence approach;
- extension points.

Keep the design proportional to the size of the exercise and avoid adding infrastructure without a concrete requirement.

**Output:** lightweight design/architecture document and decision log.

### Stage 3 — Build a minimal vertical slice

Implement the simplest end-to-end workflow first:

`Event -> Alert matching -> Notification`

Once this works, add the API operations required to manage alerts and inspect the system.

**Output:** working backend application.

### Stage 4 — Add notification channels

Implement email and Slack behind a common notification abstraction.

Verify that adding another notification channel would not require changes to the alert matching logic.

### Stage 5 — Test and validate

Add automated tests around the core behaviour, including:

- matching event;
- non-matching event;
- disabled alert;
- multiple notification channels;
- invalid input;
- notification failures where relevant.

Run the application and exercise the important API scenarios manually as well.

### Stage 6 — Review AI-generated work

Review generated code and design suggestions rather than accepting them automatically.

Checks will include:

- correctness;
- unnecessary complexity;
- error handling;
- input validation;
- asynchronous code and cancellation;
- persistence behaviour;
- duplicate notification risks;
- configuration and secret handling;
- test quality;
- consistency with the original scope.

Significant rejected or modified AI suggestions will be recorded in the decision log or review notes.

### Stage 7 — Final review

Verify that:

- the repository builds from a clean checkout;
- automated tests pass;
- the main scenario can be demonstrated;
- setup and execution instructions are documented;
- assumptions and limitations are visible;
- prompt history is included;
- major decisions and AI-assisted course corrections are documented.

## 5. Planned repository artifacts

```text
docs/
    01-plan.md
    02-requirements-and-assumptions.md
    03-design.md
    04-decision-log.md
    05-validation-and-review.md

prompts/
    ...

src/
    ...

tests/
    ...

README.md
```

The exact structure may change during implementation. Significant changes to the plan will be documented rather than silently rewritten.

## 6. Definition of Done

The exercise is complete when:

1. Alerts can be created and managed through the API.
2. An event can be submitted or simulated.
3. The system evaluates the event against configured alerts.
4. Matching alerts trigger the configured notification channels.
5. Email and Slack channels are represented by working or safely simulated implementations.
6. Administrative information is available through backend endpoints.
7. Core behaviour is covered by automated tests.
8. The application can be run using documented instructions.
9. Important assumptions, decisions, AI prompts, validation steps and known limitations are included in the repository.