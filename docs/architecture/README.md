# DecisionLens: Architecture documentation

TOGAF-aligned architecture documents for DecisionLens v1.0, written on Day 1 (Tue Oct 20, 2026) from the locked product decisions. The product requirements and the build plan are in the two documents one folder up (`docs/`).

## Documents

| File | TOGAF phase | What it answers |
| --- | --- | --- |
| [01-architecture-vision-and-context.md](01-architecture-vision-and-context.md) | Preliminary, A | Why the product exists; business context and strategy; stakeholders; principles; constraints; scope; ADM mapping |
| [02-business-capability-map.md](02-business-capability-map.md) | B | Which business capabilities exist and which v1.0 delivers |
| [03-gap-analysis.md](03-gap-analysis.md) | B to D, input to E and F | Baseline against target, what remains open, and the roadmap |
| [capability-map.png](capability-map.png) | B | Picture of the capability map, for slides and documents |
| [adr/](adr/) | C, D, G | Ten Architecture Decision Records for the locked decisions |

## Architecture Decision Records

Each ADR uses the same nine fields as the DecisionLens record, so the project documents its own decisions the way the product asks users to.

| ADR | Title | ADM phase | Domain | Status |
| --- | --- | --- | --- | --- |
| [0001](adr/0001-capture-from-pasted-text-only.md) | Capture decisions from pasted text only | B. Business Architecture | Business | Accepted |
| [0002](adr/0002-ai-proposes-human-confirms-togaf-tags.md) | AI proposes and the architect confirms the TOGAF tags | G. Implementation Governance | Business | Accepted |
| [0003](adr/0003-server-rendered-razor-pages-with-plain-javascript.md) | Server-rendered Razor Pages with plain JavaScript | C. Information Systems Architectures | Application | Accepted |
| [0004](adr/0004-postgresql-with-entity-framework-core.md) | PostgreSQL with Entity Framework Core | C. Information Systems Architectures | Data | Accepted |
| [0005](adr/0005-gemini-free-tier-behind-an-interface.md) | Gemini free tier behind a replaceable interface | C. Information Systems Architectures | Application | Accepted |
| [0006](adr/0006-no-login-in-v1-with-guardrails.md) | No login in v1.0, with usage guardrails | C. Information Systems Architectures | Application | Accepted |
| [0007](adr/0007-do-not-store-the-pasted-discussion.md) | Do not store the pasted discussion | C. Information Systems Architectures | Data | Accepted |
| [0008](adr/0008-export-with-print-view-and-markdown.md) | Export with a print view and Markdown | C. Information Systems Architectures | Application | Accepted |
| [0009](adr/0009-deploy-on-render-free-tier-with-docker.md) | Deploy on Render's free tier with Docker | D. Technology Architecture | Technology | Accepted |
| [0010](adr/0010-fixed-nine-field-record-and-ten-adm-phases.md) | Fixed nine-field record and all ten ADM phases | C. Information Systems Architectures | Data | Accepted |

The phase and domain tags on these ADRs are the author's classification. As with every DecisionLens record, a reviewer may correct them.

## Maintenance

- A decision is never edited away. To change one, add a new ADR and set the old one to Superseded.
- Update the index above whenever an ADR is added.
- Day 2 of the build plan places this folder at `docs/architecture/` in the repository.
