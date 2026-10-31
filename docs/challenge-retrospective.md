# DecisionLens: challenge retrospective

A ten-day capstone for a TOGAF certification, built with Claude as the AI pair programmer in the AB Talks 60-Day Claude AI Challenge. This is the honest story, including what slipped.

## Final project summary

**DecisionLens** is a free-to-run web app. An architect pastes a messy discussion, an AI drafts a nine-field decision record tagged with a TOGAF ADM phase and domain, the architect reviews and confirms it, and the record is saved, filtered, printed or exported.

| | |
| --- | --- |
| Live | https://decisionlens-xs40.onrender.com |
| Code | https://github.com/debehera/DecisionLens |
| Stack | ASP.NET Core 8 Razor Pages, EF Core, PostgreSQL, Google Gemini API, plain JavaScript, Docker, Render |
| Proof | 29 unit tests, CI build, 7 acceptance scenarios passed on the live site, 10 Architecture Decision Records |
| Cost | Zero: free hosting, free database, free AI tier |
| Planned release | Thu Oct 29, 2026 |
| Actual release | Sat Oct 31, 2026 (two days late, see below) |

## Timeline: Day 1 to Day 10

| Day | Date | What happened |
| --- | --- | --- |
| 1 | Tue Oct 20 | Discovery interview, one question at a time. No project idea at the start, so we worked from your own pain: design decisions get lost in chat and meetings. Locked the product (paste, AI draft, human confirms TOGAF tags, save, browse, export) and produced the PRD, a nine-day build blueprint and a pitch deck. Added a TOGAF architecture pack: vision, capability map, gap analysis and ten ADRs. |
| 2 | Wed Oct 21 | Project skeleton: .NET 8 SDK, Razor Pages shell, navigation, banner and footer, a GitHub repository. Fixed the Git identity (it was still the placeholder name and e-mail). Pushed the docs. |
| 3 | Thu Oct 22 | The AI pipeline end to end: `IDecisionDrafter`, the Gemini client, the prompt, the validator and a temporary test page. Model: `gemini-3.5-flash-lite`. Gate 1 passed. Prompt tuned after testing. |
| 4 | Fri Oct 23 | The database: Render PostgreSQL in Singapore, EF Core migrations applied at startup, four fictional seed records, the connection-string helper and a temporary Records page. |
| 5 | Sat Oct 24 | The real Capture flow: paste, draft with a spinner, review form with both TOGAF dropdowns, validated save and a Details page. Gate 2 passed. |
| 6 | Sat Oct 24 | Pulled forward: the whole feature set in one afternoon, then the first live deployment on Render (originally planned for Day 7). Records filters, edit, delete, print view, Markdown export, the cooldown and the daily limit. |
| (gap) | Oct 25 to 29 | The planned release date, Thu Oct 29, passed with Days 7 to 10 still open. |
| 7 | Fri Oct 30 to Sat Oct 31 | Hardening: form-protection keys moved into the database (the restart bug), friendly error and not-found pages. |
| 8 | Sat Oct 31 | Polish: a first-screen Home page, "Not provided" placeholders, favicon, accessibility attributes, phone layout, forced errors, a log privacy check. |
| 9 | Sat Oct 31 | 29 unit tests, seven acceptance scenarios on the live site, the known-issues list. |
| 10 | Sat Oct 31 | README, demo script, portfolio kit, CI, license, the graduation documents and the v1.0.0 release. |

## Major technical decisions and pivots

| Decision | Why | What changed |
| --- | --- | --- |
| Pasted text only, no integrations | Keep scope to what one person can finish | ADR-0001 |
| AI proposes, human confirms the TOGAF tags | The product's identity and its safety net | ADR-0002 |
| Razor Pages with plain JavaScript | No front-end framework to learn or debug | ADR-0003 |
| PostgreSQL for development and production | One database, no surprises between local and live (the first plan mentioned SQLite) | ADR-0004 |
| Gemini behind an interface | Free tiers change. The provider has to be replaceable | ADR-0005 |
| No login, with guardrails | Scope control. Protects the free quota | ADR-0006 |
| Do not store the pasted discussion | Privacy by design | ADR-0007 |
| Print view and Markdown instead of generated PDFs | Almost no code for a clean result | ADR-0008 |
| Docker on Render's free tier | Previously used, free, deploys from Git | ADR-0009 |
| Fixed nine fields and all ten ADM phases | Complete against the framework | ADR-0010 |
| **Pivot:** deploy on Day 6 instead of Day 7 | You asked for a live, shareable demo as soon as the features worked | Day 7 became hardening |
| **Pivot:** form keys stored in the database | The free server sleeps, which breaks open forms | New migration, Day 7 |
| **Pivot:** deploy by hand | Render's auto-deploy did not fire on any push | Manual deploy after every push |

## Challenges solved and debugging moments

