using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

// A named in-memory database that every context reaches with its own
// connection. This open connection keeps the database alive.
const string connectionString = "Data Source=demo;Mode=Memory;Cache=Shared";
using var keepAlive = new SqliteConnection(connectionString);
keepAlive.Open();
var options = new DbContextOptionsBuilder<DemoContext>()
    .UseSqlite(connectionString)
    .AddInterceptors(new SlowDatabase())
    .Options;

using (var seed = new DemoContext(options))
{
    seed.Database.EnsureCreated();
    seed.Incidents.Add(new Incident { User = "bob" });
    seed.SaveChanges();
}

// 1. Two queries at the same time on ONE context
using (var db = new DemoContext(options))
{
    try
    {
        await Task.WhenAll(db.Incidents.CountAsync(), db.Incidents.AnyAsync());
        Console.WriteLine("1. Both queries finished");
    }
    catch (InvalidOperationException)
    {
        Console.WriteLine("1. InvalidOperationException: one context, two operations");
    }
}

// 2. Await each operation before the next one
using (var db = new DemoContext(options))
{
    int count = await db.Incidents.CountAsync();
    bool any = await db.Incidents.AnyAsync();
    Console.WriteLine($"2. One after the other: {count}, {any}");
}

// 3. Parallel work needs one context per operation
using (var first = new DemoContext(options))
using (var second = new DemoContext(options))
{
    int[] results = await Task.WhenAll(
        first.Incidents.CountAsync(),
        second.Incidents.CountAsync());
    Console.WriteLine($"3. Two contexts in parallel: {results[0]}, {results[1]}");
}

class Incident
{
    public int Id { get; set; }
    public string User { get; set; } = "";
}

class DemoContext(DbContextOptions<DemoContext> options) : DbContext(options)
{
    public DbSet<Incident> Incidents => Set<Incident>();
}

// @fold-start
// Makes every query take a moment, so two operations really overlap
class SlowDatabase : DbCommandInterceptor
{
    public override async ValueTask<InterceptionResult<DbDataReader>>
        ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        return result;
    }
}
// @fold-end
