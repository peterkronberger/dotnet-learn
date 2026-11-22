// Test code for core-analysis-task.cs (m18-l03). Compiled together with the learner code.
using System.Globalization;
using SecLog.Model;

public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run() =>
    [
        Hours("Hour 8 has 6 failures", true, 8, 6,
            "Count the failures of hour 8 only."),
        Hours("Hour 9 has 1 failure", true, 9, 1,
            "Count the failures of hour 9 only."),
        OtherOffset(),
        EmptyList(),
    ];

    private static LogEvent[] SampleFailures() =>
    [
        Make("2026-03-02T08:01:40Z"), Make("2026-03-02T08:02:05Z"),
        Make("2026-03-02T08:04:02Z"), Make("2026-03-02T08:05:15Z"),
        Make("2026-03-02T08:06:48Z"), Make("2026-03-02T08:15:03Z"),
        Make("2026-03-02T09:10:09Z"),
    ];

    private static LogEvent Make(string timestamp) =>
        new(DateTimeOffset.Parse(timestamp, CultureInfo.InvariantCulture),
            "login-failed", "bob");

    private static (string, bool, bool, string) Hours(
        string name, bool visible, int hour, int expected, string hint)
    {
        try
        {
            var rows = LogAnalysis.FailuresPerHour(SampleFailures());
            int actual = rows.FirstOrDefault(r => r.Hour == hour)?.Count ?? 0;
            return (name, visible, actual == expected, actual == expected ? "" : hint);
        }
        catch (Exception ex)
        {
            return (name, visible, false, $"Threw {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static (string, bool, bool, string) OtherOffset()
    {
        const string name = "09:30 at +01:00 counts for hour 8";
        try
        {
            LogEvent[] events = [Make("2026-03-02T09:30:00+01:00")];
            var rows = LogAnalysis.FailuresPerHour(events);
            bool ok = rows.Count == 1 && rows[0].Hour == 8 && rows[0].Count == 1;
            return (name, false, ok, ok ? "" : "Group by Timestamp.UtcDateTime.Hour, not Timestamp.Hour.");
        }
        catch (Exception ex)
        {
            return (name, false, false, $"Threw {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static (string, bool, bool, string) EmptyList()
    {
        const string name = "No events give an empty list";
        try
        {
            var rows = LogAnalysis.FailuresPerHour([]);
            return (name, false, rows.Count == 0, rows.Count == 0 ? "" : "An empty input gives an empty result.");
        }
        catch (Exception ex)
        {
            return (name, false, false, $"Threw {ex.GetType().Name}: {ex.Message}");
        }
    }
}
