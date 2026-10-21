# ADR-0004: PostgreSQL with Entity Framework Core

| Field | Value |
| --- | --- |
| Status | Accepted |
| Date | 2026-10-20 |
| Owner | Product owner |
| ADM phase | C. Information Systems Architectures |
| Architecture domain | Data |

## Context

The records must survive restarts and redeploys, so a database file on a free host's disk is not safe. The budget is zero. Render offers a free PostgreSQL database and a free web service. The product owner knows SQL well.

## Decision

Use **PostgreSQL on Render's free tier** as the single database for both local development and production. Access it with **Entity Framework Core** and the Npgsql provider. Migrations are applied automatically at startup. The connection string comes from configuration and accepts either a Render `postgres://` URL or a standard Npgsql string. Only fictional data is stored.

## Options considered

- **PostgreSQL on Render for development and production (chosen).** Pros: one database engine everywhere; no differences between environments; free. Cons: local work depends on the free database being reachable; the free database has an expiry period.
- **SQLite locally and PostgreSQL in production.** Pros: works offline. Cons: two providers, two behaviors, and bugs that appear only in production.
- **SQLite file on the host.** Pros: simplest. Cons: the free host's disk is not durable, so records would be lost.
- **Another free hosted database.** Pros: may avoid the expiry. Cons: another account and another set of terms to verify; not needed for a 10-day capstone.

## Rationale

One engine in every environment removes a class of surprises, and Render keeps hosting and data in one place and in one region at no cost.

## Consequences and risks

- The free database expires after a fixed period. It is created on Day 4 and must outlast Thursday, October 29, 2026. If it must be rebuilt, the migrations recreate the schema and seed data.
- The database must be reachable from the developer's machine; Day 4 verifies this.
- Secrets live in `dotnet user-secrets` locally and in environment variables on Render, never in the repository.
