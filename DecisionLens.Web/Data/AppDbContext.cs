using DecisionLens.Web.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DecisionLens.Web.Data;

public class AppDbContext : DbContext, IDataProtectionKeyContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<DecisionRecord> DecisionRecords => Set<DecisionRecord>();

    public DbSet<AiUsageDay> AiUsageDays => Set<AiUsageDay>();

    // Secret keys that protect forms and cookies, stored so they survive restarts.
    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AiUsageDay>().HasKey(x => x.Day);
    }
}