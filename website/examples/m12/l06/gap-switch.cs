// Choose an action: the first matching arm wins
string ActionFor(string eventType, int failures) => (eventType, failures) switch
{
    ("login-failed", >= 5) => "lock account",
    ("login-failed", _) => "watch",
    _ => "none",
};

Console.WriteLine(ActionFor("login-failed", 7));
Console.WriteLine(ActionFor("login-failed", 1));
Console.WriteLine(ActionFor("logout", 7));
