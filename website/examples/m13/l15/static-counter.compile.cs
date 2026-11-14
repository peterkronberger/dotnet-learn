// Static mutable state: every task and every request shares it.
Task[] tasks =
[
    Task.Run(Stats.Record),
    Task.Run(Stats.Record),
];
await Task.WhenAll(tasks);
Console.WriteLine(Stats.Total);

static class Stats
{
    public static int Total;

    public static void Record()
    {
        Total++;
    }
}
