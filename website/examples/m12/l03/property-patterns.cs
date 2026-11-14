// The same test as a comparison chain and as a property pattern
Entry?[] entries =
[
    new("login-failed", "bob"),
    new("login-failed", "dave"),
    new("logout", "bob"),
    null,
];
foreach (Entry? entry in entries)
{
    bool withOperators = entry != null
        && entry.EventType == "login-failed" && entry.User == "bob";
    bool withPattern = entry is { EventType: "login-failed", User: "bob" };
    Console.WriteLine($"{withOperators} {withPattern}");
}

record Entry(string EventType, string? User);
