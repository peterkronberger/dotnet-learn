// Two workers count names in one ConcurrentDictionary.
using System.Collections.Concurrent;

ConcurrentDictionary<string, int> counts = new();
string[] first = ["bob", "alice", "bob", "bob"];
string[] second = ["bob", "dave", "alice"];

Task[] workers =
[
    Task.Run(() => Count(first)),
    Task.Run(() => Count(second)),
];
await Task.WhenAll(workers);

foreach (KeyValuePair<string, int> entry in counts.OrderBy(e => e.Key))
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}

void Count(string[] names)
{
    foreach (string name in names)
    {
        counts.AddOrUpdate(name, 1, (_, count) => count + 1);
    }
}
