// Parallel.ForEach over sources; results go into a ConcurrentDictionary.
using System.Collections.Concurrent;

Dictionary<string, string[]> sources = new()
{
    ["web"] = ["login-failed", "login-success", "login-failed"],
    ["vpn"] = ["login-failed", "logout"],
};

ConcurrentDictionary<string, int> failures = new();
Parallel.ForEach(sources, source =>
{
    failures[source.Key] = source.Value.Count(e => e == "login-failed");
});

foreach (KeyValuePair<string, int> entry in failures.OrderBy(e => e.Key))
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}
