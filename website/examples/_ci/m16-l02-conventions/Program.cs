using Microsoft.EntityFrameworkCore;

// Print the CREATE TABLE command that EF Core derives from the class
using var db = new DemoContext();
Console.WriteLine(db.Database.GenerateCreateScript());

class Incident
{
    public int Id { get; set; }
    public required string User { get; set; }
    public string? Note { get; set; }
    public int FailureCount { get; set; }
}

class DemoContext : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite("Data Source=:memory:");
}
