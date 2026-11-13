(int Number, string User)[] incidents = [(1, "bob"), (2, "dave"), (3, "erin")];
(string User, string Type)[] events =
[
    ("bob", "login-failed"),
    ("alice", "login-success"),
    ("dave", "login-failed"),
];

var rows = incidents.FullJoin(
    events,
    incident => incident.User,
    logEvent => logEvent.User,
    (incident, logEvent) =>
        $"{incident.User ?? "-"} | {logEvent.Type ?? "-"}");

foreach (string row in rows)
{
    Console.WriteLine(row);
}
