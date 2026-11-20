using Microsoft.EntityFrameworkCore;

// Print the schema: two collection navigations, no join class
using var db = new DemoContext();
Console.WriteLine(db.Database.GenerateCreateScript());

// Store one analyst for two incidents and read the links back
db.Database.OpenConnection();
db.Database.EnsureCreated();
var alice = new Analyst { Name = "alice" };
db.Incidents.Add(new Incident { User = "bob", Analysts = [alice] });
db.Incidents.Add(new Incident { User = "dave", Analysts = [alice] });
db.SaveChanges();

var counts = db.Analysts
    .Select(a => new { a.Name, Count = a.Incidents.Count })
    .ToList();
foreach (var row in counts)
{
    Console.WriteLine($"{row.Name}: {row.Count}");
}

class Incident
{
    public int Id { get; set; }
    public string User { get; set; } = "";
    public List<Analyst> Analysts { get; set; } = [];
}

class Analyst
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Incident> Incidents { get; set; } = [];
}

class DemoContext : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<Analyst> Analysts => Set<Analyst>();

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite("Data Source=:memory:");
}
