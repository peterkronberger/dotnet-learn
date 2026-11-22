    using (SecLogDbContext db = new(dbOptions))
    {
        // Apply the migrations that the database does not have yet
        db.Database.Migrate();
        foreach (string migration in db.Database.GetAppliedMigrations())
        {
            Console.WriteLine($"Applied migration: {migration[(migration.IndexOf('_') + 1)..]}");
        }
        // ⋯ (unchanged: store the incidents and print the report)
    }
