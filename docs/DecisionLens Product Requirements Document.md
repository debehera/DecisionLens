# DecisionLens: Product Requirements Document

Oct 20, 2026 · @Debraj

DecisionLens turns pasted, messy architecture discussions into TOGAF-tagged decision records that an architect reviews, saves, filters and exports. This PRD defines the v1.0 to be built and deployed by Thursday, October 29, 2026.

## Product overview

DecisionLens is a free-to-run web app where an architect pastes a messy discussion, gets an AI-drafted decision record tagged with a TOGAF ADM phase and architecture domain, corrects it, and saves it. Saved records can be browsed, filtered, edited and exported.

| Item | Value |
| --- | --- |
| Product name | DecisionLens |
| Primary user | Enterprise or solution architect (the writer) |
| Secondary users | Tech leads, project managers, product owners and governance reviewers (the readers) |
| Core flow | Paste discussion, AI drafts record, architect reviews and edits, save |
| Differentiator | AI-suggested, human-confirmed TOGAF ADM phase and architecture domain on every record |
| Build window | Tue Oct 20 to Thu Oct 29, 2026 (10 days); about 13 build hours after Day 1 |
| Access model | No login; one shared list of records; fictional or sanitized data only |
| Budget | Zero: free AI key, free hosting, no paid tools |

## Problem

Architecture decisions are made in chat threads, meetings and email, and are rarely written down in a usable form. Months later, nobody remembers why the system was built the way it was.

The root cause is extraction, not a missing template. A long thread holds the options considered, the arguments, the final call and the owner, all buried in conversation. Pulling the decision out takes time that architects and tech leads do not have.

What discovery showed:

- Decisions are discussed but not written down properly.
- The main reason is that discussions are messy, so it is hard to extract what was actually decided.
- Discussions happen across a mix of chat tools, meetings and calls, email threads, and ticket or pull request comments.
- The people who need the record are governance-minded: enterprise and solution architects.

Consequences: governance reviews lack traceability, settled debates get reopened, and new team members cannot tell why a choice was made.

## Target users

One main writer and several readers. v1.0 is designed for the writer first.

| Persona | Who | What they need | Priority |
| --- | --- | --- | --- |
| Writer | Enterprise or solution architect capturing decisions for governance | Speed, accurate extraction from messy text, TOGAF tags that are easy to confirm or correct | Primary |
| Readers | Tech leads, project managers, product owners, governance reviewers | A clear, structured record they can find by ADM phase or domain and share without asking around | Secondary |
| Evaluator | A capstone reviewer or stranger opening the public link | A built-in sample, on-screen instructions and friendly error messages so no explanation is needed | Required for Day 10 |

## Goals and success

Success on Day 10 (Thursday, October 29) is a live public link where a stranger can complete the whole workflow without help.

Product goals:

1. Cut the effort of turning a messy discussion into a structured decision record from a manual rewrite to a paste, a review and a save.
2. Make every record governance-grade: TOGAF ADM phase and architecture domain attached, confirmed by a human before saving.
3. Ship a polished, small v1.0 rather than a large, half-working one.

Day 10 success criteria:

- [ ] A public Render URL loads and shows clear instructions.
- [ ] A built-in sample discussion can be loaded with one click.
- [ ] The AI drafts all nine fields; the architect can edit every field and the two TOGAF tags before saving.
- [ ] Saved records appear in a list that filters by ADM phase and architecture domain.
- [ ] A record can be opened, edited, deleted, printed or saved as PDF, and downloaded as Markdown.
- [ ] Guardrails work: input length cap, daily AI limit, and friendly messages when a limit or AI error occurs.
- [ ] The AI never invents facts: anything not in the discussion is marked "Not stated in the discussion".
- [ ] README, demo script and the pitch deck are finished.

Quality target: on five prepared sample discussions, the owner judges the drafted decision, rationale and TOGAF tags acceptable (needing at most light edits) on at least four of the five.

## Scope

v1.0 delivers one workflow done well: capture, review, save, browse, share. Anything not listed under "In scope" is out.

**In scope for v1.0**

- Paste text from any source (chat, meeting notes, email, ticket comments); no integrations needed.
- AI-drafted nine-field decision record with TOGAF ADM phase and architecture domain suggestions.
- Human review and editing of every field before saving, plus a manual "start blank" path.
- Saved record list, single-record page, filter by ADM phase and architecture domain, edit and delete.
- Export: print-friendly page (Save as PDF from the browser) and Markdown download.
- Built-in sample discussions and a few seeded fictional records so the public link is self-explanatory.
- Guardrails: input length cap, daily AI limit, friendly error messages.
- Public deployment on a free host by Day 10.

**Intentionally left out (future scope)**

