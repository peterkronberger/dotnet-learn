using Microsoft.EntityFrameworkCore;

// Demo for spike S2: prints the SQL that EF Core generates for a LINQ query.
await using var db = new DemoContext();
db.Database.OpenConnection();
db.Database.EnsureCreated();
db.Incidents.AddRange(new Incident { Title = "Brute force", Severity = 3 }, new Incident { Title = "Odd login", Severity = 1 });
db.SaveChanges();

var query = db.Incidents.Where(i => i.Severity >= 2).OrderBy(i => i.Title).Select(i => i.Title);
Console.WriteLine(query.ToQueryString());
foreach (var title in query)
{
    Console.WriteLine(title);
}

class Incident
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int Severity { get; set; }
}

class DemoContext : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite("Data Source=:memory:");
}
