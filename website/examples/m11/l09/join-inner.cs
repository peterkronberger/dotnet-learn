(int Number, string User)[] incidents = [(1, "bob"), (2, "dave"), (3, "erin")];
(string User, string Type)[] events =
[
    ("bob", "login-failed"),
    ("alice", "login-success"),
    ("dave", "login-failed"),
    ("bob", "login-failed"),
];

var pairs = incidents.Join(
    events,
    incident => incident.User,
    logEvent => logEvent.User,
    (incident, logEvent) => $"#{incident.Number} {incident.User}: {logEvent.Type}");

foreach (string pair in pairs)
{
    Console.WriteLine(pair);
}
Console.WriteLine($"Pairs: {pairs.Count()}");
