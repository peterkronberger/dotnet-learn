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

foreach (var (ip, count) in IpRanking.TopIps(lines))
{
    Console.WriteLine($"{ip}: {count}");
}

public static class IpRanking
{
    public static List<(string Ip, int Count)> TopIps(string[] lines)
    {
        return lines
            .Where(line => line.Contains("login-failed"))
            .Select(line => line.Split(' ')[3].Replace("ip=", ""))
            .GroupBy(ip => ip)
            .Select(g => (Ip: g.Key, Count: g.Count()))
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Ip)
            .Take(3)
            .ToList();
    }
}
