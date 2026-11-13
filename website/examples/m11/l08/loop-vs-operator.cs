string[] users = ["alice", "bob", "carol", "bob"];

bool found = false;
foreach (string user in users)
{
    if (user == "carol")
    {
        found = true;
        break;
    }
}
Console.WriteLine($"With a loop: {found}");
Console.WriteLine($"With Any: {users.Any(u => u == "carol")}");
Console.WriteLine($"First bob at: {Array.IndexOf(users, users.First(u => u == "bob"))}");
