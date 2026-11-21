using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// One open connection keeps the in-memory database alive for all contexts
using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();

using (var seed = new DemoContext(connection))
{
    seed.Database.EnsureCreated();
    var host = new LogSource { Name = "auth-01" };
    seed.Incidents.AddRange(
        new Incident
        {
            User = "bob",
            Events =
            [
                new Event { EventType = "login-failed", Source = host },
                new Event { EventType = "login-failed", Source = host },
            ],
        },
        new Incident { User = "dave" });
    seed.SaveChanges();
}

// 1. No Include: the navigations stay empty
using (var db = new DemoContext(connection))
{
    Incident bob = db.Incidents.Single(i => i.User == "bob");
    Console.WriteLine("1. No Include");
    Console.WriteLine($"Events of bob: {bob.Events.Count}");
}

// 2. Include loads the collection navigation
using (var db = new DemoContext(connection))
{
    var query = db.Incidents.Include(i => i.Events);
    Console.WriteLine("2. Include");
    Console.WriteLine(query.ToQueryString());
    Incident bob = query.Single(i => i.User == "bob");
    Console.WriteLine($"Events of bob: {bob.Events.Count}");
    Console.WriteLine($"Source of an event: {bob.Events[0].Source?.Name ?? "null"}");
}

// 3. ThenInclude goes one level deeper
using (var db = new DemoContext(connection))
{
    Incident bob = db.Incidents
        .Include(i => i.Events)
        .ThenInclude(e => e.Source)
        .Single(i => i.User == "bob");
    Console.WriteLine("3. ThenInclude");
    Console.WriteLine($"Source of an event: {bob.Events[0].Source?.Name ?? "null"}");
}

class Incident
{
    public int Id { get; set; }
    public string User { get; set; } = "";
    public List<Event> Events { get; set; } = [];
}

class Event
{
    public int Id { get; set; }
    public string EventType { get; set; } = "";
    public int IncidentId { get; set; }
    public Incident? Incident { get; set; }
    public int SourceId { get; set; }
    public LogSource? Source { get; set; }
}

class LogSource
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

class DemoContext(SqliteConnection connection) : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite(connection);
}
