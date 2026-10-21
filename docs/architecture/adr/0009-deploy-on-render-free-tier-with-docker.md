# ADR-0009: Deploy on Render's free tier with Docker

| Field | Value |
| --- | --- |
| Status | Accepted |
| Date | 2026-10-20 |
| Owner | Product owner |
| ADM phase | D. Technology Architecture |
| Architecture domain | Technology |

## Context

The product must be live on a public link by Thursday, October 29, 2026, at zero cost. The product owner has used Render before. A .NET app needs a container to run there. Render can deploy from a Git repository, and its free web service sleeps after a period of inactivity.

## Decision

Host the app as a **Render free Web Service built from a Dockerfile**, deployed from the public GitHub repository (`main` branch), in the same region as the database. The app listens on the port Render provides and trusts the proxy's forwarded headers so the visitor address and HTTPS scheme are correct. Settings and secrets are Render environment variables. A health endpoint supports the host. The first live deployment is on Day 7 (Mon Oct 26), which leaves three days for polish, testing and release.

## Options considered

- **Render free Web Service with Docker (chosen).** Pros: previously used by the owner; deploys from Git; free; works with the free PostgreSQL in the same place. Cons: sleeps when idle; a cold start takes about a minute.
- **Static hosts such as Vercel or Netlify.** Pros: simple for front ends. Cons: not designed to run a C# server directly.
- **Cloud platform accounts (Azure, AWS, Google Cloud).** Pros: powerful. Cons: more setup, and sign-up can require payment details.
- **Company server.** Pros: none for a public demo. Cons: not publicly reachable and not appropriate.

## Rationale

Render gives the simplest path from a repository to a public URL for a C# app at zero cost, and the owner already knows it.

## Consequences and risks

- The first visit after inactivity is slow. The demo plan includes a warm-up step.
- The free instance has monthly limits, which are checked on the deployment day.
- The Docker image tag must use the same major .NET version as the local SDK, recorded on Day 2.
- Free-tier terms can change, so they are re-verified on Day 7.
- Repository contents must never include secrets.
