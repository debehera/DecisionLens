# ADR-0006: No login in v1.0, with usage guardrails

| Field | Value |
| --- | --- |
| Status | Accepted |
| Date | 2026-10-20 |
| Owner | Product owner |
| ADM phase | C. Information Systems Architectures |
| Architecture domain | Application |

## Context

Anyone with the live link must be able to try the product. Accounts would need registration, password handling and security testing, which could use most of the 13 build hours without improving the core idea. At the same time, an open link lets strangers use up the free AI quota.

## Decision

v1.0 has **no login**. Everyone with the link uses the app and sees the same shared list. Only fictional or sanitized data is allowed. To protect the free AI quota, add guardrails:

- an input cap of 6,000 characters, enforced in the browser and on the server;
- a global daily limit on AI drafts (default 40, counted per UTC day in the database);
- a short per-visitor cooldown (default 10 seconds, kept in memory);
- friendly messages when a limit is reached, with the manual "Start blank" path still available.

## Options considered

- **No login with guardrails (chosen).** Pros: fastest; anyone can try it; guardrails address quota abuse. Cons: anyone can read, edit and delete shared records.
- **One shared password.** Pros: gives a "governance" feel for little effort. Cons: one more screen and secret to manage; strangers cannot try the app without being given the password.
- **Full user accounts.** Pros: per-user records and real access control. Cons: most of the build budget; not needed for fictional data.

## Rationale

The goal for Day 10 is that a stranger can complete the workflow without help. No login supports that directly, and the guardrails handle the one real risk, which is quota exhaustion.

## Consequences and risks

- Any visitor can delete or change records, so the list can be altered by others. Seed data is recreated on an empty database.
- The product must not be used for real company decisions. A visible banner and the README state this.
- The daily limit is global, so heavy use can stop AI drafting for everyone until the next UTC day.
- Accounts and access control are future scope and need a new ADR.
