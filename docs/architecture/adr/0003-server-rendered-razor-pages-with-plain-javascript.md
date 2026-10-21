# ADR-0003: Server-rendered Razor Pages with plain JavaScript

| Field | Value |
| --- | --- |
| Status | Accepted |
| Date | 2026-10-20 |
| Owner | Product owner |
| ADM phase | C. Information Systems Architectures |
| Architecture domain | Application |

## Context

The product owner is strong in C#, SQL, HTML and CSS, and new to JavaScript. The build has 13 hours in total, mostly in one-hour sessions. The screens are forms, lists and detail pages, with one interactive moment: drafting a record while showing a loading indicator.

## Decision

Build the app with **ASP.NET Core Razor Pages** (server-rendered) on the .NET SDK already installed, styled with the Bootstrap that ships with the template plus two small CSS files. Use **plain JavaScript** in `wwwroot/js/` for a few behaviors only: the draft call and spinner, the character counter, the sample loader and confirmation dialogs. There is no npm, bundler or front-end framework.

## Options considered

- **Razor Pages with plain JavaScript (chosen).** Pros: fits existing C# and HTML skills; one project; simple deployment. Cons: less smooth than a single-page app.
- **ASP.NET Core MVC.** Pros: familiar pattern. Cons: more files and ceremony for the same screens.
- **Blazor.** Pros: C# on the client. Cons: new concepts and a heavier runtime; more to learn.
- **JavaScript framework (React, Vue) with a C# API.** Pros: smoother UI. Cons: two projects, a build chain and a language the owner is new to; too costly for 13 hours.

## Rationale

The chosen approach keeps new learning close to zero and keeps debugging in one language. The only dynamic behavior needed is small enough for plain JavaScript.

## Consequences and risks

- Most actions reload the page. The AI draft call is the one exception.
- Plain JavaScript must stay limited. Any request for a framework is a new decision and needs a new ADR.
- Anti-forgery protection stays on; the JavaScript sends the token in a request header.
