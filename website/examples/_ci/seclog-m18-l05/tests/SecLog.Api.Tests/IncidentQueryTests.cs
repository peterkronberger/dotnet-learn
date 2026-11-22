using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecLog.Data;
using Xunit;

namespace SecLog.Api.Tests;

public class IncidentQueryTests(SecLogApiFactory factory)
    : IClassFixture<SecLogApiFactory>
{
    [Fact]
    public void Search_WithUser_FiltersInSqlWithParameter()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SecLogDbContext>();

        var sql = IncidentFilters.Search(db.Incidents, "bob").ToQueryString();

        // ToQueryString starts with the parameter values; the SQL text
        // follows after them
        var sqlText = sql[sql.IndexOf("SELECT", StringComparison.Ordinal)..];

        // The database filters, and the name is a parameter, not SQL text
        Assert.Contains("WHERE", sqlText);
        Assert.Contains("@user", sqlText);
        Assert.DoesNotContain("bob", sqlText);
    }

    [Fact]
    public void Search_WithoutUser_HasNoWhere()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SecLogDbContext>();

        var sql = IncidentFilters.Search(db.Incidents, null).ToQueryString();

        Assert.DoesNotContain("WHERE", sql);
    }
}
