using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// A database file in a fresh temporary directory
string directory = Directory.CreateTempSubdirectory().FullName;
string connectionString = $"Data Source={Path.Combine(directory, "demo.db")}";

try
{
    // 1. The first model: EnsureCreated creates the table
    using (var db = new ContextV1(connectionString))
    {
        Console.WriteLine($"1. Created: {db.Database.EnsureCreated()}");
        db.Incidents.Add(new IncidentV1 { User = "bob" });
        db.SaveChanges();
    }

    // 2. The model grows by the column Severity. The database already
    //    exists, so EnsureCreated does nothing.
    using (var db = new ContextV2(connectionString))
    {
        Console.WriteLine($"2. Created: {db.Database.EnsureCreated()}");
        try
        {
            Console.WriteLine($"   Incidents: {db.Incidents.Count()}");
            Console.WriteLine($"   Severity of bob: {db.Incidents.Single().Severity}");
        }
        catch (SqliteException)
        {
            Console.WriteLine("   The query failed: the table has no column Severity");
        }
    }
}
finally
{
    SqliteConnection.ClearAllPools();
    Directory.Delete(directory, true);
}

public class IncidentV1
{
    public int Id { get; set; }
    public required string User { get; set; }
}

public class IncidentV2
{
    public int Id { get; set; }
    public required string User { get; set; }
    public int Severity { get; set; }
}

public class ContextV1(string connectionString) : DbContext
{
    public DbSet<IncidentV1> Incidents => Set<IncidentV1>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<IncidentV1>().ToTable("Incidents");

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(connectionString);
}

public class ContextV2(string connectionString) : DbContext
{
    public DbSet<IncidentV2> Incidents => Set<IncidentV2>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<IncidentV2>().ToTable("Incidents");

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(connectionString);
}
