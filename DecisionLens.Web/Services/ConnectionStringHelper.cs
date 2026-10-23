using Npgsql;

namespace DecisionLens.Web.Services;

public static class ConnectionStringHelper
{
    /// <summary>
    /// Reads ConnectionStrings:Default and returns an Npgsql connection string.
    /// Accepts a Render URL (postgres:// or postgresql://) or a standard key=value string.
    /// </summary>
    public static string Resolve(IConfiguration config)
    {
        var raw = config.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(raw))
        {
            throw new InvalidOperationException(
                "ConnectionStrings:Default is not set. Add it with dotnet user-secrets (local) or an environment variable (Render).");
        }

        return Convert(raw);
    }

    public static string Convert(string value)
    {
        value = value.Trim();

        var isUrl = value.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
                 || value.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase);
        if (!isUrl)
        {
            return value;
        }

        var uri = new Uri(value);

        var userInfo = uri.UserInfo;
        var colon = userInfo.IndexOf(':');
        var user = colon >= 0 ? userInfo[..colon] : userInfo;
        var password = colon >= 0 ? userInfo[(colon + 1)..] : "";

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Database = uri.AbsolutePath.TrimStart('/'),
            Username = Uri.UnescapeDataString(user),
            Password = Uri.UnescapeDataString(password),
            SslMode = SslMode.Require
            
        };

        return builder.ConnectionString;
    }
}