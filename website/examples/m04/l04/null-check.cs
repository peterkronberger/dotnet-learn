Describe("alice");
Describe(null);

void Describe(string? user)
{
    if (user != null)
    {
        Console.WriteLine($"Length: {user.Length}");
    }
    else
    {
        Console.WriteLine("No user");
    }
}
