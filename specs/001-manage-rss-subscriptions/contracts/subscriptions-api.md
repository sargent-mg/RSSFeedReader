# Subscriptions API Contract

**Feature**: [Manage RSS Subscriptions](../spec.md)
**Base path**: `/api/subscriptions`

The API owns the current in-memory ordered subscription list. It does not fetch or validate feed content.

## List subscriptions

`GET /api/subscriptions`

### Success: `200 OK`

```json
[
  { "url": "https://example.com/feed.xml" },
  { "url": "https://example.org/atom.xml" }
]
```

The response order is the order in which subscriptions were accepted. Duplicate URL values are returned as separate entries.

## Add subscription

`POST /api/subscriptions`

### Request

```json
{ "url": "https://example.com/feed.xml" }
```

### Success: `201 Created`

```json
{ "url": "https://example.com/feed.xml" }
```

The new entry is appended to the list. The response contains the value as display data; clients must render it as plain text rather than as a link.

### Empty input: `400 Bad Request`

An absent, empty, or whitespace-only `url` does not create a subscription and leaves the list unchanged. The exact error body may use the framework's standard problem-details shape.

## Out of scope

This contract intentionally defines no feed retrieval, parsing, item, removal, persistence, authentication, or background-refresh operations.
