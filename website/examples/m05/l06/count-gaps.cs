string[] users = { "bob", "alice", "bob" };
Dictionary<string, int> counts = new Dictionary<string, int>();

foreach (string user in users)
{
    counts.TryGetValue(user, out int count);
    counts[user] = 1;
}

Console.WriteLine(counts["bob"]);
