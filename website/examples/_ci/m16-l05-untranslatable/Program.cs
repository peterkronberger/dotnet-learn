using Microsoft.EntityFrameworkCore;

using var db = new DemoContext();
db.Database.OpenConnection();
db.Database.EnsureCreated();
db.Incidents.AddRange(
    new Incident { User = "bob", FailureCount = 5 },
    new Incident { User = "alice", FailureCount = 1 },
    new Incident { User = "dave", FailureCount = 1 });
db.SaveChanges();

// 1. An own method in Where: EF Core cannot translate it
Console.WriteLine("1. Where with own method");
try
{
    var broken = db.Incidents.Where(i => Rules.IsSevere(i)).ToList();
    Console.WriteLine($"Rows: {broken.Count}");
}
catch (InvalidOperationException ex)
{
    string text = ex.Message;
    Console.WriteLine($"Exception: {ex.GetType().Name}");
    Console.WriteLine($"Not translated: {text.Contains("could not be translated")}");
}

// 2. Fix: write the condition in the lambda, EF Core translates it
Console.WriteLine("2. Condition in the lambda");
var query = db.Incidents.Where(i => i.FailureCount >= 5);
Console.WriteLine(query.ToQueryString());
Console.WriteLine($"Rows: {query.Count()}");

// 3. Own method in the last Select: it runs in the program
Console.WriteLine("3. Own method in the last Select");
var labeled = db.Incidents
    .OrderBy(i => i.Id)
    .Select(i => new { i.User, Label = Rules.Describe(i.FailureCount) });
Console.WriteLine(labeled.ToQueryString());
foreach (var row in labeled)
{
    Console.WriteLine($"{row.User}: {row.Label}");
}

class Incident
{
    public int Id { get; set; }
    public string User { get; set; } = "";
    public int FailureCount { get; set; }
}

static class Rules
{
    public static bool IsSevere(Incident incident) =>
        incident.FailureCount >= 5;

    public static string Describe(int failures) =>
        failures >= 5 ? "severe" : "minor";
}

class DemoContext : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite("Data Source=:memory:");
}
