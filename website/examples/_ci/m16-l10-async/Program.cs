using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();
await using var db = new DemoContext(connection);
await db.Database.EnsureCreatedAsync();

using var cts = new CancellationTokenSource();
CancellationToken token = cts.Token;

// 1. SaveChangesAsync: the database call is I/O, so await it
db.Incidents.AddRange(
    new Incident { User = "bob", FailureCount = 5 },
    new Incident { User = "dave", FailureCount = 1 },
    new Incident { User = "alice", FailureCount = 1 });
int written = await db.SaveChangesAsync(token);
Console.WriteLine($"Rows written: {written}");

// 2. Query operators that run the query have an async version.
//    Where and OrderBy only build the query: they have none.
List<Incident> severe = await db.Incidents
    .Where(i => i.FailureCount >= 2)
    .OrderBy(i => i.User)
    .ToListAsync(token);
Console.WriteLine($"Severe: {severe.Count}");
Console.WriteLine($"Any for alice: {await db.Incidents.AnyAsync(i => i.User == "alice", token)}");
Console.WriteLine($"Count: {await db.Incidents.CountAsync(token)}");
Incident? first = await db.Incidents
    .OrderBy(i => i.Id)
    .FirstOrDefaultAsync(token);
Console.WriteLine($"First: {first?.User}");

// 3. await foreach reads the rows one after the other
await foreach (Incident row in db.Incidents.OrderBy(i => i.Id).AsAsyncEnumerable())
{
    Console.WriteLine($"Row: {row.User}");
}

// 4. A cancelled token stops the call before it reaches the database
cts.Cancel();
try
{
    await db.Incidents.ToListAsync(token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Query cancelled");
}

class Incident
{
    public int Id { get; set; }
    public string User { get; set; } = "";
    public int FailureCount { get; set; }
}

class DemoContext(SqliteConnection connection) : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite(connection);
}
