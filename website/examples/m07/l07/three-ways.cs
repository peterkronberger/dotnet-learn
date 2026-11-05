Dictionary<string, int> limits = new() { ["alice"] = 3, ["bob"] = 5 };

// Way 1: the indexer throws an exception for a missing key
try
{
    Console.WriteLine(limits["carol"]);
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"Exception: {ex.GetType().Name}");
}

// Way 2: the Try method reports the failure with false
if (limits.TryGetValue("carol", out int limit))
{
    Console.WriteLine($"Limit: {limit}");
}
else
{
    Console.WriteLine("Try method: no limit for carol");
}

// Way 3: the return value null means "not found"
int? found = FindLimit("carol");
Console.WriteLine($"Return value is null: {found == null}");

int? FindLimit(string user)
{
    if (limits.TryGetValue(user, out int value))
    {
        return value;
    }
    return null;
}
