string[] events = { "logout", "login-failed", "logout", "logout" };
Dictionary<string, int> counts = new Dictionary<string, int>();

foreach (string kind in events)
{
    counts.TryGetValue(kind, out int count);
    counts[kind] = count + 1;
}

Console.WriteLine(counts["logout"]);
Console.WriteLine(counts["login-failed"]);
Console.WriteLine(counts.Count);
