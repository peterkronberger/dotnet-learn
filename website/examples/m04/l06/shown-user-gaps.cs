Console.WriteLine(Shown("bob"));
Console.WriteLine(Shown(null));

string Shown(string? user)
{
    if (user == null)
    {
        return "unknown";
    }
    return user;
}
