# AI Prompt History

This directory documents how AI assistance was used during the World
Alerts technical exercise.

## Important note on provenance

`001-brief-analysis.md` and `002-backend-design.md` were formal prompts
recorded during the corresponding stages of the exercise.

The implementation phase then continued as an iterative chat-based
development session. Files `003` through `007` are **reconstructed
interaction summaries**, not verbatim historical prompts. They were
created at the end of the exercise to make the development workflow
easier to review without falsely presenting reconstructed text as an
exact transcript.

The important implementation decisions, validation results, rejected
suggestions, runtime failures, and course corrections are documented
separately in
[`../docs/05-validation-and-review.md`](../docs/05-validation-and-review.md).

## Prompt / interaction index

  ---------------------------------------------------------------------------------------------
  File                                          Stage                   Record type
  --------------------------------------------- ----------------------- -----------------------
  `001-brief-analysis.md`                       Brief analysis          Original recorded
                                                                        prompt

  `002-backend-design.md`                       Backend design          Original recorded
                                                                        prompt

  `003-core-domain-and-matching.md`             Domain model and        Reconstructed
                                                matching                interaction summary

  `004-persistence-and-alert-api.md`            SQLite persistence and  Reconstructed
                                                CRUD API                interaction summary

  `005-event-processing-and-notifications.md`   Event processing and    Reconstructed
                                                notification channels   interaction summary

  `006-admin-and-notification-audit.md`         Admin API and audit     Reconstructed
                                                trail                   interaction summary

  `007-final-review.md`                         Validation and final    Reconstructed
                                                review                  interaction summary
  ---------------------------------------------------------------------------------------------

## How AI was used

AI was used to help:

-   decompose the vague brief,
-   identify assumptions and scope boundaries,
-   propose a small backend architecture,
-   generate implementation candidates,
-   suggest tests,
-   investigate build and runtime failures,
-   review trade-offs,
-   organize final documentation.

Generated output was not treated as authoritative. Suggestions were
compiled, tested, exercised against SQLite and Swagger, and modified or
rejected when runtime behavior or design review showed a problem.

This distinction is important to the exercise: the repository records
both useful AI contributions and cases where human validation changed
the proposed solution.
