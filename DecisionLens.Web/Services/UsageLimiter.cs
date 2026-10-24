using System.Collections.Concurrent;
using DecisionLens.Web.Data;
using DecisionLens.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DecisionLens.Web.Services;

public record LimitResult(bool Allowed, DraftErrorCode Code, int DraftsLeft);

/// <summary>
/// Remembers when each visitor last drafted. Registered as a singleton, kept in memory only.
/// </summary>
public class CooldownTracker
{
    private readonly ConcurrentDictionary<string, DateTime> _lastAttempt = new();

    public bool IsCoolingDown(string key, TimeSpan cooldown)
    {
        return _lastAttempt.TryGetValue(key, out var last) && DateTime.UtcNow - last < cooldown;
    }

    public void Mark(string key, TimeSpan cooldown)
    {
        var now = DateTime.UtcNow;
        _lastAttempt[key] = now;

        // Keep the dictionary small: drop entries that have expired.
        if (_lastAttempt.Count > 1000)
        {
            foreach (var pair in _lastAttempt)
            {
                if (now - pair.Value >= cooldown)
                {
                    _lastAttempt.TryRemove(pair.Key, out _);
                }
            }
        }
    }
}

/// <summary>
/// Applies the two AI guardrails: a per-visitor cooldown and a global daily limit.
/// </summary>
public class UsageLimiter
{
    private readonly AppDbContext _db;
    private readonly CooldownTracker _tracker;
    private readonly LimitsOptions _limits;

    public UsageLimiter(AppDbContext db, CooldownTracker tracker, IOptions<LimitsOptions> limits)
    {
        _db = db;
        _tracker = tracker;
        _limits = limits.Value;
    }

    private static DateOnly TodayUtc() => DateOnly.FromDateTime(DateTime.UtcNow);

    public async Task<int> GetRemainingAsync(CancellationToken cancellationToken = default)
    {
        var today = TodayUtc();
        var used = await _db.AiUsageDays
            .AsNoTracking()
            .Where(x => x.Day == today)
            .Select(x => x.Count)
            .FirstOrDefaultAsync(cancellationToken);

        return Math.Max(0, _limits.DailyDraftLimit - used);
    }

    /// <summary>
    /// Call this once before every AI draft. If it returns Allowed, one draft has been counted.
    /// </summary>
    public async Task<LimitResult> TryConsumeAsync(string visitorKey, CancellationToken cancellationToken = default)
    {
        var key = string.IsNullOrWhiteSpace(visitorKey) ? "unknown" : visitorKey;
        var cooldown = TimeSpan.FromSeconds(_limits.CooldownSeconds);

        if (_tracker.IsCoolingDown(key, cooldown))
        {
            return new LimitResult(false, DraftErrorCode.Cooldown, await GetRemainingAsync(cancellationToken));
        }

        // Two attempts, in case two visitors create today's row at the same moment.
        for (var attempt = 0; attempt < 2; attempt++)
        {
            var today = TodayUtc();
            var row = await _db.AiUsageDays.FirstOrDefaultAsync(x => x.Day == today, cancellationToken);

            if (row == null)
            {
                row = new AiUsageDay { Day = today, Count = 0 };
                _db.AiUsageDays.Add(row);
            }

            if (row.Count >= _limits.DailyDraftLimit)
            {
                _db.Entry(row).State = row.Count == 0 ? EntityState.Detached : EntityState.Unchanged;
                return new LimitResult(false, DraftErrorCode.DailyLimit, 0);
            }

            row.Count++;

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException) when (attempt == 0)
            {
                _db.ChangeTracker.Clear();
                continue;
            }

            _tracker.Mark(key, cooldown);
            return new LimitResult(true, DraftErrorCode.None, Math.Max(0, _limits.DailyDraftLimit - row.Count));
        }

        return new LimitResult(false, DraftErrorCode.ProviderError, 0);
    }
}