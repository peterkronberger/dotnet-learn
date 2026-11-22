using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();
using var db = new DemoContext(connection);
db.Database.EnsureCreated();
db.Incidents.AddRange(
    new Incident { User = "bob", FailureCount = 5 },
    new Incident { User = "dave", FailureCount = 1 },
    new Incident { User = "alice", FailureCount = 1 });
db.SaveChanges();

// A made-up input that changes the meaning of the query:
// the quote ends the text, the rest is always true
string input = "x' OR '1'='1";

// Unsafe: the input is part of the SQL text
var unsafeQuery = db.Incidents.FromSqlRaw(
    "SELECT * FROM Incidents WHERE User = '" + input + "'");
Console.WriteLine("Concatenated, FromSqlRaw:");
Console.WriteLine(unsafeQuery.ToQueryString());
Console.WriteLine($"Rows: {unsafeQuery.Count()}");
Console.WriteLine();

// Unsafe too: the interpolation is finished before FromSqlRaw is called
var rawInterpolated = db.Incidents.FromSqlRaw(
    $"SELECT * FROM Incidents WHERE User = '{input}'");
Console.WriteLine($"Interpolated, FromSqlRaw rows: {rawInterpolated.Count()}");
Console.WriteLine();

// Safe: FromSql sends the input as a parameter
var safeQuery = db.Incidents.FromSql(
    $"SELECT * FROM Incidents WHERE User = {input}");
Console.WriteLine("Interpolated, FromSql:");
Console.WriteLine(safeQuery.ToQueryString());
Console.WriteLine($"Rows: {safeQuery.Count()}");
Console.WriteLine();

// Safe: FromSqlRaw with an explicit parameter
var explicitQuery = db.Incidents.FromSqlRaw(
    "SELECT * FROM Incidents WHERE User = {0}", input);
Console.WriteLine($"Explicit parameter, FromSqlRaw rows: {explicitQuery.Count()}");

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
