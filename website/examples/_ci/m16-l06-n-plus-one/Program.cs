using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using var seed = new DemoContext();
seed.Database.OpenConnection();
seed.Database.EnsureCreated();
seed.Incidents.AddRange(
    new Incident { User = "bob", Events = [new Event(), new Event()] },
    new Incident { User = "alice", Events = [new Event()] },
    new Incident { User = "dave", Events = [new Event()] });
seed.SaveChanges();

// 1. Explicit loading inside a loop: one command for the incidents,
// one more for every incident
using var loop = new DemoContext(seed.Database.GetDbConnection());
CommandCounter.Count = 0;
foreach (Incident incident in loop.Incidents.ToList())
{
    loop.Entry(incident).Collection(i => i.Events).Load();
    Console.WriteLine($"{incident.User}: {incident.Events.Count} events");
}
Console.WriteLine($"1. Loop: {CommandCounter.Count} commands");

// 2. Fix: Include loads the events together with the incidents
using var db = new DemoContext(seed.Database.GetDbConnection());
CommandCounter.Count = 0;
foreach (Incident incident in db.Incidents.Include(i => i.Events))
{
    Console.WriteLine($"{incident.User}: {incident.Events.Count} events");
}
Console.WriteLine($"2. Include: {CommandCounter.Count} commands");

class Incident
{
    public int Id { get; set; }
    public string User { get; set; } = "";
    public List<Event> Events { get; set; } = [];
}

class Event
{
    public int Id { get; set; }
    public int IncidentId { get; set; }
}

class DemoContext : DbContext
{
    private readonly DbConnection? connection;

    public DemoContext() { }

    public DemoContext(DbConnection connection) => this.connection = connection;

    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (connection is null)
        {
            options.UseSqlite("Data Source=:memory:");
        }
        else
        {
            options.UseSqlite(connection);
        }
        options.AddInterceptors(new CommandCounter());
    }
}

// @fold-start
// Counts every command that runs in the database
class CommandCounter : DbCommandInterceptor
{
    public static int Count { get; set; }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        Count++;
        return result;
    }
}
// @fold-end
