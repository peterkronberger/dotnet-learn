(string User, int Number)[] incidents = [("bob", 1), ("dave", 2)];
(string User, string Type)[] events =
[
    ("bob", "login-failed"),
    ("alice", "login-success"),
    ("dave", "login-failed"),
    ("bob", "login-failed"),
];

Console.WriteLine("With two loops:");
foreach (var incident in incidents)
{
    foreach (var logEvent in events)
    {
        if (incident.User == logEvent.User)
        {
            Console.WriteLine($"#{incident.Number} {logEvent.Type}");
        }
    }
}

Console.WriteLine("With Join:");
var pairs = incidents.Join(
    events,
    incident => incident.User,
    logEvent => logEvent.User,
    (incident, logEvent) => $"#{incident.Number} {logEvent.Type}");
foreach (string pair in pairs)
{
    Console.WriteLine(pair);
}
