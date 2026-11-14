// Parallel.ForEachAsync runs async work, at most two items at a time.
using System.Collections.Concurrent;

string[] names = ["web", "vpn", "mail", "db"];
ConcurrentDictionary<string, int> lengths = new();
ParallelOptions options = new() { MaxDegreeOfParallelism = 2 };

await Parallel.ForEachAsync(names, options, async (name, token) =>
{
    await Task.Delay(10, token);
    lengths[name] = name.Length;
});

foreach (KeyValuePair<string, int> entry in lengths.OrderBy(e => e.Key))
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}
