// Test code for ai-brute-force-boundary.cs (m19-l03). Compiled together with the learner code.
public static class TaskTests
{
    private static readonly DateTimeOffset Start =
        DateTimeOffset.Parse("2026-03-02T08:00:00Z");

    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run() =>
    [
        Check("5 failures in 10 minutes are an alarm", true, 5, 1, true,
            "The rule says 5, so exactly 5 must raise the alarm."),
        Check("4 failures are no alarm", true, 4, 1, false,
            "Fewer than 5 failures must not raise the alarm."),
        Check("6 failures are an alarm", false, 6, 1, true,
            "More than 5 failures must raise the alarm."),
        Check("5 failures over 20 minutes are no alarm", false, 5, 5, false,
            "Only failures within 10 minutes count."),
    ];

    private static (string, bool, bool, string) Check(
        string name, bool visible, int count, int stepMinutes, bool expected, string hint)
    {
        var times = Enumerable.Range(0, count)
            .Select(i => Start.AddMinutes(i * stepMinutes))
            .ToList();
        try
        {
            bool actual = FailureRule.IsBruteForce(times);
            return (name, visible, actual == expected, actual == expected ? "" : hint);
        }
        catch (Exception ex)
        {
            return (name, visible, false, $"Threw {ex.GetType().Name}: {ex.Message}");
        }
    }
}
