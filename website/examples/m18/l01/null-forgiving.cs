string[] users = ["alice", "bob", "carol", "dave"];

Console.WriteLine($"alice: {FindUser("alice", users)!.Length} characters");
try
{
    Console.WriteLine($"eve: {FindUser("eve", users)!.Length} characters");
}
catch (NullReferenceException)
{
    Console.WriteLine("eve: NullReferenceException");
}

// Return the name if it is in the list, otherwise null
static string? FindUser(string name, string[] knownUsers)
{
    return Array.Find(knownUsers, user => user == name);
}
