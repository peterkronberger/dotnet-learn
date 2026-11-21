using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using var db = new DemoContext();
db.Database.OpenConnection();
db.Database.EnsureCreated();
db.Incidents.AddRange(
    new Incident { User = "bob", FailureCount = 5 },
    new Incident { User = "alice", FailureCount = 1 },
    new Incident { User = "dave", FailureCount = 1 });
db.SaveChanges();
CommandPrinter.Enabled = true;

// 1. IQueryable: Where is translated, the database filters
Console.WriteLine("1. IQueryable");
List<Incident> inDatabase = db.Incidents
    .Where(i => i.FailureCount >= 5)
    .ToList();
Console.WriteLine($"Rows loaded: {inDatabase.Count}");

// 2. IEnumerable: Where is a delegate, the program filters
Console.WriteLine("2. IEnumerable");
IEnumerable<Incident> all = db.Incidents;
List<Incident> inMemory = all
    .Where(i => i.FailureCount >= 5)
    .ToList();
Console.WriteLine($"Rows kept: {inMemory.Count}");

// 3. AsEnumerable: a deliberate switch after the translated part
Console.WriteLine("3. AsEnumerable");
List<Incident> switched = db.Incidents
    .Where(i => i.FailureCount >= 1)
    .AsEnumerable()
    .Where(i => i.User.Length == 3)
    .ToList();
Console.WriteLine($"Rows kept: {switched.Count}");

// 4. ToList too early: everything is loaded, the rest runs in memory
Console.WriteLine("4. ToList too early");
List<Incident> tooEarly = db.Incidents
    .ToList()
    .Where(i => i.FailureCount >= 5)
    .ToList();
Console.WriteLine($"Rows kept: {tooEarly.Count}");

class Incident
{
    public int Id { get; set; }
    public string User { get; set; } = "";
    public int FailureCount { get; set; }
}

class DemoContext : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite("Data Source=:memory:")
            .AddInterceptors(new CommandPrinter());
}

// @fold-start
// Prints the text of every command that runs in the database
class CommandPrinter : DbCommandInterceptor
{
    public static bool Enabled { get; set; }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        if (Enabled)
        {
            Console.WriteLine(command.CommandText);
        }
        return result;
    }
}
// @fold-end
