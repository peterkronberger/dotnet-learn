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

using (var db = new DemoContext(connection))
{
    // 1. A new object is not tracked until the context learns about it
    var carol = new Incident { User = "carol", Status = "open" };
    Console.WriteLine($"1. New object: {db.Entry(carol).State}");
    db.Incidents.Add(carol);
    Console.WriteLine($"   After Add: {db.Entry(carol).State}");
    int written = db.SaveChanges();
    Console.WriteLine($"   After SaveChanges: {db.Entry(carol).State}, rows written: {written}");

    // 2. A loaded entity is tracked; a changed property marks it as modified
    Incident bob = db.Incidents.Single(i => i.User == "bob");
    Console.WriteLine($"2. Loaded: {db.Entry(bob).State}");
    bob.Status = "investigating";
    Console.WriteLine($"   After the change: {db.Entry(bob).State}");
    written = db.SaveChanges();
    Console.WriteLine($"   After SaveChanges: {db.Entry(bob).State}, rows written: {written}");

    // 3. Remove marks the entity as deleted; SaveChanges deletes the row
    db.Incidents.Remove(carol);
    Console.WriteLine($"3. After Remove: {db.Entry(carol).State}");
    written = db.SaveChanges();
    Console.WriteLine($"   After SaveChanges: {db.Entry(carol).State}, rows written: {written}");

    // 4. SaveChanges without any change writes nothing
    written = db.SaveChanges();
    Console.WriteLine($"4. Nothing changed, rows written: {written}");
}

using (var db = new DemoContext(connection))
{
    Console.WriteLine("5. In the database now");
    foreach (Incident row in db.Incidents.OrderBy(i => i.Id))
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
