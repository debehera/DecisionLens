# DecisionLens: future scope

How this specific project could evolve over the next 3, 6 and 12 months. The order follows the roadmap already written in the Day 10 handoff: keyword search, a decisions dashboard, linking and superseding decisions, then accounts and integrations.

**Guiding rules (do not break these as the product grows)**

1. **The human confirms.** The AI proposes and the architect decides, especially for the TOGAF tags (ADR-0002).
2. **Never invent facts.** Missing information stays "Not stated in the discussion".
3. **Do not store raw discussions by default** (ADR-0007). If that ever changes, it needs a new ADR, a clear opt-in and a retention rule.
4. **Stay free unless you decide otherwise.** Each item below names a free path. Check the free-tier terms before relying on them, because they change.
5. **Every new decision gets an ADR.** The project documents itself.

## Next 3 months: make v1.0 solid and useful every day

| Area | What | Why it matters | Rough effort |
| --- | --- | --- | --- |
| Find | Keyword search across title, decision, context and rationale (start with PostgreSQL full-text search) | The first thing a returning user wants is to find "that database decision" | Small |
| Understand | A dashboard: decisions per ADM phase, per domain, per status, over time (server-rendered, plain SVG or a small chart script) | Turns a list into a picture of the architecture portfolio | Medium |
| Link | Mark a decision as superseded by another and show the chain (closes the gap listed in capability 3.3) | Real architecture decisions replace each other | Medium |
| Status | A clearer status workflow (Proposed, Accepted, Rejected, Superseded) with rules and a status filter | Governance needs to see what is still open | Small |
| Safety net | Export all records as one Markdown or JSON file, plus a restore path | The free database expires after 30 days. A backup removes the fear | Small |
| Database | Move to a free PostgreSQL host without a 30-day expiry (evaluate Neon, Supabase or similar) or automate the backup and re-create routine | The biggest operational risk in v1.0 | Small to medium |
| AI quality | An evaluation set: 10 to 15 fictional discussions with expected tags, and a script that scores a prompt change | Prompt tuning with evidence, not by feel. Catches regressions | Medium |
| Engineering | Deploy automatically from GitHub Actions after tests pass (Render's own auto-deploy did not fire reliably); add an uptime ping to avoid cold starts | Faster, safer releases | Small |
| Polish | Pagination, sort by column, tag chips, a nicer empty state, dark mode | Needed once there are more than 20 records | Small |

**Outcome at 3 months:** a tool you would use for your own team's real (sanitized) decisions, with search, a dashboard, supersession and a safe backup.

## Months 4 to 6: from demo to a team tool

| Area | What | Notes |
| --- | --- | --- |
| Accounts | Sign-in (ASP.NET Core Identity, or a free identity provider) and private workspaces | Retires the "no login" limitation from ADR-0006. Needs a new ADR |
| Roles | Writer, reviewer and reader roles. A reviewer approves a proposed decision | Moves capability 3.1 from "informal" to a real workflow |
| History | Version history and an audit trail of who changed what | Closes capability 3.4 |
| Capture | A "paste a link" or e-mail-in path, and a template for meeting notes | Reduces friction at the moment decisions happen |
| AI | A second AI provider as a fallback, behind the existing `IDecisionDrafter` interface | Protects against free-tier changes and outages |
| Quality | End-to-end browser tests (Playwright) for the capture, review and save flow | Catches regressions in the part users touch most |
| Privacy | A redaction helper that warns when pasted text looks like names, emails or keys | Reduces the "sanitized text only" risk |
| Docs | A public changelog and a contribution guide | Makes the repository a place others can join |

**Outcome at 6 months:** several architects can use it with their own workspaces, a review step and a history, and the AI provider can be swapped without a rewrite.

## Months 7 to 12: connect it to how architecture teams work

| Area | What | Notes |
| --- | --- | --- |
| Repository sync | Write each accepted decision as an ADR Markdown file into a Git repository (through a pull request) | Meets architects where they already keep ADRs |
| Integrations | Slack or Teams message capture, and links to Jira or Confluence pages | Capability 5.3. Keep each integration optional |
| Architecture repository | Link decisions to capabilities from the business capability map and to systems | Connects decisions to the TOGAF architecture repository |
| Governance | An architecture board view: decisions awaiting review, overdue reviews, stale proposals | Capability 3.1 and 4.4 at team scale |
| Analytics | Decision health: how many are superseded, how long they take to reach Accepted, which phases are neglected | Portfolio analytics (capability 4.4) |
| Confidentiality | Evaluate an option for running a smaller open-source model inside the company's own environment | Answers the biggest enterprise objection: text leaves the company |
| Public API | A small read API so other tools can query decisions | Only after accounts and roles exist |

**Outcome at 12 months:** DecisionLens is an architecture decision hub for a team: decisions captured from where they happen, reviewed, linked to the architecture repository and visible to governance.

## Risks to watch

| Risk | Mitigation |
| --- | --- |
| Free AI quotas or model names change | Keep the interface, keep a second provider ready, check the terms monthly |
| The free database or hosting limits get in the way as usage grows | Decide on a hosting plan deliberately and write an ADR. Paid services only with your approval |
| Confidential data ends up in the AI prompt | Keep the banner, add redaction hints, offer a local-model option |
| Scope grows faster than quality | One milestone at a time, tests first, a known-issues list that is kept up to date |
