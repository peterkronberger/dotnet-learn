using Microsoft.EntityFrameworkCore;

using var db = new DemoContext();

// Method syntax: filter, sort, project
var methodQuery = db.Incidents
    .Where(i => i.FailureCount >= 5)
    .OrderByDescending(i => i.FailureCount)
    .Select(i => new { i.User, i.FailureCount });
Console.WriteLine(methodQuery.ToQueryString());
Console.WriteLine();

// Query syntax: the compiler turns it into the same method calls
var queryQuery =
    from i in db.Incidents
    where i.FailureCount >= 5
    orderby i.FailureCount descending
    select new { i.User, i.FailureCount };
Console.WriteLine($"Same SQL: {queryQuery.ToQueryString() == methodQuery.ToQueryString()}");
Console.WriteLine();

// Take becomes LIMIT
Console.WriteLine(db.Incidents.OrderBy(i => i.Id).Take(1).ToQueryString());

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
        options.UseSqlite("Data Source=:memory:");
}
