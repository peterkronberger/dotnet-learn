public class DemoContext(DbContextOptions<DemoContext> options)
    : DbContext(options)
{
    public ___<Incident> Incidents => Set<Incident>();
}
