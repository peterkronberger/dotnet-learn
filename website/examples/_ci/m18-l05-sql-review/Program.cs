using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();
using var db = new DemoContext(connection);
string user = "bob";

// 1. Translated: filter, sort and projection run in the database
var translated = db.Incidents
    .Where(i => i.User == user)
    .OrderBy(i => i.Id)
    .Select(i => new { i.Id, i.FailureCount });
Console.WriteLine("1. Where, OrderBy, Select:");
Console.WriteLine(translated.ToQueryString());
Console.WriteLine();

// 2. FromSql: the interpolated value becomes a parameter
var safe = db.Incidents.FromSql(
    $"SELECT * FROM Incidents WHERE User = {user}");
Console.WriteLine("2. FromSql with interpolation:");
Console.WriteLine(safe.ToQueryString());
Console.WriteLine();

// 3. FromSqlRaw with concatenation: the value is part of the SQL text
var raw = db.Incidents.FromSqlRaw(
    "SELECT * FROM Incidents WHERE User = '" + user + "'");
Console.WriteLine("3. FromSqlRaw with concatenation:");
Console.WriteLine(raw.ToQueryString());
Console.WriteLine();

// 4. After AsEnumerable the filter runs in memory: the SQL has no WHERE.
// The part before AsEnumerable is the query that reaches the database.
var beforeAsEnumerable = db.Incidents.OrderBy(i => i.Id);
var inMemory = beforeAsEnumerable
    .AsEnumerable()
    .Where(i => i.User == user);
Console.WriteLine("4. Filter after AsEnumerable:");
Console.WriteLine(beforeAsEnumerable.ToQueryString());

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
