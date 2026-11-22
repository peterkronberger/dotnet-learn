using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// One open connection keeps the in-memory database alive for all contexts
using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();

using (var seed = new DemoContext(connection))
{
    seed.Database.EnsureCreated();
    seed.Incidents.Add(new Incident { User = "bob", Status = "open" });
    seed.SaveChanges();
}

// 1. One SaveChanges call: the first change is fine, the second one breaks
//    the unique index on User. Neither change is saved.
using (var db = new DemoContext(connection))
{
    db.Incidents.Add(new Incident { User = "carol", Status = "open" });
    db.Incidents.Add(new Incident { User = "bob", Status = "open" });
    try
    {
        db.SaveChanges();
    }
    catch (DbUpdateException)
    {
        Console.WriteLine("1. SaveChanges failed");
    }
}

using (var db = new DemoContext(connection))
{
    Console.WriteLine($"   Incidents in the database: {db.Incidents.Count()}");
}

// 2. Two SaveChanges calls in one explicit transaction: Commit saves both,
//    leaving the block without Commit undoes both
using (var db = new DemoContext(connection))
{
    using var transaction = db.Database.BeginTransaction();
    db.Incidents.Add(new Incident { User = "dave", Status = "open" });
    db.SaveChanges();
    db.Incidents.Add(new Incident { User = "erin", Status = "open" });
    db.SaveChanges();
    Console.WriteLine("2. Two SaveChanges calls done, no Commit yet");
}

using (var db = new DemoContext(connection))
{
    Console.WriteLine($"   Incidents in the database: {db.Incidents.Count()}");
}

// 3. The same two calls, now with Commit
using (var db = new DemoContext(connection))
{
    using var transaction = db.Database.BeginTransaction();
    db.Incidents.Add(new Incident { User = "dave", Status = "open" });
    db.SaveChanges();
    db.Incidents.Add(new Incident { User = "erin", Status = "open" });
    db.SaveChanges();
    transaction.Commit();
    Console.WriteLine("3. Committed");
}

using (var db = new DemoContext(connection))
{
    Console.WriteLine($"   Incidents in the database: {db.Incidents.Count()}");
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<Incident>().HasIndex(i => i.User).IsUnique();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(connection);
}
