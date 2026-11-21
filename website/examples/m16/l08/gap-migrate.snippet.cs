// Apply every migration that the database does not have yet
db.Database.___();
// The table __EFMigrationsHistory now lists the applied migrations
foreach (string id in db.Database.GetAppliedMigrations())
{
    Console.WriteLine(id);
}
