// The user name comes from outside, for example from an input field
string user = "bob";
// Send it as a parameter, never as part of the SQL text
var query = db.Incidents.___($"SELECT * FROM Incidents WHERE User = {user}");
Console.WriteLine(query.ToQueryString());