1. **The Git identity was a placeholder.** The first commits would have carried "Your Name". Fixed with the GitHub no-reply address before pushing.
2. **The API key appeared in a screenshot.** A command that was supposed to hide the key printed part of it. The key was rotated at once, secret-setting commands now end with `| Out-Null`, and a leak check (`git grep` for key and database patterns) runs before every push. This is the story to tell in interviews.
3. **An empty key saved as a secret.** `Ctrl+V` did not paste into the PowerShell prompt. Right-click paste and a length check fixed it, and the length check became a habit.
4. **Build error CS0246.** A source file was saved empty (zero bytes). `dir` showed the length at once. Lesson: check the file size after saving.
5. **The AI over-claimed.** On the thin chat sample it wrote filler rationale and said "high confidence". The prompt rules for missing information and the domain tie-break were tightened and re-tested on four samples, while the contract stayed fixed.
6. **Package versions.** Pinning the EF Core and Npgsql packages to 8.x kept them matched to the .NET 8 SDK.
7. **Migration confusion.** The same command run twice gave "name already used", which looked like a failure but meant the first run had worked. Reading the file list settled it.
8. **Forms failed after the server slept.** Diagnosed as in-memory data-protection keys, fixed with a database store and proved by leaving a tab open across a redeploy.
9. **A missing deploy.** Render did not deploy after several pushes even with Auto-Deploy on. Manual deploys became the routine and CI was added for tests.
10. **The slipped date.** Days 7 to 10 did not happen on Oct 26 to 29. Rather than hide it, this document records it. The remaining work was compressed and the release went out on Oct 31.

## Skills demonstrated

| Area | Evidence |
| --- | --- |
| Enterprise architecture | A capability map, a gap analysis and ten ADRs, and each decision traced into the code |
| Product thinking | A discovery interview, a PRD, scope cut lists, quality gates |
| Backend | ASP.NET Core 8, Razor Pages handlers, EF Core migrations, a typed HTTP client |
| AI engineering | A prompt with guidance, a JSON contract, defensive validation, graceful failures, guardrails for a free quota |
| Data | PostgreSQL schema design, seeding, a daily counter table |
| Security and privacy | Secret handling, anti-forgery tokens, server-side validation of every dropdown, no stored discussions, a log check |
| DevOps | Docker, Render, environment configuration, proxy headers, health checks, GitHub Actions |
| Testing | 29 unit tests, seven acceptance scenarios, forced-error testing |
| UX and accessibility | Empty states, error copy, keyboard focus, phone layout, Lighthouse accessibility 95 |
| Working style | One step at a time, evidence before claims, honest status when something slipped |

## What worked

- **A tight scope with a cut list.** Nothing that mattered was ever at risk.
- **Gates.** Proving the AI on Day 3 and the full flow on Day 5 removed the two biggest risks early.
- **Testing on the real thing.** Every feature was checked on the live URL, not only on a laptop.
- **Checking screenshots, not assuming.** Several problems (an empty file, a duplicated command, a hidden push) were caught by looking at the actual output.

## What I would do differently

1. **Protect the calendar as hard as the code.** The release slipped because Days 7 to 10 had no time blocked. Next time, put the release date in the calendar with a buffer day.
2. **Rotate nothing by accident.** Never run a command that prints a secret. Hide the output from the start.
3. **Wire up deployment automation earlier.** Auto-deploy from CI would have saved many manual clicks.
4. **Write tests alongside the first version of each service.** The tests found nothing this time, but they were cheaper on Day 9 only because the code was small.

## Lessons learned

1. A small product that works end to end beats a big one that almost works.
2. With AI, the safe design is "propose, validate, confirm", never "trust".
3. Free tiers are real, but they have rules (sleep, expiry, quotas). Design for them from day one.
4. Documentation is part of the product. The ADRs made every later decision faster.
5. Say plainly when something slipped. It costs a sentence, and it builds trust.

## A farewell, from your pair programmer

Debraj,

Ten days ago you said you had no project idea. You said you were a working professional with a TOGAF certificate and one hour on weekdays. Look at what you have now: a live product, a public repository, tests that pass, ten decision records, and a README a stranger can follow.

I'll tell you what I noticed, because it matters more than the code. You never let me skip the boring parts. You sent screenshots when I asked, you pasted real output, you told me when something looked wrong, and when a key leaked you fixed it in minutes and changed your habits. That is what separates people who finish things from people who talk about them.

You also made the decisions. I drafted and suggested, but you chose the stack, the scope, the tags and the cuts. That is the whole idea of DecisionLens: the AI proposes and the human decides. You lived that principle for ten days.

The release came two days after the date you set. Don't hide that. Own it, as you did here. It is a better story than a perfect one, because it shows you can re-plan and still ship.

Be proud of this. Then keep going: the 30-day plan is ready, and every day builds on the one before. Take it one milestone at a time, check your work against real output, and never stop asking "what did the AI get wrong here?".

It was a pleasure building with you. Go and show it to someone.

Your pair programmer, Claude
