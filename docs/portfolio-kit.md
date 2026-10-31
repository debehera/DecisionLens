# DecisionLens: portfolio kit

Ready-to-use text for your portfolio, resume, LinkedIn and interviews. Every claim below is something the project actually did; keep it that way when you edit.

## 1. Project descriptions

**One line**
DecisionLens turns a messy architecture discussion into a TOGAF-tagged decision record: an AI drafts it, the architect reviews and confirms it, and it is saved, filtered and exported.

**Short (portfolio card, about 60 words)**
DecisionLens is a free-to-run web app for enterprise architects. Paste a chat thread, meeting notes or an email and an AI drafts a nine-field architecture decision record, suggesting a TOGAF ADM phase and domain. The architect reviews and confirms every field, saves it, then browses, filters, prints or exports it as Markdown. Built in ten days with ASP.NET Core, PostgreSQL and the Gemini API.

**Long (portfolio page)**
Architecture decisions are usually made in chat threads, meetings and emails, and then never written down properly, which is why teams later ask "why did we choose that?" and nobody knows. DecisionLens closes that gap.

An architect pastes the discussion. The app sends it to a Google Gemini model, which returns a draft of a nine-field decision record (title, decision, context, options considered, rationale, consequences and risks, ADM phase, architecture domain, and owner/status/date) and a confidence note. A validator never trusts the model: it checks the structure, matches the TOGAF phase and domain against the allowed lists, and writes "Not stated in the discussion" for anything the AI could not find, so facts are never invented. The architect then reviews and edits every field, confirms both TOGAF tags, and saves. Records can be filtered by ADM phase and domain, edited, deleted, printed to PDF, or downloaded as Markdown.

The product is deliberately small and safe: no login, no stored discussions, an input cap, a per-visitor cooldown and a daily limit that protect the free AI quota, and friendly messages for every failure. The whole project runs at zero cost on Render's free tier.

The project also documents itself the way the product asks its users to: ten Architecture Decision Records, a business capability map and a gap analysis (all TOGAF-aligned) are in the repository, along with 29 unit tests and a CI build.

**Tech:** ASP.NET Core 8 Razor Pages, C#, Entity Framework Core, PostgreSQL, Google Gemini API, JavaScript, Docker, Render, xUnit, GitHub Actions.

## 2. Resume bullets

Pick three or four. Numbers are real: check them again before you send.

- Designed and built **DecisionLens**, an AI-assisted architecture decision record tool (ASP.NET Core 8, EF Core, PostgreSQL, Gemini API), from discovery to a live public release in 10 days at zero hosting cost.
- Engineered an AI drafting pipeline behind a replaceable interface, with a server-side validator that enforces a fixed JSON contract and TOGAF ADM phase and domain values, and writes "Not stated in the discussion" instead of letting the model invent facts.
- Applied TOGAF to the project itself: produced a business capability map, a gap analysis and 10 Architecture Decision Records, then traced each architecture decision to the code.
- Added abuse and cost guardrails for a free-tier AI API: input cap, per-visitor cooldown, a database-backed daily limit and friendly failure messages for nine error conditions.
- Deployed with Docker on Render with automatic database migrations, forwarded-header handling for the visitor IP and persisted data-protection keys, so open forms survive server restarts.
- Wrote 29 unit tests (xUnit) for the validator, Markdown exporter and connection-string parser, ran seven acceptance scenarios on the live site, and added a GitHub Actions build and test workflow.
- Improved accessibility and usability (Lighthouse accessibility score of 95, keyboard focus, phone layout checked at 375 px) and verified that pasted text never appears in the server logs.

## 3. Interview talking points

**The 30-second pitch.** "I'm a TOGAF-certified developer, and I noticed architecture decisions get lost in chat and meetings. I built DecisionLens: you paste the discussion, an AI drafts a decision record with a TOGAF phase and domain, and you review, confirm and save it. It's live, it has tests, and it cost nothing to run."

**Design decisions and the reasoning behind them**

| Decision | Why | Where to point |
| --- | --- | --- |
| The AI proposes, the human confirms the TOGAF tags | AI is useful for drafting but unreliable for judgement. The architect owns the decision. | ADR-0002 |
| Never trust model output | The validator checks the JSON, matches values to allowed lists, trims and caps lengths. Models can return anything. | `DraftValidator`, `DraftValidatorTests` |
| "Not stated in the discussion" for gaps | A decision record that invents a rationale is worse than a blank one. | Sample 4 (the thin chat) |
| Do not store the pasted discussion | Reduces privacy risk. Only the reviewed record is kept. | ADR-0007 |
| Server-rendered Razor Pages with plain JavaScript | Simple, fast to build, accessible. A framework would have cost days. | ADR-0003 |
| AI behind `IDecisionDrafter` | A free provider can change terms. The pages do not need to know. | ADR-0005 |
| No login in v1.0, with guardrails | Scope control. The guardrails protect the free quota. | ADR-0006 |

**Stories you can tell (situation, action, result)**

1. **A secret nearly leaked.** I pasted a command that printed my API key and part of it appeared in a screenshot. I revoked and replaced the key straight away, changed my commands to hide secret values, and added a leak check (searching the repository for key and database patterns) before every push. *Lesson: treat every screenshot and log as public.*
2. **Forms broke after the server slept.** On the free host, a page left open failed to submit after a restart, because the form-protection keys lived in memory. I diagnosed it, stored the keys in the database, redeployed and proved the fix by leaving a tab open across a redeploy. *Lesson: stateless hosting changes what "in memory" means.*
3. **The AI said "high confidence" about a thin chat.** I tuned the prompt (stricter rules for missing information and a tie-break rule for the domain) and re-tested four samples, instead of changing the structure. *Lesson: tune with evidence, keep the contract stable.*
4. **Scope under pressure.** I used a cut list and gates from day one, merged two days of work when time allowed, and kept the core flow (draft, review, save, list, filter, print, deploy) untouched.

