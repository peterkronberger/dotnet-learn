// Test code for failure-rate-task.cs. Compiled together with the learner code.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run() =>
    [
        Check("7 of 20 failed -> 35", true, 7, 20, 35.0, "Divide as double before multiplying: (double)failed / total * 100."),
        Check("1 of 4 failed -> 25", true, 1, 4, 25.0, "Integer division drops the decimals. Convert one operand to double."),
        Check("1 of 3 failed", false, 1, 3, 100.0 / 3, "The result must keep its decimals (33.33...)."),
        Check("No attempts", false, 0, 0, 0.0, "With 0 attempts there is no rate - return 0 instead of dividing."),
    ];

    private static (string, bool, bool, string) Check(string name, bool visible, int failed, int total, double expected, string hint)
    {
        try
        {
            var actual = LoginStats.FailureRate(failed, total);
            var passed = Math.Abs(actual - expected) < 1e-9;
            return (name, visible, passed, passed ? "" : hint);
        }
        catch (Exception ex)
        {
            return (name, visible, false, $"Threw {ex.GetType().Name}: {ex.Message}");
        }
    }
}
