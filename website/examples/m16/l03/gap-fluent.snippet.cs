protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Event>()
        .HasOne(e => e.Incident)
        .WithMany(i => i.Events)
        .___(e => e.IncidentId);
}
