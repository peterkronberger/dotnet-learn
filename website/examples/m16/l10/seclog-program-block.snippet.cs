// The database context comes from the container: one scoped instance per scope
services.AddDbContext<SecLogDbContext>(options => options.UseSqlite(
    configuration.GetConnectionString("SecLog")));
// The analysis runs in a background service that stops the host when it is done
services.AddHostedService(provider => new AnalysisService(
    stoppingToken => RunAnalysisAsync(provider, stoppingToken),
    provider.GetRequiredService<IHostApplicationLifetime>()));
// ⋯

async Task RunAnalysisAsync(
    IServiceProvider provider, CancellationToken stoppingToken)
{
    // ⋯
    // One scope is one unit of work: it gets its own context and disposes it
    await using (AsyncServiceScope dbScope = provider.CreateAsyncScope())
    {
        SecLogDbContext db = dbScope.ServiceProvider
            .GetRequiredService<SecLogDbContext>();
        // Apply the migrations that the database does not have yet
        await db.Database.MigrateAsync(stoppingToken);
        db.Incidents.AddRange(incidents.Select(StoredIncident.From));
        await db.SaveChangesAsync(stoppingToken);
        List<StoredIncident> stored = await db.Incidents
            .OrderBy(i => i.Id)
            .ToListAsync(stoppingToken);
        // ⋯ (every other database call is async in the same way)
    }
}
