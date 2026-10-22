namespace DecisionLens.Web.Services;

public record Sample(int Number, string Title, string Text);

public static class SampleDiscussions
{
    public static readonly IReadOnlyList<Sample> All = new[]
    {
        new Sample(1, "Chat thread: database choice", """
#orders-platform
Priya: We need to pick the database for the new Orders service before Friday's design review. Options on the table: PostgreSQL or MongoDB.
Marcus: Mongo would be quicker to start. Our order payloads change a lot and I like the flexible schema.
Lena: Finance needs reporting joins across orders, payments and refunds. In Mongo that gets ugly. Postgres does it natively and we already run it for Inventory.
Marcus: True, but schema changes in Postgres slow us down.
Priya: We can put the volatile parts of the payload in JSONB columns.
Dev: Ops also prefers fewer database technologies to patch and monitor. We already have Postgres runbooks.
Marcus: Okay, with JSONB for the flexible bits I'm fine with Postgres.
Priya: Decision then: the Orders service goes on PostgreSQL, using JSONB for variable payload fields. Lena owns the schema. Risk: JSONB queries need indexing discipline, so we add that to the design review checklist.
"""),
        new Sample(2, "Meeting notes: API gateway", """
Meeting notes: Partner Integration Review (Tuesday)
Attendees: Anika (Enterprise Architect), Tom (Security), Rui (Platform), Joanne (Partner Onboarding)

Today each partner connects directly to individual services, each with its own authentication and rate limiting. Two partner outages last quarter were caused by one partner flooding a single service.

Options discussed:
1. Keep direct connections and add rate limiting inside each service. Cheap, but inconsistent and every team has to build it.
2. Put a managed API gateway in front of all partner traffic. Central authentication, throttling and logging; adds one component and a monthly cost.
3. Build our own gateway. Full control, but Rui estimated about four months of effort.

Tom: Central logging and token validation would close two open audit findings.
Anika: Go with option 2 and reuse the cloud provider's gateway service rather than building anything.

Agreed: adopt a managed API gateway for all partner-facing APIs. Rui runs a two-week pilot with the Orders API; rollout to other APIs depends on the pilot. Risk: the gateway becomes a single point of failure, so Rui designs a multi-zone deployment.
"""),
        new Sample(3, "Email thread: legacy billing", """
Subject: RE: RE: Legacy billing - what do we do about it?

From: Chen (Finance Systems)
The legacy billing module is 14 years old and only two people understand it. Support costs keep rising and it blocks the new subscription pricing.

From: Fatima (Solution Architect)
Options: (a) leave it alone, (b) big-bang replacement over a long weekend, (c) migrate customer segments to the new billing service in waves while the old module runs in parallel. Big-bang is risky because month-end close cannot be re-run. Waves take longer but let us validate invoices segment by segment.

From: Chen
I can't accept a long weekend of risk around month-end. Waves it is, but I need a retirement date or the old module will live forever.

From: Fatima
Agreed. Decision: migrate in three waves (internal accounts, small business, enterprise) starting in August, run both systems in parallel for one billing cycle per wave, and switch off the legacy module by the end of Q3. Chen's team signs off each wave. Risk: parallel running doubles reconciliation work for finance.
""")
    };

    // Test only: a thin chat with no rationale, used to check that the AI does not invent facts.
    public static readonly Sample ThinChatTest = new(4, "Test only: thin chat, no rationale", """
Sam: Are we putting the new reporting jobs on the shared scheduler or their own?
Ira: Shared scheduler.
Sam: Ok, I'll tell the team.
Ira: Yes. Ravi will update the runbook.
""");

    public static Sample? Get(int number) =>
        number == 4 ? ThinChatTest : All.FirstOrDefault(s => s.Number == number);
}