using Microsoft.EntityFrameworkCore;

DirectoryInfo directory = Directory.CreateTempSubdirectory();
try
{
    var options = new DbContextOptionsBuilder<DemoContext>()
        .UseSqlite($"Data Source={directory.FullName}/demo.db")
        .Options;
    using var db = new DemoContext(options);
    db.Database.EnsureCreated();
    db.Incidents.AddRange(
        new Incident { User = "bob" }, new Incident { User = "dave" });
    db.SaveChanges();

    // A DbSet is the start of a query; no row is read yet
    IQueryable<Incident> all = db.Incidents;
    Console.WriteLine("Query created");

    // The query runs now, and every enumeration runs it again
    Console.WriteLine($"First run: {all.Count()} rows");
    db.Incidents.Add(new Incident { User = "alice" });
    db.SaveChanges();
    Console.WriteLine($"Second run: {all.Count()} rows");
}
finally
{
    directory.Delete(true);
}

class Incident
{
    public int Id { get; set; }
    public required string User { get; set; }
}

class DemoContext(DbContextOptions<DemoContext> options) : DbContext(options)
{
    public DbSet<Incident> Incidents => Set<Incident>();
}
