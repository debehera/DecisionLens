# DecisionLens: Gap Analysis

| Item | Value |
| --- | --- |
| TOGAF phase | B to D (gap analysis) and E/F (roadmap input) |
| Version | 1.0 (Day 1, Tue Oct 20, 2026) |
| Related | `02-business-capability-map.md`, the PRD and the Implementation Blueprint |

## 1. Purpose

Gap analysis compares the **baseline** (how decisions are handled today) with the **target** (DecisionLens v1.0) and with the **future vision**. It shows what is new, what stays, what is replaced, and what is still missing after v1.0.

## 2. Baseline and target at a glance

| Aspect | Baseline (today) | Target (v1.0) | Vision (later) |
| --- | --- | --- | --- |
| Where decisions are discussed | Chat, meetings, email, tickets | Same places; text is pasted into DecisionLens | Imports from those tools |
| How a record is written | Manually, if at all | AI drafts, architect confirms | Same, with more templates |
| TOGAF alignment | None | ADM phase and domain on every record | Dashboards by phase and domain |
| Where records live | Scattered or nowhere | One shared database | Searchable decision memory with accounts |
| How people find a decision | Ask colleagues | Filter by phase and domain | Keyword search and links between decisions |
| How a record is shared | Copy and paste | Print or save as PDF; Markdown download | Word and generated PDF |
| Cost and effort | High effort | Zero running cost; paste, review, save | Same principle |

## 3. Gap matrix: baseline elements against the target

Gap types follow TOGAF's gap analysis matrix: **Retained** (stays), **Eliminated** (replaced), **New** (added), **Partly closed** (reduced but not removed).

| # | Baseline element or need | Gap type | Target element | Closed by | Residual gap after v1.0 |
| --- | --- | --- | --- | --- | --- |
| 1 | Discussion in chat, meetings, email and tickets | Retained | Pasted text as the input | Capture page | None; the discussion still happens there |
| 2 | Manual rewriting of a discussion into a record | Eliminated | AI-drafted record | Draft service | Quality depends on the AI and on the review |
| 3 | No standard record structure | New | Nine-field decision record | Record model and form | None for v1.0 |
| 4 | No link to TOGAF | New | ADM phase and domain tags, AI-suggested and human-confirmed | Draft service, review form | Wrong tags are possible; the human review is the control |
| 5 | No owner, status or date | New | Owner, status, date fields | Review form | Owner is free text, not linked to a directory |
| 6 | Decisions not stored | New | Record database | Database and save flow | One shared list; no per-user separation |
| 7 | Cannot find decisions | Partly closed | Browse and filter by phase and domain | List page | No keyword search |
| 8 | Cannot see how decisions relate | Not closed | None in v1.0 | Not applicable | Linking and supersession are future scope |
| 9 | No history of changes | Not closed | None in v1.0 | Not applicable | No version history |
| 10 | Copy and paste to share | Partly closed | Print view and Markdown download | Export features | No Word or generated PDF |
| 11 | Asking colleagues "why?" | Partly closed | Readable records with context and rationale | Record page | Only for decisions that were captured |
| 12 | Sensitive text handling | Partly closed | Source text not stored; banner; fictional data only | Privacy design | Text still goes to a free AI service, so real confidential data is not allowed |
| 13 | Control of who can read and change | Not closed | None in v1.0 | Not applicable | No login; anyone with the link can read and edit |

## 4. Capability gap summary

This restates the map in `02-business-capability-map.md` as a gap list, ranked for the roadmap.

| Priority | Gap | Capability | Why it matters | Suggested release |
| --- | --- | --- | --- | --- |
| 1 | Keyword search | 4.3 | Finding a decision by words is the next thing readers ask for | v1.1 |
| 2 | Portfolio dashboard by phase and domain | 4.4 | Shows governance patterns; uses tags already captured | v1.1 |
| 3 | Linked and superseding decisions | 3.3 | Turns a log into a decision memory | v1.2 |
| 4 | Access control and accounts | 6.3 | Required before any real data is used | v2.0 |
| 5 | Private or approved AI for confidential text | 6.2 | Required before any real data is used | v2.0 |
| 6 | Version history and audit trail | 3.4 | Governance evidence | v2.0 |
| 7 | Integrations with Teams, Slack, Jira, email | 5.3 | Removes the paste step | v2.x |
| 8 | Notifications | 5.2 | Keeps readers informed | v2.x |
| 9 | Word and generated PDF export | 5.1 | Convenience for formal documents | v2.x |

## 5. Gap closure approach and roadmap

| Release | Theme | Gaps closed | Gate before starting |
| --- | --- | --- | --- |
| v1.0 (Oct 29, 2026) | Capture, review, save, browse, share | 2, 3, 4, 5, 6, 7 (partly), 10 (partly) | Day 10 success criteria met |
| v1.1 | Find and see patterns | Search, dashboard | A new ADR for the search approach |
| v1.2 | Connect decisions | Linking and supersession | A new ADR for the data model change |
| v2.0 | Trust and scale | Accounts, access control, private AI, version history | A new ADR for each; a data-protection review |
| v2.x | Reach | Integrations, notifications, richer export | One ADR per integration |

**Change rule (TOGAF Phase H):** any change to a locked decision, or any item moved from "left out" into scope, starts with a new Architecture Decision Record in `docs/architecture/adr/`. A superseded ADR is not deleted; its status changes to Superseded and it points to the new one.

## 6. Dependencies and risks tied to the gaps

| Dependency or risk | Affects gap | Note |
| --- | --- | --- |
| Free AI tier terms and limits | 2, 4, 12 | Re-checked on the day the key is created; provider can be swapped |
| Free hosting sleeps and free database expires | 6 | Warm-up before demos; the database can be rebuilt |
| 13 build hours | All v1.0 gaps | A fixed cut list protects the core flow |
| No login | 13 | Accepted for v1.0 because only fictional data is used |
