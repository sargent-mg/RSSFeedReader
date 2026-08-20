# Research: Manage RSS Subscriptions

**Date**: 2026-08-20
**Feature**: [Manage RSS Subscriptions](spec.md)

## Decision 1: Use a two-project API/UI boundary

- **Decision**: Build an ASP.NET Core Web API backend and a Blazor WebAssembly frontend as separate projects.
- **Rationale**: This matches the stakeholder technology decision, keeps subscription state and business behavior separate from presentation, and leaves a clean boundary for later feed operations.
- **Alternatives considered**: A frontend-only implementation would be smaller, but it would bypass the stated backend responsibility and make the planned API contract impossible to exercise.

## Decision 2: Keep subscriptions in process memory

- **Decision**: Store the ordered subscription values in a process-local collection owned by a backend service.
- **Rationale**: The MVP explicitly defers persistence and is intended as a local proof of concept. This is the smallest design that supports add/list behavior and preserves insertion order.
- **Alternatives considered**: SQLite or another database was rejected because persistence is explicitly post-MVP; browser-only storage was rejected because the project defines the backend as the owner of subscription data.

## Decision 3: Expose only list and add operations

- **Decision**: Provide `GET /api/subscriptions` and `POST /api/subscriptions`.
- **Rationale**: These operations map directly to the MVP journeys and avoid speculative feed, refresh, remove, or search APIs.
- **Alternatives considered**: A broader CRUD API was rejected because removal, persistence, and feed content are outside scope.

## Decision 4: Treat submitted values as display data

- **Decision**: Reject empty or whitespace-only submissions, accept other values without feed validation, preserve insertion order, allow duplicates, and render values as plain text.
- **Rationale**: This follows the feature specification while respecting secure-by-default handling of untrusted input. No external request or URL navigation is needed in the MVP.
- **Alternatives considered**: URL/feed validation and clickable links were rejected because they add network, security, and UX scope before feed operations are planned.

## Decision 5: Pin the SDK selected during scaffolding

- **Decision**: Select the current supported LTS .NET SDK during project setup and record it in `global.json`.
- **Rationale**: The stakeholder documents specify ASP.NET Core and Blazor but do not prescribe a version. Pinning the actual SDK used by the repository makes builds reproducible without inventing a version before scaffolding.
- **Alternatives considered**: Hardcoding an arbitrary SDK version in this plan was rejected because no SDK or existing project files are present yet.

## Decision 6: Validate with focused backend tests and a runnable quickstart

- **Decision**: Use xUnit for subscription behavior and document browser/API end-to-end checks in [quickstart.md](quickstart.md).
- **Rationale**: Backend state transitions and input rules can be automated immediately; the repository has no existing frontend test harness, so a simple runnable UI flow keeps the MVP validation proportional.
- **Alternatives considered**: Adding a component-testing framework now was rejected as unnecessary setup for a single page and a narrow proof of concept.
