using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();
using var db = new DemoContext(connection);
db.Database.EnsureCreated();
db.Incidents.AddRange(
    new Incident { User = "bob", FailureCount = 5, Status = "open" },
    new Incident { User = "dave", FailureCount = 1, Status = "open" },
    new Incident { User = "alice", FailureCount = 1, Status = "open" });
db.SaveChanges();

string user = "bob";

// 1. FromSql: each hole of the interpolated string becomes a parameter
var byUser = db.Incidents
    .FromSql($"SELECT * FROM Incidents WHERE User = {user}");
Console.WriteLine(byUser.ToQueryString());
Console.WriteLine($"Rows: {byUser.Count()}");
Console.WriteLine();

// 2. LINQ on top of FromSql: the raw SQL becomes a subquery
int atLeast = 1;
var composed = db.Incidents
    .FromSql($"SELECT * FROM Incidents WHERE FailureCount >= {atLeast}")
    .OrderBy(i => i.User)
    .Select(i => i.User);
Console.WriteLine(composed.ToQueryString());
Console.WriteLine(string.Join(", ", composed));
Console.WriteLine();

// 3. SqlQuery: a result that is not an entity (here one number)
int minimum = 2;
int count = db.Database
    .SqlQuery<int>($"SELECT COUNT(*) AS Value FROM Incidents WHERE FailureCount >= {minimum}")
    .Single();
Console.WriteLine($"Incidents with at least {minimum} failures: {count}");

// 4. ExecuteSql: a command, it returns the number of changed rows
string newStatus = "closed";
int changed = db.Database.ExecuteSql(
    $"UPDATE Incidents SET Status = {newStatus} WHERE User = {user}");
Console.WriteLine($"Rows changed: {changed}");

class Incident
{
    public int Id { get; set; }
    public string User { get; set; } = "";
    public int FailureCount { get; set; }
    public string Status { get; set; } = "";
}

class DemoContext(SqliteConnection connection) : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite(connection);
}
