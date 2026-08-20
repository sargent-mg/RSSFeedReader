# Quickstart: Manage RSS Subscriptions

This guide validates the MVP without feed retrieval or persistence.

## Prerequisites

- .NET SDK version recorded in the repository's `global.json`.
- A browser with JavaScript enabled.
- The backend and frontend projects created at the paths described in [plan.md](plan.md).

## Start the backend

```sh
dotnet run --project backend/RSSFeedReader.Api
```

Verify that the API is available at the configured backend URL, expected by the MVP to default to `http://localhost:5151`.

## Start the frontend

In a second terminal:

```sh
dotnet run --project frontend/RSSFeedReader.UI
```

Open the configured frontend URL, expected by the MVP to default to `http://localhost:5213`.

## Browser validation

1. Confirm the subscriptions page loads with an empty list.
2. Enter `https://example.com/feed.xml` and submit it.
3. Confirm the value appears as plain text within 1 second without a full-page reload.
4. Add `https://example.org/atom.xml` and confirm both entries remain visible in insertion order.
5. Submit an empty or whitespace-only value and confirm the list is unchanged.
6. Add the first URL again and confirm duplicate values remain as separate ordered entries.
7. Restart the backend and reload the frontend; confirm the list starts empty because persistence is out of scope.

## API validation

```sh
curl -i http://localhost:5151/api/subscriptions
curl -i -X POST http://localhost:5151/api/subscriptions \
  -H 'Content-Type: application/json' \
  -d '{"url":"https://example.com/feed.xml"}'
curl -i -X POST http://localhost:5151/api/subscriptions \
  -H 'Content-Type: application/json' \
  -d '{"url":"   "}'
```

Expected outcomes:

- `GET` returns the current ordered list.
- A non-empty `POST` adds and returns the subscription.
- An empty or whitespace-only `POST` does not change the list and returns a client-error response documented by the implementation.
- No request is made to the submitted feed URL.

## Quality gates

Before the feature is considered complete:

```sh
dotnet test
dotnet build
```

Also verify that only one frontend page owns the root route, the frontend API base URL matches the backend port, and backend CORS allows the frontend origin.
