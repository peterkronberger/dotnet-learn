// GetOrAdd returns the stored value, or adds one if the key is missing.
using System.Collections.Concurrent;

ConcurrentDictionary<string, int> limits = new();

int first = limits.GetOrAdd("bob", 3);
int second = limits.GetOrAdd("bob", 99);
Console.WriteLine($"First call: {first}");
Console.WriteLine($"Second call: {second}");
