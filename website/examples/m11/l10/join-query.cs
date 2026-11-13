(int Number, string User)[] incidents = [(1, "bob"), (2, "dave")];
string[] eventUsers = ["bob", "alice", "dave", "bob"];

var query =
    from incident in incidents
    join eventUser in eventUsers on incident.User equals eventUser
    select $"#{incident.Number} {eventUser}";

foreach (string line in query)
{
    Console.WriteLine(line);
}
