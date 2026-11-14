// A switch over a tuple of two values
string Decide(string eventType, int failures) => (eventType, failures) switch
{
    ("login-failed", >= 5) => "lock account",
    ("login-failed", _) => "watch",
    ("login-success", _) => "none",
    _ => "unknown",
};

Console.WriteLine(Decide("login-failed", 5));
Console.WriteLine(Decide("login-failed", 2));
Console.WriteLine(Decide("login-success", 9));
Console.WriteLine(Decide("logout", 0));