- Keyword search across records.
- Summary dashboard (for example, decisions per domain).
- Linking related decisions or marking one as superseding another.
- User accounts, login, roles or per-user records (v1.0 uses one shared list).
- Direct integrations with Teams, Slack, email or Jira.
- Native Word or generated PDF export (the browser's Save as PDF covers PDF).
- File upload, audio transcription, version history, comments and notifications.
- Storing the original pasted discussion (only the saved record is kept).

## The decision record

The record has nine fields. The product owner approved this shape on Day 1; changing it is out of scope.

| # | Field | Filled by | Rules |
| --- | --- | --- | --- |
| 1 | Title | AI draft, architect edits | Short, specific; up to 150 characters; required |
| 2 | Decision | AI draft, architect edits | What was decided, in one to three sentences; required |
| 3 | Context | AI draft, architect edits | Why the decision came up; the problem and constraints |
| 4 | Options considered | AI draft, architect edits | One option per line: name, pros, cons |
| 5 | Rationale | AI draft, architect edits | Why the chosen option won |
| 6 | Consequences and risks | AI draft, architect edits | Trade-offs accepted and risks created |
| 7 | ADM phase | AI suggests, architect confirms | Exactly one of the ten ADM values below; required |
| 8 | Architecture domain | AI suggests, architect confirms | Business, Data, Application or Technology; required |
| 9 | Owner, status, date | Architect only | Owner is free text; status is Proposed, Accepted, Rejected or Superseded; date defaults to today; status and date required |

**Allowed ADM phase values (all ten)**

| Value | Meaning |
| --- | --- |
| Preliminary | Preparing the architecture capability |
| A - Architecture Vision | Scope, vision and stakeholders |
| B - Business Architecture | Business capabilities and processes |
| C - Information Systems Architectures | Data and application architecture |
| D - Technology Architecture | Infrastructure and platforms |
| E - Opportunities and Solutions | Candidate solutions and work packages |
| F - Migration Planning | Sequencing and transition |
| G - Implementation Governance | Oversight of delivery against the architecture |
| H - Architecture Change Management | Managing change to the architecture |
| Requirements Management | Requirements across all phases |

Phase C covers both data and application work, so the separate Architecture domain field keeps that distinction.

**Governance principle:** the AI only suggests the two TOGAF tags. A human confirms or corrects them before a record can be saved.

**Review aid (not stored):** next to the draft, the app shows a short AI note with a confidence level (low, medium, high) and one sentence explaining the suggested phase and domain. It helps the architect review and is not part of the saved record.

## Functional requirements

All requirements are must-haves for v1.0 and testable on the live site.

| ID | Area | Requirement |
| --- | --- | --- |
| FR-1 | Home | Show what the app does, three-step instructions, and a warning to paste only fictional or sanitized text |
| FR-2 | Capture | Provide a large text area with a live character counter and a maximum input length (default 6,000 characters) enforced in the browser and on the server |
| FR-3 | Capture | Offer a "Load sample" control with at least three built-in fictional discussions |
| FR-4 | Capture | "Draft record" calls the AI in the background and shows a loading indicator without a full page reload |
| FR-5 | AI | Validate the AI reply on the server: every field present, ADM phase and domain from the allowed lists; otherwise retry once, then show a friendly error |
| FR-6 | AI | Never invent facts: anything missing from the discussion is written as "Not stated in the discussion" |
| FR-7 | Review | Pre-fill a review form with the draft; every field is editable; ADM phase and domain are dropdowns; owner, status and date are architect-entered with sensible defaults |
| FR-8 | Review | Show the transient AI note (confidence and one-line reason for the two TOGAF tags) beside the form |
| FR-9 | Review | A "Start blank" path lets the architect create a record manually without AI |
| FR-10 | Save | Validate required fields (title, decision, ADM phase, domain, status, date) before saving; show field-level messages |
| FR-11 | List | Show all records, newest first, with title, ADM phase, domain, status and date |
| FR-12 | List | Filter by ADM phase and by domain (separately or together) with a clear-filters control and a result count |
| FR-13 | Detail | Show all nine fields on a single record page, including TOGAF tags as visible badges |
| FR-14 | Edit | Edit any field of a saved record and save the change |
| FR-15 | Delete | Delete a record after an explicit confirmation |
| FR-16 | Export | Print view with a clean layout and a button to print or save as PDF |
| FR-17 | Export | Download a record as a Markdown file named from its title |
| FR-18 | Guardrail | Enforce a global daily AI draft limit (default 40, configurable); when reached, show a friendly message and offer the manual path |
| FR-19 | Errors | Show friendly messages for AI timeout, rate limit, invalid reply and network failure; never show stack traces |
| FR-20 | Seed data | On first run with an empty database, add four fictional sample records so the list is not empty |
| FR-21 | Empty states | Friendly empty-state and not-found pages |
| FR-22 | Ops | Provide a health endpoint for the host and log AI failures without logging the pasted text |

## Non-functional requirements and guardrails

| Area | Requirement |
| --- | --- |
| Performance | An AI draft returns in under 30 seconds in typical use; pages load in under 2 seconds once the host is awake |
| Availability | Free hosting sleeps after inactivity; the first visit can take about a minute, so the demo plan includes a warm-up step |
| Cost | Zero: free AI key, free host, free database, no paid tools |
| Security | No secrets in the repository; the AI key lives only in environment variables or user secrets; anti-forgery tokens on all form posts; all output HTML-encoded |
| Privacy | The pasted text is sent to the AI provider and is not stored by DecisionLens; only the reviewed, saved record is stored; a visible banner says to use fictional or sanitized data, because free AI tiers may use inputs to improve their products |
| Abuse protection | With no login, protect the free AI quota using the input length cap, the global daily draft limit, and a short cooldown between drafts per visitor |
| Reliability | The AI provider sits behind an interface so it can be swapped; AI failures never block manual record creation |
| Accessibility | Semantic HTML, labelled form controls, visible keyboard focus, readable contrast |
| Responsiveness | Usable at phone width (375 px) and on desktop |
| Compatibility | Current Chrome, Edge, Firefox and Safari |
| Maintainability | Configuration (AI model, limits) in settings, not hard-coded; one README with setup and deploy steps |

## Constraints, assumptions and dependencies

| Type | Item |
| --- | --- |
| Time | 13 build hours across Days 2-10: 1 hour on weekdays (Wed Oct 21 to Fri Oct 23, Mon Oct 26 to Thu Oct 29) and 3 hours each on Sat Oct 24 and Sun Oct 25 |
| Skills | Strong in C# backend, SQL, HTML and CSS, calling APIs and deployment; new to JavaScript, so it stays plain and minimal |
| Budget | No paid tools or services |
| Environment | Windows with Visual Studio or VS Code and the .NET SDK installed; a GitHub account |
| Hosting | Render, previously used by the product owner |
| AI | A free-tier model key; provider chosen in the blueprint; free-tier limits and terms can change and are re-checked on the day the key is created |
| Data | Fictional or sanitized sample data only |
| Assumption | One shared list is acceptable because no real data is entered |
| Dependency | The free AI provider, the free host and a free PostgreSQL database stay available through Day 10 |

## Risks and mitigations

| Risk | Likelihood | Impact | Mitigation |
| --- | --- | --- | --- |
| Free AI tier limits, terms or models change | Medium | High | Provider behind an interface; verify limits on the day the key is created; a second OpenAI-compatible provider as fallback; manual "Start blank" path always works |
| AI suggests the wrong ADM phase or domain | Medium | Medium | Human confirmation is mandatory; AI note shows confidence; prompt gives definitions and examples for each phase |
| AI invents details not in the discussion | Medium | High | Prompt forbids invention and requires "Not stated in the discussion"; test with sample discussions that omit information |
| Only 13 build hours | High | High | One workflow only; weekday sessions are single-purpose; the blueprint carries design decisions so no day is spent re-planning; a fixed cut list below |
| JavaScript is new | Medium | Medium | Plain JavaScript limited to three small features: fetch the draft, show a spinner, copy and download helpers |
| Free host sleeps; free database expires | Certain | Low | Warm-up step before demos; the database is created on Day 4 and lasts beyond Oct 29; export is available if it must be rebuilt |
| Strangers drain the AI quota | Medium | Medium | Input cap, daily limit, short cooldown, friendly limit message |
| Real confidential text pasted | Low | High | Visible banner; no storage of pasted text; documented in the README |

**Cut list if time runs short (drop in this order):** Markdown download, seeded sample records, cooldown, second sample discussion. Never cut: draft, review, save, list, filter, print view, deployment.

## Release plan and acceptance criteria

| Day | Date | Hours | Milestone |
| --- | --- | --- | --- |
| 1 | Tue Oct 20 | done | Discovery, PRD, blueprint, pitch deck |
| 2 | Wed Oct 21 | 1 | Project setup, repository and running skeleton |
| 3 | Thu Oct 22 | 1 | AI drafting proven end to end |
| 4 | Fri Oct 23 | 1 | Database and decision record model |
| 5 | Sat Oct 24 | 3 | Capture, review and save flow |
| 6 | Sun Oct 25 | 3 | List, filters, detail, edit, delete, exports, AI limit |
| 7 | Mon Oct 26 | 1 | First live deployment on Render |
| 8 | Tue Oct 27 | 1 | Polish, resilience, sample data |
| 9 | Wed Oct 28 | 1 | Testing and bug fixes |
| 10 | Thu Oct 29 | 1 | Release v1.0, README, demo script, final deck |

**Acceptance scenarios (all must pass on the live URL)**

1. Given the home page, when a stranger clicks "Load sample" and "Draft record", then a nine-field draft appears with an ADM phase and domain pre-selected.
2. Given a draft, when the architect changes the ADM phase and saves, then the record appears in the list with the corrected tag.
3. Given several saved records, when the list is filtered by domain and ADM phase, then only matching records show and the count is correct.
4. Given a saved record, when the architect clicks Print, then the print preview is clean and can be saved as PDF.
5. Given a saved record, when the architect downloads Markdown, then the file contains all nine fields.
6. Given the daily limit is reached, when someone clicks Draft, then a friendly message appears and the manual path still works.
7. Given a discussion that omits the rationale, when it is drafted, then the rationale reads "Not stated in the discussion" and nothing is invented.
