using Microsoft.EntityFrameworkCore;

// Print the schema of a one-to-many relationship
using var db = new DemoContext();
Console.WriteLine(db.Database.GenerateCreateScript());

// Store two incidents with their events and count the events per incident
db.Database.OpenConnection();
db.Database.EnsureCreated();
db.Incidents.Add(new Incident
{
    User = "bob",
    Events =
    [
        new Event { EventType = "login-failed" },
        new Event { EventType = "login-failed" },
    ],
});
db.Incidents.Add(new Incident
{
    User = "dave",
    Events = [new Event { EventType = "login-failed" }],
});
db.SaveChanges();

var counts = db.Incidents
    .OrderBy(i => i.Id)
    .Select(i => new { i.User, Count = i.Events.Count })
    .ToList();
foreach (var row in counts)
{
    Console.WriteLine($"{row.User}: {row.Count}");
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
    public Incident Incident { get; set; } = null!;
}

class DemoContext : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<Event> Events => Set<Event>();

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite("Data Source=:memory:");
}
