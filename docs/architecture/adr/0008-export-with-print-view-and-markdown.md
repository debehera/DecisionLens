# ADR-0008: Export with a print view and Markdown

| Field | Value |
| --- | --- |
| Status | Accepted |
| Date | 2026-10-20 |
| Owner | Product owner |
| ADM phase | C. Information Systems Architectures |
| Architecture domain | Application |

## Context

Architects need to share a record with stakeholders. Export is the last step of the writer-to-reader flow and the first thing reviewers will judge on screen. Real PDF or Word generation could use half a day of a very small budget.

## Decision

Support two exports and no more:

1. A **print view** with a print stylesheet. The user chooses "Save as PDF" in the browser.
2. A **Markdown download** built with a string builder.

No PDF or Word libraries are used.

## Options considered

- **Print view plus Markdown (chosen).** Pros: a polished PDF through the browser with almost no extra code; Markdown suits developer and wiki workflows. Cons: PDF layout depends on the browser.
- **Generated PDF with a library.** Pros: consistent layout. Cons: extra dependency, rendering quirks and a large time cost.
- **Word (.docx).** Pros: familiar to enterprise stakeholders. Cons: the highest build effort.

## Rationale

The chosen exports cover the sharing need at a fraction of the cost and keep the dependency list small.

## Consequences and risks

- PDF output depends on the user's browser print settings. The print stylesheet is tested on Day 6 and Day 9.
- The Markdown download is the first item on the cut list if time runs short; the print view is never cut.
- Word and generated PDF are future scope.
