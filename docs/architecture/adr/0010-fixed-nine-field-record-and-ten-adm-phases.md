# ADR-0010: Fixed nine-field record and all ten ADM phases

| Field | Value |
| --- | --- |
| Status | Accepted |
| Date | 2026-10-20 |
| Owner | Product owner |
| ADM phase | C. Information Systems Architectures |
| Architecture domain | Data |

## Context

The record shape drives the AI prompt, the database, the screens and the exports. Changing it later costs far more than choosing it now. The product's differentiator is the TOGAF layer, so the phase list must be complete and accurate.

## Decision

The decision record has **nine fields**, fixed for v1.0:

1. Title
2. Decision
3. Context
4. Options considered
5. Rationale
6. Consequences and risks
7. ADM phase
8. Architecture domain
9. Owner, status and date

Fields 1 to 6 are drafted by the AI and edited by the architect. Fields 7 and 8 are suggested by the AI and confirmed by the architect. Field 9 is entered only by the architect. The **ADM phase** allows all ten values: Preliminary, A (Architecture Vision), B (Business Architecture), C (Information Systems Architectures), D (Technology Architecture), E (Opportunities and Solutions), F (Migration Planning), G (Implementation Governance), H (Architecture Change Management), and Requirements Management. The **architecture domain** is one of Business, Data, Application or Technology; this separate field keeps the distinction between data and application work inside Phase C. **Status** is Proposed, Accepted, Rejected or Superseded.

## Options considered

- **Nine fields and all ten phases (chosen).** Pros: complete and recognisable to TOGAF practitioners; the phase list is only a dropdown, so it costs little. Cons: more choices for the AI to get wrong, which the human review catches.
- **Only the phases where most design decisions happen (B, C, D, G, H).** Pros: a narrower task for the AI. Cons: incomplete against the framework; weaker story for a TOGAF capstone.
- **A richer record (links, severity, impact).** Pros: more detail. Cons: more AI guesswork, more screens and more testing than the time allows.

## Rationale

A lean, fixed record that follows well-known decision record practice, plus the full ADM list, gives the product its TOGAF identity without extra build cost.

## Consequences and risks

- The prompt must define each phase and domain so the AI chooses reliably.
- Any new field or removed field needs a new ADR, because it changes the prompt, database, forms and exports together.
- Status accepts Rejected and Superseded, but records cannot yet be linked to the decision that supersedes them. Linking is future scope.
