# DecisionLens: 30-day growth plan (v1.0.0 to v1.1.0)

A realistic roadmap that turns the v1.0 MVP into a noticeably more complete product, one achievable milestone per day. Each day builds on the one before. It follows the order in `docs/future-scope.md`: make it safe, make it findable, make it insightful, then make the AI trustworthy.

**How to use it**

- "Day N" means the Nth working day of the plan, not a calendar date. Start the day after v1.0.0 is released.
- Weekdays are sized for about **1 hour**, weekend days (marked *) for about **3 hours**. If a day runs over, move the leftover to the next day and cut something from the next day, not the whole week.
- Use `docs/daily-build-prompt.md` every day. It tells the AI to read this plan, so the plan stays the source of truth.
- Every day ends the same way: tests pass (`dotnet test`), a leak check (`git grep` for key and database patterns) is clean, one commit and push, a manual deploy to Render, a smoke test on the live site.
- Free tools only. Anything that would cost money is an ADR and needs your approval first.
- Rest days are allowed. A missed day does not break the plan. Day 30 is the release.

**Working rules carried over from the capstone:** one step at a time, wait for output or a screenshot before the next step, never show secrets in a screenshot, give complete files, PowerShell commands, and record every real decision as an ADR.

---

## Week 1: Make it safe and easy to ship

**Day 1: Deploy automatically after tests pass.**
Build: extend the GitHub Actions workflow so that after the tests pass on `main`, it calls a Render deploy hook (stored as a GitHub secret). Files: `.github/workflows/ci.yml`. Done when: a push to `main` deploys with no manual click, and a failing test blocks the deploy. Commit: "Day 1: auto-deploy after tests".

**Day 2: Export all records.**
Build: a "Download all (Markdown)" button on Records that returns one file with every record, and a JSON export for backup. Files: `Services/MarkdownExporter.cs` (add a batch method), `Pages/Records/Index.cshtml(.cs)`. Done when: both downloads open and contain every record. Commit: "Day 2: export all records".

**Day 3: Import a backup.**
Build: upload a JSON backup on a new `/Records/Import` page; validate every record with the same rules as the form, skip duplicates, show a summary. Files: `Pages/Records/Import.cshtml(.cs)`, `Services/RecordImporter.cs`. Done when: export then import into an empty database restores everything. Commit: "Day 3: import backup".