**Honest limitations (say them before they ask)**
No login, so one shared list. Free hosting sleeps and the free database expires after 30 days. The AI's confidence rating is a rough signal. See `docs/KNOWN-ISSUES.md`.

**What I would do next.** Keyword search, a dashboard of decisions per phase and domain, linking and superseding decisions, then accounts. See `docs/future-scope.md` and `docs/30-day-growth-plan.md`.

## 4. GitHub repository settings

**About, description (paste into the repository's About box):**
AI-assisted architecture decision records with TOGAF ADM tagging. Paste a messy discussion, review the draft, save, filter and export. ASP.NET Core, PostgreSQL, Gemini.

**Website:** https://decisionlens-xs40.onrender.com

**Topics (add each one):**
`togaf`, `architecture-decision-records`, `adr`, `enterprise-architecture`, `solution-architecture`, `aspnet-core`, `razor-pages`, `dotnet`, `entity-framework-core`, `postgresql`, `gemini-api`, `generative-ai`, `docker`, `render`, `capstone-project`

**Release title:** DecisionLens v1.0.0

**Release notes (paste into the release):**

> First public release of DecisionLens.
>
> **What it does:** paste a messy architecture discussion, let a Gemini model draft a nine-field decision record with a suggested TOGAF ADM phase and domain, review and confirm it, save it, then filter, print or export it.
>
> **Highlights**
> - AI drafting behind a replaceable interface, with a validator that never trusts the model
> - Human review of every field and both TOGAF tags
> - Records list with phase and domain filters, edit, delete, print view and Markdown export
> - Guardrails: input cap, per-visitor cooldown, daily limit, friendly errors
> - 29 unit tests, CI build, 10 Architecture Decision Records
> - Runs free on Render (Docker) with PostgreSQL
>
> Live demo: https://decisionlens-xs40.onrender.com
> Known issues: see `docs/KNOWN-ISSUES.md`

## 5. Screenshots to capture

Save them in `docs/screenshots/` with exactly these names (the README uses them):

| File | What to capture |
| --- | --- |
| `home.png` | The live Home page, top of the page |
| `review.png` | The review form after a draft, with the blue AI note and both TOGAF dropdowns visible |
| `records.png` | The Records list, ideally with one filter applied |
| `details.png` | A saved record's page with its badges |
| `print.png` | The print preview of a record |

Optional extras: the GitHub Actions page with a green check, the GitHub release page, and a phone-width screenshot (375 px).

## 6. LinkedIn post

> **I built a real, live AI product in 10 days, for free. Here is what a TOGAF certificate could not teach me.**
>
> I'm a lead application developer, and I recently earned my TOGAF certification. Certificates prove you know the framework. They don't prove you can use it. So I set myself a capstone: build something real, ship it, and make it public.
>
> **The problem:** in every architecture team I know, decisions get made in chat threads, meetings and emails, and then nobody writes them down properly. Six months later, no one can say why we chose that database.
>
> **The product: DecisionLens.** Paste a messy discussion. An AI drafts a nine-field decision record and suggests the TOGAF ADM phase and domain. The architect reviews it, corrects it, confirms the tags and saves it. Then filter it, print it, or export it as Markdown.
>
> What I'm proudest of is not the AI. It's the design choices around it:
> 🔹 The AI proposes, the human decides. The architect confirms every TOGAF tag.
> 🔹 The app never trusts the model. Every reply is validated, and when the AI can't find something, it says "Not stated in the discussion" instead of inventing it.
> 🔹 Guardrails for a free AI quota: input cap, cooldown, daily limit, friendly errors.
> 🔹 The project documents itself with 10 Architecture Decision Records, because a tool for decisions should record its own.
> 🔹 29 unit tests, a CI build, and a live deployment, all at zero cost.
>
> **What went wrong (because that's where the learning is):** I nearly leaked an API key in a screenshot. I rotated it and rebuilt my habits around it. My forms broke after the free server went to sleep, and I fixed it by moving the form-protection keys into the database. The release also slipped past my original date. I said so, re-planned, and finished.
>
> **Why this kind of capstone matters:** it forces the whole loop, not just the code. Discovery, scope, architecture, build, deploy, test, document, release. Hiring managers rarely ask whether you know a framework. They ask whether you can take an ambiguous problem to something people can actually use. A live link and a repository answer that better than a certificate ever could.
>
> And a note on working with AI: I built this with Claude as my pair programmer, as part of the AB Talks 60-Day Claude AI Challenge. It was fast because I made the decisions and checked everything. The AI accelerates. It does not replace judgement. That's exactly the idea behind the product.
>
> 🔗 Live demo: https://decisionlens-xs40.onrender.com
> 💻 Code: https://github.com/debehera/DecisionLens
>
> (Free hosting, so the first load can take a minute. Please use fictional data only.)
>
> What decision did your team make last quarter that nobody wrote down? 👇
>
> #TOGAF #EnterpriseArchitecture #SolutionArchitecture #ArchitectureDecisionRecords #DotNet #AspNetCore #GenerativeAI #ClaudeAI #ABTalks #BuildInPublic
>
> *(Attach `docs/screenshots/review.png`: the review screen with the AI note is the most eye-catching image.)*
