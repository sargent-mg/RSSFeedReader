---

description: "Executable task list for the Manage RSS Subscriptions MVP"
---

# Tasks: Manage RSS Subscriptions

**Input**: Design documents from `/specs/001-manage-rss-subscriptions/`

**Prerequisites**: [plan.md](plan.md), [spec.md](spec.md), [research.md](research.md), [data-model.md](data-model.md), [contracts/](contracts/), and [quickstart.md](quickstart.md)

**Organization**: Tasks are grouped by setup, shared foundations, and the single P1 user story so the MVP can be implemented and validated independently.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the cross-platform .NET solution and the planned backend, frontend, and test projects.

- [ ] T001 Create `global.json` at the repository root pinning the current LTS .NET SDK selected during scaffolding.
- [ ] T002 Create `RSSFeedReader.sln` at the repository root and add the backend, frontend, and test projects to it.
- [ ] T003 [P] Create `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj` as an ASP.NET Core Web API project targeting the pinned SDK.
- [ ] T004 [P] Create `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj` as a Blazor WebAssembly project targeting the pinned SDK.
- [ ] T005 [P] Create `tests/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj` with xUnit and a project reference to `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish the API/UI boundary, local configuration, and route hygiene required before the subscription story can be implemented.

**Checkpoint**: Foundation ready - the projects build, the backend and frontend launch on coordinated ports, and no template route conflicts remain.

- [ ] T006 Configure backend startup, dependency injection, JSON settings, and CORS for the frontend origins in `backend/RSSFeedReader.Api/Program.cs`.
- [ ] T007 [P] Configure backend local HTTP/HTTPS ports in `backend/RSSFeedReader.Api/Properties/launchSettings.json` using the documented backend defaults.
- [ ] T008 Configure frontend startup to load the API base URL and register the API client HTTP service in `frontend/RSSFeedReader.UI/Program.cs`.
- [ ] T009 [P] Configure the frontend API base URL and local HTTP/HTTPS ports in `frontend/RSSFeedReader.UI/wwwroot/appsettings.json` and `frontend/RSSFeedReader.UI/Properties/launchSettings.json`.
- [ ] T010 [P] Remove template demo pages and duplicate root navigation entries from `frontend/RSSFeedReader.UI/Pages/` and `frontend/RSSFeedReader.UI/Layout/NavMenu.razor`, leaving one root route for the MVP page.

---

## Phase 3: User Story 1 - Add and View Feed Subscriptions (Priority: P1) 🎯 MVP

**Goal**: Let a local user add non-empty subscription values and see the ordered values as plain text in the current session.

**Independent Test**: Start the backend and frontend with an empty list, add two non-empty values, verify immediate ordered plain-text display, submit an empty value and verify no change, add a duplicate and verify it remains as a separate entry, then restart and verify the list resets.

### Tests for User Story 1

- [ ] T011 [P] [US1] Add xUnit tests for empty-state listing, ordered additions, duplicate values, and restart-cleared in-memory state in `tests/RSSFeedReader.Api.Tests/SubscriptionBehaviorTests.cs`.
- [ ] T012 [US1] Add API contract tests for `GET /api/subscriptions` and `POST /api/subscriptions`, including `201 Created` for non-empty input and `400 Bad Request` without list mutation for empty input, in `tests/RSSFeedReader.Api.Tests/SubscriptionBehaviorTests.cs`.

### Implementation for User Story 1

- [ ] T013 [P] [US1] Create the backend `Subscription` DTO with the required `url` field in `backend/RSSFeedReader.Api/Models/Subscription.cs`.
- [ ] T014 [P] [US1] Create the frontend subscription DTO matching the API contract in `frontend/RSSFeedReader.UI/Models/Subscription.cs`.
- [ ] T015 [US1] Implement the ordered in-memory subscription store with empty/whitespace rejection and duplicate preservation in `backend/RSSFeedReader.Api/Services/InMemorySubscriptionStore.cs`.
- [ ] T016 [US1] Implement `GET /api/subscriptions` and `POST /api/subscriptions` with the documented status codes and response shapes in `backend/RSSFeedReader.Api/Controllers/SubscriptionsController.cs`.
- [ ] T017 [US1] Implement the frontend API client for listing and adding subscriptions in `frontend/RSSFeedReader.UI/Services/SubscriptionClient.cs`.
- [ ] T018 [US1] Implement the subscriptions page with URL input, add action, empty-input no-op behavior, ordered list refresh, and plain-text rendering in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor`.
- [ ] T019 [US1] Update the frontend navigation to expose the subscriptions page without adding another root route in `frontend/RSSFeedReader.UI/Layout/NavMenu.razor`.

