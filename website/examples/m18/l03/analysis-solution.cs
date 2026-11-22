// Task 1: start from the events parameter
// Task 2: keep only the failures
// Task 3: group by user, order by user, project into UserCount
// Task 4: return a finished list, not the query
(string User, string Type)[] events =
[
    ("bob", "login-failed"), ("alice", "login-success"),
    ("bob", "login-failed"), ("alice", "login-failed"),
    ("carol", "logout"),
];
foreach (UserCount row in Analysis.FailuresPerUser(events))
{
    Console.WriteLine($"{row.User}: {row.Count}");
}

record UserCount(string User, int Count);

static class Analysis
{
    // Failed logins per user, ordered by user name
    public static IReadOnlyList<UserCount> FailuresPerUser(
        IEnumerable<(string User, string Type)> events)
    {
        return events
            .Where(e => e.Type == "login-failed")
            .GroupBy(e => e.User)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new UserCount(g.Key, g.Count()))
            .ToList();
    }
}
