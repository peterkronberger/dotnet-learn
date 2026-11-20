using Microsoft.EntityFrameworkCore;
using SecLog.Data.Entities;

namespace SecLog.Data;

/// <summary>The session with the SecLog database.</summary>
public class SecLogDbContext(DbContextOptions<SecLogDbContext> options)
    : DbContext(options)
{
    /// <summary>The table of the stored incidents.</summary>
    public DbSet<StoredIncident> Incidents => Set<StoredIncident>();
}
