using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecLog.Data;
using SecLog.Data.Entities;

namespace SecLog.Api.Tests;

/// <summary>Starts the API in memory with a fresh, seeded test database.</summary>
public sealed class SecLogApiFactory : WebApplicationFactory<Program>
{
    private readonly string _directory =
        Directory.CreateTempSubdirectory("seclog-api-tests-").FullName;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // The setting beats appsettings.json of the API
        builder.UseSetting("ConnectionStrings:SecLog",
            $"Data Source={Path.Combine(_directory, "seclog.db")}");

        // The test scheme reads user and role from request headers
        builder.ConfigureTestServices(TestAuthHandler.Replace);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SecLogDbContext>();
        db.Database.Migrate();
        db.Incidents.AddRange(
            new StoredIncident
            {
                User = "bob", Source = "sample log",
                Status = "investigating", FailureCount = 5,
            },
            new StoredIncident
            {
                User = "dave", Source = "sample log",
                Status = "investigating", FailureCount = 1,
            });
        db.SaveChanges();

        return host;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            // Release the pooled connection, then delete the file
            SqliteConnection.ClearAllPools();
            Directory.Delete(_directory, recursive: true);
        }
    }
}
