# DecisionLens v1.0: Known issues and limits

These are deliberate limits or minor issues found in testing. None blocks the core flow.

## Hosting and data

| # | Item | Severity | What to do |
| --- | --- | --- | --- |
| 1 | The free Render web service sleeps after about 15 minutes without visits. The next request can take 50 seconds or more. | 3 | Open the site five minutes before a demo and run one sample. |
| 2 | The free Render Postgres database expires 30 days after creation (created Oct 23, 2026, so around Nov 22, 2026), with a 14-day grace period. | 2 | Before it expires, create a new free database, update `ConnectionStrings__Default` on Render and redeploy. The tables and seed records are rebuilt on start. Export anything worth keeping first. |
| 3 | Render's Auto-Deploy did not start a deploy after pushes, although it is set to On Commit. | 3 | After each push, deploy by hand with Manual Deploy, then Deploy latest commit. |

## AI behaviour

| # | Item | Severity | What to do |
| --- | --- | --- | --- |
| 4 | The AI's confidence rating is only a rough signal. A thin discussion can still be rated medium. | 3 | The architect always confirms the ADM phase and domain. |
| 5 | The message for a missing AI model name or key ("not configured") was not tested directly. A blank model name produced the general "the AI service had a problem" message. | 3 | Both messages are friendly and show no technical details. |
| 6 | The free Gemini service has its own limits, and model names can change. | 2 | Check the model name in Google AI Studio if drafting starts failing, then update `Ai__Model` on Render. |

## Scope (not built in v1.0)

- No login. Everyone with the link shares one list of records, so only fictional or sanitized text should be used.
- Pasted text is sent to a free AI provider and is never stored by DecisionLens.
- No record linking or "superseded by" relationships, keyword search, version history, notifications or integrations.
- The daily AI limit (40 drafts per UTC day) is shared by all visitors. The per-visitor cooldown (10 seconds) is held in memory and resets when the server restarts.
- The PDF layout comes from the browser's print dialog, so it can differ between browsers.