**Checkpoint**: User Story 1 is independently functional when the API tests pass and the browser flow in `quickstart.md` succeeds.

---

## Phase 4: Polish & Cross-Cutting Concerns

**Purpose**: Verify the complete MVP against its documented quality gates and keep the design artifacts aligned with the implementation.

- [ ] T020 [P] Review `backend/RSSFeedReader.Api/Controllers/SubscriptionsController.cs` and `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` to confirm submitted values are treated as untrusted plain text and no feed URL is fetched or navigated to.
- [ ] T021 [P] Verify backend/frontend port, API base URL, and CORS alignment in `backend/RSSFeedReader.Api/Properties/launchSettings.json`, `frontend/RSSFeedReader.UI/Properties/launchSettings.json`, `frontend/RSSFeedReader.UI/wwwroot/appsettings.json`, and `backend/RSSFeedReader.Api/Program.cs`.
- [ ] T022 Run `dotnet test` from the repository root and resolve any failures covered by `tests/RSSFeedReader.Api.Tests/SubscriptionBehaviorTests.cs`.
- [ ] T023 Run `dotnet build` from the repository root and confirm the backend, frontend, and test projects compile cleanly.
- [ ] T024 Execute every browser and API scenario in `specs/001-manage-rss-subscriptions/quickstart.md` and record any implementation-specific status/body details in that guide.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies; creates the solution and project files.
- **Foundational (Phase 2)**: Depends on Phase 1; blocks User Story 1 until startup, configuration, and routing are ready.
- **User Story 1 (Phase 3)**: Depends on Phase 2; contains the complete MVP and has no dependency on another user story.
- **Polish (Phase 4)**: Depends on User Story 1 implementation; validates the complete MVP across projects.

### User Story Dependencies

- **User Story 1 (P1)**: Starts after Phase 2 and is independently deliverable. No other user stories exist for this MVP.

### Within User Story 1

- Write the API behavior and contract tests first (T011-T012).
- Create DTOs before the store and controller (T013-T016).
- Complete the API client before the page integration (T017-T018).
- Update navigation after the page exists (T019).
- Stop at the checkpoint and run the independent browser/API validation before polish.

## Parallel Opportunities

- **Setup**: T003, T004, and T005 can run in parallel after T002 establishes the solution name.
- **Foundation**: T007, T009, and T010 can run in parallel once the project directories exist; T006 and T008 can then be completed against those configuration files.
- **User Story 1 tests**: T011 and T012 can be authored in parallel because they cover separate test concerns in the same test file only if coordinated; otherwise run sequentially to avoid file conflicts.
- **User Story 1 models**: T013 and T014 can run in parallel because they are separate files.
- **Polish**: T020 and T021 can run in parallel; T022-T024 run after implementation and configuration are complete.

## Parallel Example: User Story 1

```text
After Phase 2 completes, assign these independent work items:

Task: "Add xUnit subscription behavior tests in tests/RSSFeedReader.Api.Tests/SubscriptionBehaviorTests.cs"
Task: "Create backend Subscription DTO in backend/RSSFeedReader.Api/Models/Subscription.cs"
Task: "Create frontend Subscription DTO in frontend/RSSFeedReader.UI/Models/Subscription.cs"
```

The store, controller, API client, and page then proceed in dependency order because each consumes the preceding contract or model.

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 setup.
2. Complete Phase 2 foundation and verify both projects can start.
3. Complete Phase 3 User Story 1.
4. Run the API tests and the independent browser flow in `specs/001-manage-rss-subscriptions/quickstart.md`.
5. Stop and demo the subscription add/list behavior before considering any Extended-MVP work.

### Incremental Delivery

1. Deliver the P1 subscription story as the complete MVP.
2. Keep feed fetching, parsing, item display, persistence, removal, polling, and organization deferred to a later feature specification.
3. Preserve the API/UI boundary so later work can extend the system without changing the MVP's core contract.

## Notes

- Every task uses the required checkbox, sequential ID, optional `[P]` marker, story label where applicable, and an exact file path.
- `[P]` marks work that can proceed in parallel only when its listed file is not being edited by another active task.
- No production-scale authentication, persistence, feed networking, or content rendering tasks are included because they violate the MVP scope.
