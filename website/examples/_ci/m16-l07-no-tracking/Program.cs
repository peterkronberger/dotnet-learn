using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// One open connection keeps the in-memory database alive for all contexts
using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();

using (var seed = new DemoContext(connection))
{
    seed.Database.EnsureCreated();
    seed.Incidents.AddRange(
        new Incident { User = "bob", Status = "open" },
        new Incident { User = "dave", Status = "open" });
    seed.SaveChanges();
}

// 1. A tracking query (the default): the change is saved
using (var db = new DemoContext(connection))
{
    Incident bob = db.Incidents.Single(i => i.User == "bob");
    bob.Status = "investigating";
    Console.WriteLine($"1. Tracking: {db.Entry(bob).State}");
    Console.WriteLine($"   Rows written: {db.SaveChanges()}");
}

// 2. A no-tracking query: the context does not watch the entity
using (var db = new DemoContext(connection))
{
    Incident dave = db.Incidents.AsNoTracking().Single(i => i.User == "dave");
    dave.Status = "investigating";
    Console.WriteLine($"2. AsNoTracking: {db.Entry(dave).State}");
    Console.WriteLine($"   Rows written: {db.SaveChanges()}");
}

using (var db = new DemoContext(connection))
{
    Console.WriteLine("3. In the database now");
    foreach (Incident row in db.Incidents.AsNoTracking().OrderBy(i => i.Id))
    {
        Console.WriteLine($"   {row.User}: {row.Status}");
    }
}

public class Incident
{
    public int Id { get; set; }
    public required string User { get; set; }
    public required string Status { get; set; }
}

public class DemoContext(SqliteConnection connection) : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(connection);
}
