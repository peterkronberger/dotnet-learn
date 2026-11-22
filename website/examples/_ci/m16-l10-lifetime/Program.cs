using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();

ServiceCollection services = new();
// AddDbContext registers the context as a scoped service
services.AddDbContext<DemoContext>(o => o.UseSqlite(connection));
// The factory creates a new context on every call
services.AddDbContextFactory<FactoryContext>(o => o.UseSqlite(connection));
services.AddSingleton<WrongCounter>();
services.AddScoped<RunCounter>();

using ServiceProvider provider = services.BuildServiceProvider(
    new ServiceProviderOptions { ValidateScopes = true });

using (IServiceScope setup = provider.CreateScope())
{
    var db = setup.ServiceProvider.GetRequiredService<DemoContext>();
    await db.Database.EnsureCreatedAsync();
    db.Incidents.Add(new Incident { User = "bob" });
    await db.SaveChangesAsync();
}

// 1. A scope is one unit of work: the same context inside, a new one outside
using (IServiceScope scope = provider.CreateScope())
{
    var a = scope.ServiceProvider.GetRequiredService<DemoContext>();
    var b = scope.ServiceProvider.GetRequiredService<DemoContext>();
    Console.WriteLine($"1. Same scope, same context: {ReferenceEquals(a, b)}");
}
using (IServiceScope other = provider.CreateScope())
using (IServiceScope scope = provider.CreateScope())
{
    var a = other.ServiceProvider.GetRequiredService<DemoContext>();
    var b = scope.ServiceProvider.GetRequiredService<DemoContext>();
    Console.WriteLine($"   Two scopes, same context: {ReferenceEquals(a, b)}");
}

// 2. A singleton must not hold a scoped context (captive dependency)
try
{
    provider.GetRequiredService<WrongCounter>();
}
catch (InvalidOperationException)
{
    Console.WriteLine("2. The singleton cannot use the scoped context");
}

// 3. A long-living service creates its own scope for every unit of work
await using (AsyncServiceScope scope = provider.CreateAsyncScope())
{
    var counter = scope.ServiceProvider.GetRequiredService<RunCounter>();
    Console.WriteLine($"3. Incidents in this run: {await counter.CountAsync()}");
}

// 4. Or it asks the factory for a short-living context
var factory = provider.GetRequiredService<IDbContextFactory<FactoryContext>>();
await using (FactoryContext db = await factory.CreateDbContextAsync())
{
    Console.WriteLine($"4. Incidents from the factory: {await db.Incidents.CountAsync()}");
}

class Incident
{
    public int Id { get; set; }
    public string User { get; set; } = "";
}

class DemoContext(DbContextOptions<DemoContext> options) : DbContext(options)
{
    public DbSet<Incident> Incidents => Set<Incident>();
}

class FactoryContext(DbContextOptions<FactoryContext> options) : DbContext(options)
{
    public DbSet<Incident> Incidents => Set<Incident>();
}

class RunCounter(DemoContext db)
{
    public Task<int> CountAsync() => db.Incidents.CountAsync();
}

// A singleton that asks for a scoped context: the container refuses it
class WrongCounter(DemoContext db)
{
    public DemoContext Db => db;
}
