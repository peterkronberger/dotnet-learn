// An own exception type that carries extra data
string[] users = ["bob", "bo"];
foreach (string user in users)
{
    try
    {
        Console.WriteLine(LogTools.CheckUser(user));
    }
    catch (UnknownUserException ex)
    {
        Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
        Console.WriteLine($"User in the exception: {ex.User}");
    }
}

static class LogTools
{
    public static string CheckUser(string user)
    {
        bool known =
            user == "alice" || user == "bob" ||
            user == "carol" || user == "dave";
        if (!known)
        {
            throw new UnknownUserException(user, "The user is not in the log.");
        }
        return $"{user} is known";
    }
}

class UnknownUserException : Exception
{
    public string User { get; }

    public UnknownUserException(string user, string message)
        : base(message)
    {
        User = user;
    }
}
