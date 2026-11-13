string[] events =
[
    "login", "logout", "login", "login-failed", "login", "logout", "login-failed",
];

var top = events
    .GroupBy(e => e)
    .OrderByDescending(g => g.Count())
    .ThenBy(g => g.Key, StringComparer.Ordinal)
    .Take(2);

foreach (var eventGroup in top)
{
    Console.WriteLine($"{eventGroup.Key}: {eventGroup.Count()}");
}
