string?[] users = [null, "", "  ", "bob"];

foreach (string? user in users)
{
    if (string.IsNullOrBlank(user))
    {
        Console.WriteLine("Rejected: the user name is missing.");
    }
    else
    {
        Console.WriteLine($"Accepted: {user}");
    }
}
