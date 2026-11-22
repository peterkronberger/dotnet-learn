using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SecLog.Data;

namespace SecLog.Cli;

/// <summary>
/// Creates the context for the dotnet ef tool at design time. The program
/// itself never uses this class.
/// </summary>
public class SecLogDbContextFactory : IDesignTimeDbContextFactory<SecLogDbContext>
{
    public SecLogDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<SecLogDbContext>()
            .UseSqlite("Data Source=design-time.db")
            .Options;
        return new SecLogDbContext(options);
    }
}
