# Prompt 001 — Brief Analysis

## Context

I received an intentionally vague product brief for a backend technical exercise.

The system should allow users to configure alerts for important world events such as breaking news, market movements and natural disasters.

Notifications should initially support email and Slack, while allowing additional notification channels to be added later.

Administrative functionality is also required.

The exercise explicitly evaluates how ambiguity is handled and how AI output is critically reviewed.

I have decided to keep the implementation backend-focused. A graphical frontend or admin UI will not be implemented.

## Prompt

Analyze the provided product brief from the perspective of a senior backend developer.

Do not design the implementation yet and do not generate code.

Identify:

1. Explicit requirements stated directly in the brief.
2. Important ambiguities or missing requirements.
3. Assumptions that would need to be made before implementation.
4. Questions that would normally be clarified with the product manager.
5. Requirements that can reasonably be deferred or excluded from a backend-focused prototype.

Do not silently invent missing requirements.

Clearly distinguish between:

- requirements stated in the brief;
- reasonable assumptions;
- implementation decisions.

Keep the proposed scope appropriate for a time-limited backend technical exercise.