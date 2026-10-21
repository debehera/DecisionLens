# ADR-0002: AI proposes and the architect confirms the TOGAF tags

| Field | Value |
| --- | --- |
| Status | Accepted |
| Date | 2026-10-20 |
| Owner | Product owner |
| ADM phase | G. Implementation Governance |
| Architecture domain | Business |

## Context

The differentiator of DecisionLens is that every record carries a TOGAF ADM phase and an architecture domain. An AI can suggest these tags, but it can be wrong. A record used for governance must be accountable to a person.

## Decision

The AI **suggests** the ADM phase and the architecture domain. The architect **must confirm or correct** both on the review form before a record can be saved. The AI never fills owner, status or date. The review screen shows a short AI note with a confidence level and one sentence of reasoning; this note is not stored.

## Options considered

- **AI suggests, human confirms (chosen).** Pros: fast and accountable; the human step catches wrong tags. Cons: one more step.
- **AI tags and saves automatically.** Pros: fastest. Cons: wrong tags enter the record unseen; weak for governance.
- **Human tags only, no AI suggestion.** Pros: simple and fully accountable. Cons: removes the main time saving and the product's differentiator.

## Rationale

A black-box summary would not be trusted in governance. Human confirmation keeps the record accountable, while the AI suggestion keeps the effort low.

## Consequences and risks

- Saving is blocked until both tags are set.
- The AI can still suggest wrong tags; the confidence note and the human review are the controls.
- The prompt must define each phase and domain clearly. This is tested on Day 3 and Day 9 with the sample discussions.
