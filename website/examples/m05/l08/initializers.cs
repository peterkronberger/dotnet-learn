List<string> users = new List<string> { "alice", "bob" };

Dictionary<string, int> failures = new Dictionary<string, int>
{
    ["bob"] = 5,
    ["alice"] = 1,
};

Dictionary<string, int> sameFailures = new Dictionary<string, int>
{
    { "bob", 5 },
    { "alice", 1 },
};

Console.WriteLine(users.Count);
Console.WriteLine(failures["bob"]);
Console.WriteLine(sameFailures.Count);
