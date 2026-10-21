# ADR-0005: Gemini free tier behind a replaceable interface

| Field | Value |
| --- | --- |
| Status | Accepted |
| Date | 2026-10-20 |
| Owner | Product owner |
| ADM phase | C. Information Systems Architectures |
| Architecture domain | Application |

## Context

The core feature needs an AI model that reads a messy discussion and returns a structured record. The budget is zero, so only free tiers qualify. Research on Day 1 found that:

- Google AI Studio offers a free key without payment details, with rate limits shown in the AI Studio console and a note that free-tier requests may be used to improve Google products.
- Anthropic publishes no free API tier; new accounts get a small, unpublished amount of free credits.
- Other free options exist, with their own limits and sign-up requirements, for example Groq and OpenRouter.

Free-tier terms change often.

## Decision

Call the **Google Gemini API** with a free Google AI Studio key over REST, using `HttpClient`, asking for a JSON response with a response schema. Put the provider behind an interface (`IDecisionDrafter`) so it can be replaced without touching the pages. The model name, timeout and limits live in configuration. On Day 3 choose a stable Flash or Flash-Lite model that AI Studio lists as free, not a preview model. If Gemini is unusable, build a fallback for any free OpenAI-compatible endpoint behind the same interface.

The server never trusts the reply. It strips code fences, parses the JSON, checks every field, matches the ADM phase and domain against the allowed lists, trims values, retries once on failure, and returns a friendly error code if the reply is still invalid.

## Options considered

- **Gemini free tier over REST (chosen).** Pros: free key without payment details; supports structured JSON output; plain HTTP matches the owner's C# skills. Cons: rate limits; submitted text may be used by the provider; terms can change.
- **Groq or OpenRouter free tiers.** Pros: free, OpenAI-compatible. Cons: sign-up requirements such as phone verification or low daily caps; kept as the fallback.
- **Claude API.** Pros: strong output quality. Cons: no ongoing free tier; conflicts with the no-paid-tools rule.
- **No AI, rules or templates only.** Pros: no external dependency. Cons: cannot extract a decision from messy text, which is the whole point.

## Rationale

Gemini is the lowest-friction free option that supports structured output. The interface and the server-side validation make the choice reversible and the output safe.

## Consequences and risks

- Free limits can block drafting. A manual "Start blank" path always works.
- Only fictional or sanitized text may be pasted, because free tiers may use inputs.
- The API key is a secret. It is stored only in user secrets or environment variables, and the app never logs the key or the pasted text.
- Limits and terms are re-checked on the day the key is created.
