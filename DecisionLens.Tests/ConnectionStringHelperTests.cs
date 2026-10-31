using DecisionLens.Web.Services;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Xunit;

namespace DecisionLens.Tests;

public class ConnectionStringHelperTests
{
    [Fact]
    public void PostgresUrl_IsConverted()
    {
        var result = ConnectionStringHelper.Convert("postgres://appuser:secret@db.example.com:5433/appdb");

        var builder = new NpgsqlConnectionStringBuilder(result);
        Assert.Equal("db.example.com", builder.Host);
        Assert.Equal(5433, builder.Port);
        Assert.Equal("appdb", builder.Database);
        Assert.Equal("appuser", builder.Username);
        Assert.Equal("secret", builder.Password);
        Assert.Equal(SslMode.Require, builder.SslMode);
    }

    [Fact]
    public void PostgresqlScheme_IsAlsoAccepted()
    {
        var result = ConnectionStringHelper.Convert("postgresql://appuser:secret@db.example.com/appdb");

        var builder = new NpgsqlConnectionStringBuilder(result);
        Assert.Equal("db.example.com", builder.Host);
        Assert.Equal("appdb", builder.Database);
    }

    [Fact]
    public void PasswordWithSpecialCharacters_IsDecoded()
    {
        var result = ConnectionStringHelper.Convert("postgres://appuser:p%40ss%23w%2Frd%3F@db.example.com/appdb");

        var builder = new NpgsqlConnectionStringBuilder(result);
        Assert.Equal("p@ss#w/rd?", builder.Password);
    }

    [Fact]
    public void MissingPort_DefaultsTo5432()
    {
        var result = ConnectionStringHelper.Convert("postgres://appuser:secret@db.example.com/appdb");

        var builder = new NpgsqlConnectionStringBuilder(result);
        Assert.Equal(5432, builder.Port);
    }

    [Fact]
    public void StandardKeyValueString_PassesThroughUnchanged()
    {
        var standard = "Host=localhost;Port=5432;Database=x;Username=u;Password=p";

        Assert.Equal(standard, ConnectionStringHelper.Convert(standard));
    }

    [Fact]
    public void Resolve_ReadsTheDefaultConnectionString()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = "postgres://appuser:secret@db.example.com/appdb"
            })
            .Build();

        var result = ConnectionStringHelper.Resolve(config);

        Assert.Equal("db.example.com", new NpgsqlConnectionStringBuilder(result).Host);
    }

    [Fact]
    public void Resolve_ThrowsWhenNotConfigured()
    {
        var config = new ConfigurationBuilder().Build();

        Assert.Throws<InvalidOperationException>(() => ConnectionStringHelper.Resolve(config));
    }
}