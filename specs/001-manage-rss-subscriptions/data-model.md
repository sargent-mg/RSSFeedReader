# Data Model: Manage RSS Subscriptions

**Feature**: [Manage RSS Subscriptions](spec.md)

## Subscription

Represents one feed URL entered by the local user during the current application session.

| Field | Type | Required | Rules |
|---|---|---:|---|
| `url` | string | Yes | Must contain at least one non-whitespace character. The value is retained for display; feed validity is not checked. |

## Subscription List

An ordered collection of `Subscription` values owned by the current application process.

- Starts empty when the application process starts.
- Appends each accepted subscription to the end of the collection.
- Preserves insertion order when returned to the frontend.
- Allows repeated URL values as separate entries.
- Is discarded when the backend process stops or restarts.

## State Transitions

1. **Empty**: no subscriptions exist.
2. **Added**: a non-empty value is appended and becomes visible in the list.
3. **Unchanged on invalid submission**: an empty or whitespace-only value is rejected and the current list remains unchanged.

## Data Handling Boundary

The URL is untrusted display data. The MVP does not fetch it, parse it, turn it into a link, or render it as markup.
