using Microsoft.EntityFrameworkCore;

// A fresh directory for the database file; it is deleted at the end
DirectoryInfo directory = Directory.CreateTempSubdirectory();
string connectionString = $"Data Source={directory.FullName}/demo.db";
try
{
    var options = new DbContextOptionsBuilder<DemoContext>()
        .UseSqlite(connectionString)
        .Options;

    // Create the database and the table, then save two new objects
    using (var db = new DemoContext(options))
    {
        db.Database.EnsureCreated();
        var bob = new Incident { User = "bob", FailureCount = 5 };
        var dave = new Incident { User = "dave", FailureCount = 1 };
        Console.WriteLine($"Id before SaveChanges: {bob.Id}");
        db.Incidents.AddRange(bob, dave);
        db.SaveChanges();
        Console.WriteLine($"Id after SaveChanges: {bob.Id}");
        Console.WriteLine($"Id of dave: {dave.Id}");
    }

    // Read them back with a LINQ query, in a new context
    using (var db = new DemoContext(options))
    {
        List<Incident> severe = db.Incidents
            .Where(i => i.FailureCount >= 3)
            .ToList();
        foreach (Incident incident in severe)
        {
            Console.WriteLine($"{incident.Id} {incident.User}");
        }
    }
}
finally
{
    directory.Delete(true);
}

class Incident
{
    public int Id { get; set; }
    public required string User { get; set; }
    public int FailureCount { get; set; }
}

class DemoContext(DbContextOptions<DemoContext> options) : DbContext(options)
{
    public DbSet<Incident> Incidents => Set<Incident>();
}
