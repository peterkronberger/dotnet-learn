// Every call is thread safe, but the two calls together are not one step.
using System.Collections.Concurrent;

ConcurrentDictionary<string, int> counts = new();

Task[] workers =
[
    Task.Run(() => Register("bob")),
    Task.Run(() => Register("bob")),
];
await Task.WhenAll(workers);

void Register(string name)
{
    if (!counts.ContainsKey(name))
    {
        // Another worker can add the key between the check and this line.
        counts[name] = 1;
    }
}
