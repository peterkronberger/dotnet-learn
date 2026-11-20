using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

// Print the schema for a class with annotations and fluent configuration
using var db = new DemoContext();
Console.WriteLine(db.Database.GenerateCreateScript());

// SQLite ignores the length in the script, so ask the model itself
var incident = db.Model.FindEntityType(typeof(Incident))!;
Console.WriteLine($"User: {incident.FindProperty("User")!.GetMaxLength()}");
Console.WriteLine($"Source: {incident.FindProperty("Source")!.GetMaxLength()}");

class Incident
{
    [Key]
    public int Number { get; set; }

    [Required]
    [MaxLength(20)]
    public string User { get; set; } = "";

    [MaxLength(20)]
    public string Source { get; set; } = "";
}

class Assignment
{
    public int IncidentId { get; set; }
    public int AnalystId { get; set; }
}

class DemoContext : DbContext
{
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<Assignment> Assignments => Set<Assignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // The fluent API wins over the annotation [MaxLength(20)]
        modelBuilder.Entity<Incident>()
            .Property(i => i.User)
            .HasMaxLength(64);

        // An index for fast search by user
        modelBuilder.Entity<Incident>()
            .HasIndex(i => i.User);

        // A key of two properties
        modelBuilder.Entity<Assignment>()
            .HasKey(a => new { a.IncidentId, a.AnalystId });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite("Data Source=:memory:");
}
