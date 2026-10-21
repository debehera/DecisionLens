# ADR-0007: Do not store the pasted discussion

| Field | Value |
| --- | --- |
| Status | Accepted |
| Date | 2026-10-20 |
| Owner | Product owner |
| ADM phase | C. Information Systems Architectures |
| Architecture domain | Data |

## Context

The pasted text is the raw material for a record, and in real use it could contain sensitive information. The record itself, after human review, is the governance artefact. Storing the raw text would add risk and storage with little benefit for v1.0.

## Decision

The pasted discussion is used only to produce the draft. It is **not saved** in the database, and it is **not written to logs**. Only the reviewed record is stored. Logs keep only the result code and the provider's HTTP status.

## Options considered

- **Do not store the source text (chosen).** Pros: smaller data footprint; lower privacy risk; simpler schema. Cons: the original discussion cannot be shown next to the record later.
- **Store the source text with the record.** Pros: traceability back to the original. Cons: stores potentially sensitive content; larger risk if the shared list is open.
- **Store a short excerpt.** Pros: some traceability. Cons: still stores content; adds a field to a record shape that is locked.

## Rationale

The record is the unit of value, and the reviewed record already holds the context, options and rationale. Not keeping the source text follows the principle of storing only what is needed.

## Consequences and risks

- A user cannot re-draft from a saved record, because the source is gone.
- The text is still sent to the AI provider during drafting, so the banner and README ask for fictional or sanitized text.
- Keeping source text for traceability is future scope and needs a new ADR with a data-protection review.
