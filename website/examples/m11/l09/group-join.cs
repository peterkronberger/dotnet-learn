(int Number, string User)[] incidents = [(1, "bob"), (2, "dave"), (3, "erin")];
string[] eventUsers = ["bob", "alice", "dave", "bob"];

var groups = incidents.GroupJoin(
    eventUsers,
    incident => incident.User,
    eventUser => eventUser,
    (incident, matches) => new { incident.Number, Events = matches.Count() });

foreach (var row in groups)
{
    Console.WriteLine($"#{row.Number}: {row.Events} events");
}
