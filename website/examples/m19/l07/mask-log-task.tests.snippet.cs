// Test code for mask-log-task.cs (m19-l07). Compiled together with the learner code.
public static class TaskTests
{
    private static readonly string[] Input =
    [
        "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7",
        "2026-03-02T08:03:31Z login-success user=carol ip=198.51.100.23",
        "2026-03-02T08:30:00Z logout user=alice ip=192.0.2.10",
    ];

    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run() =>
    [
        Check("the user name is replaced", true,
            output => output.All(l => !l.Contains("bob") && !l.Contains("carol") && !l.Contains("alice")),
            "Replace the value after user= with a placeholder."),
        Check("the timestamp and the event stay", true,
            output => output[0].StartsWith("2026-03-02T08:01:40Z login-failed"),
            "Keep the timestamp and the event type, the question needs them."),
        Check("no IP address is left in the output", false,
            output => output.All(l => !l.Contains("203.0.113.") && !l.Contains("198.51.100.") && !l.Contains("192.0.2.")),
            "An IP address can identify a person. Replace the value after ip= as well."),
        Check("every line still has an ip= part", false,
            output => output.All(l => l.Contains("ip=")),
            "Keep the ip= key and replace only its value."),
    ];

    private static (string, bool, bool, string) Check(
        string name, bool visible, Func<string[], bool> test, string hint)
    {
        try
        {
            bool ok = test(LogMasker.MaskLines(Input));
            return (name, visible, ok, ok ? "" : hint);
        }
        catch (Exception ex)
        {
            return (name, visible, false, $"Threw {ex.GetType().Name}: {ex.Message}");
        }
    }
}
