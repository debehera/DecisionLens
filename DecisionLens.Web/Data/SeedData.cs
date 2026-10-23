using DecisionLens.Web.Models;

namespace DecisionLens.Web.Data;

public static class SeedData
{
    public static void EnsureSeeded(AppDbContext db)
    {
        if (db.DecisionRecords.Any())
        {
            return;
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        db.DecisionRecords.AddRange(
            new DecisionRecord
            {
                Title = "Use PostgreSQL for the Orders service",
                Decision = "The Orders service will store its data in a managed PostgreSQL database. The team will use the same database engine for the other new Harbor Retail services where it fits.",
                Context = "Harbor Retail is rebuilding its order handling. The current system uses a database that is costly to license and hard to scale during seasonal peaks. The team needs strong transactions for payments and stock updates.",
                OptionsConsidered = "Keep the current licensed database\nPostgreSQL (managed service)\nA document database",
                Rationale = "PostgreSQL gives reliable transactions and has no license cost. The team already knows SQL. A managed service removes most routine administration work.",
                Consequences = "Orders data must be migrated from the old system. The team needs backup and restore practice for the managed service. Reporting queries must be reviewed, because some use features of the old database.",
                AdmPhase = "C - Information Systems Architectures",
                Domain = "Data",
                Owner = "Anita Rao",
                Status = "Accepted",
                DecisionDate = today.AddDays(-21),
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            },
            new DecisionRecord
            {
                Title = "Expose partner integrations through an API gateway",
                Decision = "All partner and supplier integrations will go through a single API gateway. Partners will no longer call internal services directly.",
                Context = "Harbor Retail has about a dozen partners that call internal services in different ways. Security reviews are slow, and a change to one service can break a partner without warning.",
                OptionsConsidered = "Keep direct partner access to each service\nA managed API gateway\nA gateway the team builds and runs itself",
                Rationale = "A gateway gives one place for authentication, rate limits and logging. A managed product is faster to adopt than building one, and it frees the team to work on business features.",
                Consequences = "The gateway becomes a critical dependency and needs monitoring. Partners must move to the new endpoints over several months. Request pricing needs to be reviewed each quarter.",
                AdmPhase = "D - Technology Architecture",
                Domain = "Technology",
                Owner = "Marcus Lee",
                Status = "Accepted",
                DecisionDate = today.AddDays(-14),
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            },
            new DecisionRecord
            {
                Title = "Migrate reporting to the cloud in two phases",
                Decision = "Reporting will move to a cloud data platform in two phases. Phase one moves the standard daily reports. Phase two moves the ad hoc analysis tools.",
                Context = "The on-premises reporting server is near the end of its life and runs slowly at month end. Finance and sales both depend on the reports, so a single large cutover is seen as too risky.",
                OptionsConsidered = "Replace the on-premises server with new hardware\nMove everything to the cloud at once\nMove to the cloud in two phases",
                Rationale = "Two phases limit the risk for finance and sales. The first phase shows real costs and performance before the larger second phase begins.",
                Consequences = "Two reporting systems will run side by side for a few months. Staff need short training for the new tools. A rollback plan is needed for each phase.",
                AdmPhase = "F - Migration Planning",
                Domain = "Technology",
                Owner = "Priya Nair",
                Status = "Proposed",
                DecisionDate = today.AddDays(-9),
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            },
            new DecisionRecord
            {
                Title = "Retire the legacy billing module by Q3",
                Decision = "The legacy billing module will be retired by the end of Q3. New invoices will come from the new billing service from the start of Q3.",
                Context = "The legacy billing module is written in an old language and only two people can maintain it. Tax rule changes now take weeks to deliver. The new billing service already handles most invoice types.",
                OptionsConsidered = "Keep maintaining the legacy module\nRetire it by Q3\nRetire it at the end of the year",
                Rationale = "Retiring in Q3 removes the dependence on two people and speeds up tax rule changes. Waiting until year end would keep the risk through another busy season.",
                Consequences = "Old invoices must stay readable in an archive. The remaining invoice types must be built in the new service before Q3. Customer support needs updated guides.",
                AdmPhase = "H - Architecture Change Management",
                Domain = "Application",
                Owner = "Anita Rao",
                Status = "Accepted",
                DecisionDate = today.AddDays(-4),
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            });

        db.SaveChanges();
    }
}