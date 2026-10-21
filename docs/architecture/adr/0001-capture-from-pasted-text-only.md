# ADR-0001: Capture decisions from pasted text only

| Field | Value |
| --- | --- |
| Status | Accepted |
| Date | 2026-10-20 |
| Owner | Product owner |
| ADM phase | B. Business Architecture |
| Architecture domain | Business |

## Context

Architecture discussions happen in a mix of chat tools, meetings and calls, email threads, and ticket or pull request comments. Whatever the source, the content can be reduced to text. Direct integrations with Teams, Slack, Jira or email would each take a large share of a 13-hour build budget.

## Decision

DecisionLens v1.0 accepts **pasted text only**. There is no file upload and there are no integrations. A built-in sample loader supports demos.

## Options considered

- **Paste text only.** Pros: works for every source on day one; almost no build effort; easy to test. Cons: one manual step for the user.
- **File upload (transcripts, exports).** Pros: fewer manual steps. Cons: needs file handling, size limits and parsing for each format.
- **Direct integrations (Teams, Slack, Jira, email).** Pros: removes the paste step. Cons: authentication, permissions and review for each tool; not possible in the time available.

## Rationale

Pasting covers every source with the least effort and keeps the product focused on its real value, which is extraction and classification. It also makes the product safe to try, because nothing connects to real company systems.

## Consequences and risks

- No integrations means one extra step for the architect. This is accepted.
- Long threads must be trimmed to the relevant part because of the 6,000-character input cap.
- Integrations and file upload are recorded as future scope. Adding either needs a new ADR.
