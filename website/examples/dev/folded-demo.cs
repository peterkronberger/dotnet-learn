// SecLog sample: count failed logins per user. The two marked blocks are hidden in the runner.
// @fold-start
Console.WriteLine("SecLog report");
Console.WriteLine("Source: sample log");
Console.WriteLine();
// @fold-end
var users = new[] { "alice", "bob", "bob", "carol", "bob", "alice" };
var results = new[] { true, false, false, true, false, false };

Console.WriteLine("Failed logins:");
foreach (var name in new[] { "alice", "bob", "carol" })
{
    var failures = CountFailures(name);
    Console.WriteLine($"{name}: {failures}");
}

// @fold-start
Console.WriteLine();
Console.WriteLine("Users with more than 1 failure:");
foreach (var name in new[] { "alice", "bob", "carol" })
{
    if (CountFailures(name) > 1)
    {
        Console.WriteLine(name);
    }
}
// @fold-end

int CountFailures(string name)
{
    var count = 0;
    for (var i = 0; i < users.Length; i++)
    {
        if (users[i] == name && !results[i])
        {
            count++;
        }
    }
    return count;
}
