string[] users = ["alice", "bob", "carol", "dave"];

PrintLength("alice", users);
PrintLength("eve", users);

// Print the length of a known user, or say that the user is unknown
static void PrintLength(string name, string[] knownUsers)
{
    string? found = Array.Find(knownUsers, user => user == name);
    if (found is null)
    {
        Console.WriteLine($"{name}: unknown user");
        return;
    }
    Console.WriteLine($"{name}: {found.Length} characters");
}
