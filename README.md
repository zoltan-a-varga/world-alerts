# World Alerts

Backend-focused implementation of the **Feature Design & Build from a Vague Brief** technical exercise.

## Purpose

The goal of this exercise is to take an intentionally vague product brief and turn it into a working backend implementation using AI-assisted development.

The repository documents not only the resulting code, but also the process used to arrive at it: assumptions, design decisions, AI prompts, validation steps, rejected suggestions, and course corrections.

## Scope

The implementation focuses on the backend of the alerting system.

The planned core workflow is:

`Event -> Alert matching -> Notification`

Users will be able to configure alerts for events and receive notifications through supported channels such as email and Slack.

Administrative functionality will be exposed through backend API endpoints. A graphical frontend or admin UI is outside the scope of this implementation.

## Documentation

Development artifacts are kept in the [`docs`](docs/) directory.

- [`01-plan.md`](docs/01-plan.md) — initial implementation plan

AI prompts used during the exercise will be recorded in the [`prompts`](prompts/) directory.

Additional documentation will be added as the implementation progresses.

## Status

🚧 **Work in progress**

The repository is being developed incrementally. Requirements, technical decisions, setup instructions and implementation details will be added as they are established and validated.