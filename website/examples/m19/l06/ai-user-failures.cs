// Failed logins of the sample data: user and IP address
(string User, string Ip)[] failures =
[
    ("bob", "203.0.113.7"),
    ("bob", "203.0.113.7"),
    ("bob", "203.0.113.7"),
    ("bob", "203.0.113.7"),
    ("bob", "203.0.113.7"),
    ("alice", "192.0.2.10"),
    ("dave", "198.51.100.8"),
];

var users = failures
    .GroupBy(f => f.User)
    .OrderByDescending(g => g.Count())
    .ThenBy(g => g.Key)
    .Take(2)
    .Select(g => $"{g.Key}: {g.Count()}");

foreach (string line in users)
{
    Console.WriteLine(line);
}
