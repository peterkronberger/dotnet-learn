// Count the failed logins of bob with a property pattern
Entry[] entries =
[
    new("login-failed", "bob", 3),
    new("login-failed", "dave", 1),
    new("login-failed", "bob", 8),
    new("logout", "bob", 0),
];
int count = 0;
foreach (Entry entry in entries)
{
    if (entry is { EventType: "login-failed", User: "bob", Attempts: >= 2 })
    {
        count++;
    }
}
Console.WriteLine(count);

record Entry(string EventType, string User, int Attempts);
