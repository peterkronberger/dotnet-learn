// Log lines of the sample data
string[] lines =
[
    "2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10",
    "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:02:05Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:03:31Z login-success user=carol ip=198.51.100.23",
    "2026-03-02T08:04:02Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:05:15Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:06:48Z login-failed user=bob ip=203.0.113.7",
    "2026-03-02T08:07:20Z login-success user=dave ip=198.51.100.8",
    "2026-03-02T08:15:03Z login-failed user=alice ip=192.0.2.10",
    "2026-03-02T08:15:30Z login-success user=alice ip=192.0.2.10",
    "2026-03-02T08:42:11Z logout user=carol ip=198.51.100.23",
    "2026-03-02T09:10:09Z login-failed user=dave ip=198.51.100.8",
    "2026-03-02T09:10:41Z login-success user=dave ip=198.51.100.8",
    "2026-03-02T09:30:00Z logout user=alice ip=192.0.2.10",
];

var failuresPerUser = lines
    .Select(LogParser.Parse)
    .OfType<LogEvent>()
    .Where(e => e.Kind == "login-failed")
    .GroupBy(e => e.User)
    .Select(g => (User: g.Key, Count: g.Count()))
    .OrderByDescending(x => x.Count)
    .ThenBy(x => x.User);

foreach (var (user, count) in failuresPerUser)
{
    Console.WriteLine($"{user}: {count}");
}

// Existing code of the project (part of the request)
public record LogEvent(string Kind, string User, string Ip);

public static class LogParser
{
    // Returns null if the line does not have four parts
    public static LogEvent? Parse(string line)
    {
        string[] parts = line.Split(' ');
        if (parts.Length != 4)
        {
            return null;
        }
        return new LogEvent(
            parts[1],
            parts[2].Replace("user=", ""),
            parts[3].Replace("ip=", ""));
    }
}
