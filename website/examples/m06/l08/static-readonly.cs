Console.WriteLine(Limits.MaxAttempts);
Console.WriteLine(Limits.EventTypes.Length);
Console.WriteLine(Limits.EventTypes[1]);

static class Limits
{
    public const int MaxAttempts = 5;
    public static readonly string[] EventTypes =
        ["login-success", "login-failed", "logout"];
}
