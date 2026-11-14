// The static constructor runs once, even if many tasks use the class.
Task[] tasks = new Task[4];
for (int i = 0; i < tasks.Length; i++)
{
    tasks[i] = Task.Run(() => Settings.Level);
}
await Task.WhenAll(tasks);
Console.WriteLine($"Level: {Settings.Level}");
Console.WriteLine($"Static constructor runs: {Settings.Runs}");

static class Settings
{
    public static readonly string Level;
    public static int Runs;

    static Settings()
    {
        Runs++;
        Level = "warn";
    }
}