**Day 4: Choose a durable free database.**
Build: no feature code. Compare free PostgreSQL hosts that do not expire after 30 days (check each one's current terms), write ADR-0011 with the decision, and list the exact steps to move. Files: `docs/architecture/adr/0011-...md`. Done when: the ADR is committed and the move steps are written. Commit: "Day 4: ADR for database hosting".

**Day 5\*: Move the database, rehearse the restore.**
Build: create the new database, point a local copy at it, run migrations, import the Day 2 backup, then switch Render's `ConnectionStrings__Default` and redeploy. Done when: the live site shows all records from the new database and the old one can be deleted. Commit: "Day 5: move database".

**Day 6\*: Health, logging and uptime.**
Build: make `/healthz` check the database connection, add structured logs for the main events (draft requested, draft failed with its code, record saved), and set up a free uptime ping so the site stays awake before demos. Files: `Program.cs`, `Pages/Capture.cshtml.cs`. Done when: `/healthz` returns an error when the database is unreachable and no pasted text appears in logs. Commit: "Day 6: health check and logging".

**Day 7: Tests and week-1 review.**
Build: unit tests for export, import and the backup format; update `README.md` and `docs/KNOWN-ISSUES.md`. Done when: `dotnet test` passes and the README shows the new backup feature. Commit: "Day 7: tests and docs for week 1".

## Week 2: Make records easy to find

**Day 8: Keyword search (simple).**
Build: a search box on Records that matches title, decision, context and rationale, combined with the existing phase and domain filters, as a shareable URL. Files: `Pages/Records/Index.cshtml(.cs)`. Done when: a search for "gateway" finds the right seeded record and Clear resets it. Commit: "Day 8: keyword search".

**Day 9: Real full-text search.**
Build: switch to PostgreSQL full-text search with a generated search column and index (a new migration), ranked results and highlighted matching words. Files: `Data/AppDbContext.cs`, a new migration, `Pages/Records/Index.cshtml(.cs)`. Done when: results are ranked, the migration applies on a fresh database, and search stays fast. Commit: "Day 9: full-text search".

**Day 10: Pagination.**
Build: 10 records per page with Previous/Next and a count, keeping all filters in the URL. Done when: with 25 records the page numbers and counts are right. Commit: "Day 10: pagination".

**Day 11: Sorting.**
Build: sort by date, title or status from the column headers, with an indicator of the current sort. Done when: sorting and pagination work together and the sort survives filtering. Commit: "Day 11: sorting".

**Day 12: More filters.**
Build: a status filter and an owner filter. Done when: any combination of filters gives the right count. Commit: "Day 12: status and owner filters".

**Day 13\*: Records page polish.**
Build: phase and domain chips that act as filters when clicked, a card layout on phones instead of a table, and a friendlier empty state. Done when: the page reads well at 375 px and Lighthouse accessibility stays at 90 or more. Commit: "Day 13: records page polish".

**Day 14\*: Tests, ADR and week-2 review.**
Build: tests for the search and filter logic (move the query building into a testable class if needed), an ADR for the search approach, README update. Done when: tests pass and the ADR is committed. Commit: "Day 14: search tests and ADR".

## Week 3: Make the portfolio visible

**Day 15: Dashboard queries.**
Build: a service that returns counts per ADM phase, per domain, per status and per month. Files: `Services/DashboardService.cs`, unit tests. Done when: the numbers match a manual count of the seeded records. Commit: "Day 15: dashboard queries".

**Day 16: Dashboard page.**
Build: a `/Dashboard` page with simple bar charts in plain SVG (no chart library), linked from the menu. Done when: it shows phase and domain charts and works on a phone. Commit: "Day 16: dashboard page".

**Day 17: Dashboard drill-down.**
Build: clicking a bar opens Records filtered to that phase or domain; add a "decisions over time" chart and a list of the 5 newest. Done when: every bar links to the right filtered list. Commit: "Day 17: dashboard drill-down".

**Day 18: Supersede data model.**
Build: add a nullable `SupersededById` to the record, a migration, and validation (a record cannot supersede itself, no cycles). Files: `Models/DecisionRecord.cs`, a new migration, `RecordInput.cs`. Done when: the migration applies and unit tests cover the rules. Commit: "Day 18: supersede model".

**Day 19: Supersede in the edit form.**
Build: a dropdown on Edit to choose the decision that replaces this one, which also sets the status to Superseded. Done when: saving updates the link and rejects invalid choices. Commit: "Day 19: supersede in edit".

**Day 20\*: Show the decision chain.**
Build: Details shows "Superseded by..." and "Replaces..." links, the list shows a badge, and Markdown and Print include the link. Done when: following the chain works in both directions. Commit: "Day 20: decision chain".

**Day 21\*: Tests, ADR and week-3 review.**
Build: tests for the dashboard and the supersede rules, ADR-0012 for linking, README and screenshots update. Done when: all tests pass and the capability map shows 3.3 as delivered. Commit: "Day 21: dashboard and supersede docs".

## Week 4: Make the AI trustworthy, then release

**Day 22: Evaluation set.**
Build: 12 fictional discussions with the expected phase and domain, stored as JSON in a test data folder, including thin and misleading ones. Done when: the file is committed and reviewed against the TOGAF phase guidance. Commit: "Day 22: AI evaluation set".

**Day 23: Evaluation runner.**
Build: a small console project or test category that runs the set through the real drafter (using your user-secrets key) and prints a score table, with a delay between calls to respect the cooldown and quota. Done when: you have a baseline score saved in `docs`. Commit: "Day 23: AI evaluation runner".

**Day 24: Improve the prompt with evidence.**
Build: change only the guidance text in `PromptText.cs`, re-run the set, and keep the change only if the score improves. Record before and after. Done when: the score is equal or better and no sample invents facts. Commit: "Day 24: prompt tuning with evaluation".

**Day 25: A fallback AI provider.**
Build: a second implementation of `IDecisionDrafter` for a free OpenAI-compatible service, chosen by the `Ai:Provider` setting; the app falls back only when the first is rate-limited or down. Check the free terms first. Done when: switching the setting changes the provider and the validator still guards both. Commit: "Day 25: fallback provider".

**Day 26: Redaction warnings.**
Build: in the browser, a gentle warning when pasted text looks like an email address, a phone number or a key ("this looks like personal or secret data"). Done when: the warning shows for sample patterns and never blocks drafting. Commit: "Day 26: redaction warnings".

**Day 27\*: End-to-end browser tests.**
Build: Playwright tests for the main flows (load sample, draft with a stubbed AI, review, save, filter, export). Done when: the tests run locally and in the CI workflow. Commit: "Day 27: end-to-end tests".

**Day 28\*: Audit and hardening.**
Build: re-run Lighthouse on every page, check keyboard use, review logs for sensitive text, update `docs/KNOWN-ISSUES.md`, and fix what you find. Done when: accessibility is 90 or more everywhere and the known-issues list is current. Commit: "Day 28: audit and fixes".

**Day 29: Documentation and release candidate.**
Build: update the README, the capability map and gap analysis, add a `CHANGELOG.md`, refresh the demo script and screenshots. Done when: a stranger can follow the README and the demo script matches the product. Commit: "Day 29: docs and changelog".

**Day 30: Release v1.1.0.**
Build: a final regression pass on the live site, a secrets scan, tag `v1.1.0`, publish the GitHub release, update the pitch deck and write a short "what I learned" post. Done when: the live site shows everything, the release is published and the plan's Day 1 to Day 29 boxes are ticked. Commit: "Day 30: v1.1.0 release".

---

## Progress tracker

Copy this into your notes and tick as you go.

| Week | Days | Theme | Done |
| --- | --- | --- | --- |
| 1 | 1 to 7 | Safe and easy to ship | [ ] |
| 2 | 8 to 14 | Easy to find | [ ] |
| 3 | 15 to 21 | Visible portfolio | [ ] |
| 4 | 22 to 30 | Trustworthy AI and release | [ ] |

## If you fall behind

Cut in this order: Day 26 (redaction warnings), Day 13 (card layout), Day 17 (drill-down), Day 25 (fallback provider). Never cut: backups (Days 2 and 3), the database move (Days 4 and 5), tests and the release day.
