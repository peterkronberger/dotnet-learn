// Test code for top-ips-task.cs (m19-l06). Compiled together with the learner code.
public static class TaskTests
{
    private static readonly string[] Lines =
    [
        "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7",
        "2026-03-02T08:02:05Z login-failed user=bob ip=203.0.113.7",
        "2026-03-02T08:03:31Z login-success user=carol ip=198.51.100.23",
        "2026-03-02T08:15:03Z login-failed user=alice ip=192.0.2.10",
        "2026-03-02T09:10:09Z login-failed user=dave ip=198.51.100.8",
        "2026-03-02T09:10:30Z login-failed user=dave ip=198.51.100.8",
        "2026-03-02T09:10:41Z login-failed user=dave ip=198.51.100.8",
    ];

    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run() =>
    [
        Check("minimum 2 keeps only IPs with 2 or more failures", true, 2,
            ["198.51.100.8", "203.0.113.7"],
            "Only IPs with at least 2 failed logins may stay in the list."),
        Check("minimum 1 keeps every IP with a failure", true, 1,
            ["198.51.100.8", "203.0.113.7", "192.0.2.10"],
            "With minimum 1 every IP with a failed login is listed."),
        Check("an IP with exactly the minimum stays", false, 3,
            ["198.51.100.8"],
            "Use >= so that an IP with exactly minFailures stays."),
        Check("a high minimum gives an empty list", false, 10, [],
            "If no IP reaches the minimum, the list is empty."),
    ];

    private static (string, bool, bool, string) Check(
        string name, bool visible, int min, string[] expected, string hint)
    {
        try
        {
            var actual = IpRanking.TopIps(Lines, min).Select(x => x.Ip).ToArray();
            bool ok = actual.SequenceEqual(expected);
            return (name, visible, ok, ok ? "" : hint);
        }
        catch (Exception ex)
        {
            return (name, visible, false, $"Threw {ex.GetType().Name}: {ex.Message}");
        }
    }
}
