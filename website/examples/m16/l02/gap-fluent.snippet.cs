protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Incident>()
        .Property(i => i.Source)
        .___(32);
}
