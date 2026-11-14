// Two tasks register the same key. Both can pass the check before one adds.
Dictionary<string, int> seen = new();

void Register(string key)
{
    if (!seen.ContainsKey(key))
    {
        seen.Add(key, 1);
    }
}

Task first = Task.Run(() => Register("bob"));
Task second = Task.Run(() => Register("bob"));

await Task.WhenAll(first, second);
Console.WriteLine($"Entries: {seen.Count}");
