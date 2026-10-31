# DecisionLens: three-minute demo script

**Live URL:** https://decisionlens-xs40.onrender.com

## Before you start (5 minutes early)

1. Open the live URL so the free server wakes up (it can take about a minute).
2. Open **Capture**, load sample 1, click **Draft with AI** once and check it works. Then delete that test record if you saved it.
3. Check "Drafts left today" is well above zero.
4. Have a second tab open on **Records**, and the repository open on GitHub.
5. Backup plan: if the AI is busy or the limit is reached, use **Start blank**, or show the saved screenshots in `docs/screenshots`.

## The script

| Time | What you do | What you say |
| --- | --- | --- |
| 0:00 (20 s) | Show the **Home** page | "Architecture decisions get made in chat threads, meetings and emails, and then nobody writes them down properly. Six months later no one can say why we chose that database. DecisionLens fixes that." |
| 0:20 (30 s) | Click **Try it with a sample**, point at the text area and counter | "I paste a real-looking discussion. These are all fictional. There is an input cap and a counter, because the AI is a free service and I protect it." |
| 0:50 (30 s) | Click **Draft with AI**, wait for the form, point at the **AI note** | "In a few seconds I get a nine-field decision record. Notice the AI note: it suggests a TOGAF ADM phase and domain and tells me how confident it is. Anything it could not find reads 'Not stated in the discussion', so it never invents facts." |
| 1:20 (30 s) | Change the **ADM phase** to a different value, set an **Owner**, click **Save record** | "The AI proposes, but I decide. I'm the architect, so I correct the phase and confirm. The server re-checks every value, even if someone tampers with the page." |
| 1:50 (20 s) | Go to **Records**, filter by domain and phase | "Now the decisions are searchable by where they sit in TOGAF: phase and domain." |
| 2:10 (20 s) | Open the record, click **Print** (preview), then **Markdown** | "I can share it as a PDF through the browser's print dialog, or as Markdown for a wiki or a repository." |
| 2:30 (30 s) | Show GitHub: README, `docs/architecture` (ADRs), tests, CI badge | "I built it in ten days with free tools: ASP.NET Core, PostgreSQL and the Gemini API. The project documents its own decisions as ten ADRs, it has 29 unit tests and a CI build. Next I would add keyword search, a decisions dashboard and linking superseded decisions." |

## Likely questions

- **Is my pasted text stored?** No. Only the record you save is stored. The text goes to a free AI provider, so use fictional or sanitized text.
- **What if the AI gets the tags wrong?** That is the design: the architect confirms the tags, and the confidence note tells you when to look twice.
- **Why no login?** A deliberate scope decision for v1.0 (ADR-0006). The guardrails (input cap, cooldown, daily limit) protect the free AI quota.
- **Why Razor Pages and not a front-end framework?** Simple, fast to build and accessible by default (ADR-0003).
- **What happens when the free tier changes?** The AI sits behind an interface, so the provider can be swapped without touching the pages.
