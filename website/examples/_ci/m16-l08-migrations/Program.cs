using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

// A database file in a fresh temporary directory
string directory = Directory.CreateTempSubdirectory().FullName;
string connectionString = $"Data Source={Path.Combine(directory, "demo.db")}";
var options = new DbContextOptionsBuilder<DemoContext>()
    .UseSqlite(connectionString)
    .Options;

try
{
    using var db = new DemoContext(options);

    // 1. The code knows two migrations, the database has none yet
    Console.WriteLine("1. Migrations in the code");
    foreach (string id in db.Database.GetMigrations())
    {
        Console.WriteLine($"   {Name(id)}");
    }

    // 2. An older version of the app applied only the first migration and
    //    stored a row
    string first = db.Database.GetMigrations().First();
    db.GetService<IMigrator>().Migrate(first);
    db.Database.ExecuteSql($"INSERT INTO Incidents (User) VALUES ('bob')");
    Console.WriteLine("2. Applied so far");
    foreach (string id in db.Database.GetAppliedMigrations())
    {
        Console.WriteLine($"   {Name(id)}");
    }

    // 3. Migrate applies the missing migration to the existing database
    db.Database.Migrate();
    Console.WriteLine("3. Applied after Migrate");
    foreach (string id in db.Database.GetAppliedMigrations())
    {
        Console.WriteLine($"   {Name(id)}");
    }

    // 4. The row survived, and the new column has its default value
    Incident bob = db.Incidents.Single();
    Console.WriteLine($"4. {bob.User}: Severity {bob.Severity}");
}
finally
{
    SqliteConnection.ClearAllPools();
    Directory.Delete(directory, true);
}

// The migration id is a timestamp and the name: show only the name
static string Name(string id) => id[(id.IndexOf('_') + 1)..];

public class Incident
{
    public int Id { get; set; }
    public required string User { get; set; }
    public int Severity { get; set; }
}

public class DemoContext(DbContextOptions<DemoContext> options)
    : DbContext(options)
{
    public DbSet<Incident> Incidents => Set<Incident>();
}

// The dotnet ef tool uses this factory to create the context at design time
public class DemoContextFactory : IDesignTimeDbContextFactory<DemoContext>
{
    public DemoContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<DemoContext>()
            .UseSqlite("Data Source=design-time.db")
            .Options;
        return new DemoContext(options);
    }
}
