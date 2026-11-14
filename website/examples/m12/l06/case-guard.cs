// A case guard adds a condition to a pattern
string Describe(string eventType, string? user) => eventType switch
{
    "login-success" when user is null => "check user",
    "login-success" => "ok",
    "login-failed" => "failed",
    _ => "other",
};

Console.WriteLine(Describe("login-success", null));
Console.WriteLine(Describe("login-success", "alice"));
Console.WriteLine(Describe("login-failed", null));
Console.WriteLine(Describe("logout", "bob"));
