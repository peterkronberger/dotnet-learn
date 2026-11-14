Entry[] entries =
[
    new("login-failed", 4),
    new("logout", 0),
    new("login-failed", 1),
];
int count = 0;
foreach (Entry entry in entries)
{
    if (entry is { EventType: "login-failed" or "logout", Attempts: > 0 })
    {
        count++;
    }
}
Console.WriteLine(count);

record Entry(string EventType, int Attempts);
