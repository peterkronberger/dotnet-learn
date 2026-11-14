Entry?[] entries =
[
    new("login-failed", "bob"),
    new("login-failed", null),
    null,
];
foreach (Entry? entry in entries)
{
    Console.WriteLine(entry is { EventType: "login-failed", User: { } });
}

record Entry(string EventType, string? User);
