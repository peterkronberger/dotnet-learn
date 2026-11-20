using Microsoft.EntityFrameworkCore;
using SecLog.Data.Entities;

namespace SecLog.Data;

/// <summary>The session with the SecLog database.</summary>
public class SecLogDbContext(DbContextOptions<SecLogDbContext> options)
    : DbContext(options)
{
    /// <summary>The table of the stored incidents.</summary>
    public DbSet<StoredIncident> Incidents => Set<StoredIncident>();

    /// <summary>Configures the schema: the fluent API wins over conventions.</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StoredIncident>(incident =>
        {
            // The user is required and has at most 64 characters
            incident.Property(i => i.User)
                .IsRequired()
                .HasMaxLength(64);
            // An index makes the search by user fast
            incident.HasIndex(i => i.User);
        });
    }
}
