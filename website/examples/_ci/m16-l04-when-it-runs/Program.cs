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

// Building a query sends nothing
Console.WriteLine("Build the query");
var query = db.Incidents.Where(i => i.FailureCount >= 5);

// Enumerating sends one command
Console.WriteLine("Enumerate it");
foreach (var incident in query)
{
    Console.WriteLine($"Found {incident.User}");
}

// Aggregates and Any run in the database, too
Console.WriteLine("Count");
Console.WriteLine(db.Incidents.Count(i => i.FailureCount >= 5));
Console.WriteLine("Any");
Console.WriteLine(db.Incidents.Any(i => i.FailureCount >= 9));

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
