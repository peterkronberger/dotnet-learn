// Failed logins of the sample data: user and UTC time
(string User, string Time)[] failures =
[
    ("bob", "2026-03-02T08:01:40Z"),
    ("bob", "2026-03-02T08:02:05Z"),
    ("bob", "2026-03-02T08:04:02Z"),
    ("bob", "2026-03-02T08:05:15Z"),
    ("bob", "2026-03-02T08:06:48Z"),
    ("alice", "2026-03-02T08:15:03Z"),
    ("dave", "2026-03-02T09:10:09Z"),
];

foreach (var group in failures.GroupBy(f => f.User))
{
    var times = group.Select(f => DateTimeOffset.Parse(f.Time)).ToList();
    bool alarm = FailureRule.IsBruteForce(times);
    Console.WriteLine($"{group.Key}: {(alarm ? "alarm" : "no alarm")}");
}

public static class FailureRule
{
    // Rule: 5 failed attempts within 10 minutes
    public static bool IsBruteForce(List<DateTimeOffset> times)
    {
        DateTimeOffset latest = times.Max();
        int recent = times.Count(t => latest - t <= TimeSpan.FromMinutes(10));
        return recent >= 5;
    }
}
