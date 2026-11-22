// Test code for core-severity-task.cs (m18-l02). Compiled together with the learner code.
using SecLog.Model;

public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run() =>
    [
        Rate("2 failures are Low", true, 2, Severity.Low, "Counts 1 and 2 are Low."),
        Rate("3 failures are Medium", true, 3, Severity.Medium, "The limit 3 already belongs to Medium."),
        Rate("4 failures are Medium", true, 4, Severity.Medium, "Counts 3 and 4 are Medium."),
        Rate("5 failures are High", true, 5, Severity.High, "The limit 5 already belongs to High."),
        Rate("0 failures are None", false, 0, Severity.None, "Only 0 is None."),
        RejectsNegative(),
    ];

    private static (string, bool, bool, string) Rate(
        string name, bool visible, int count, Severity expected, string hint)
    {
        try
        {
            Severity actual = SeverityRating.For(count);
            return (name, visible, actual == expected, actual == expected ? "" : hint);
        }
        catch (Exception ex)
        {
            return (name, visible, false, $"Threw {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static (string, bool, bool, string) RejectsNegative()
    {
        const string name = "-1 is rejected";
        try
        {
            SeverityRating.For(-1);
            return (name, false, false, "A negative number must throw ArgumentOutOfRangeException.");
        }
        catch (ArgumentOutOfRangeException)
        {
            return (name, false, true, "");
        }
        catch (Exception ex)
        {
            return (name, false, false, $"Threw {ex.GetType().Name} instead of ArgumentOutOfRangeException.");
        }
    }
}
