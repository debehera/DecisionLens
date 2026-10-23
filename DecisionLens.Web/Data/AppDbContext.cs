using DecisionLens.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace DecisionLens.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<DecisionRecord> DecisionRecords => Set<DecisionRecord>();

    public DbSet<AiUsageDay> AiUsageDays => Set<AiUsageDay>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AiUsageDay>().HasKey(x => x.Day);
    }
}