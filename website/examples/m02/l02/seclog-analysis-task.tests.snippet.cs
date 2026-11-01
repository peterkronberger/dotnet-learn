// Test code for seclog-analysis-task.cs. Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 26 lines are printed", true, lines.Length == 26, "The output has 26 lines (an empty line does not count). Add exactly two lines after the old output."),
            ("Earlier lines stay unchanged", true, lines.Length > 23 && lines[0] == "SecLog project profile" && lines[20] == "Failure rate from input (%): 35.7" && lines[23] == "Alert: many failed logins", "The first 24 lines must stay as they are."),
            ("Suspicious event", true, lines.Length > 24 && lines[24] == "Suspicious: True", "Line 25 must be: Suspicious: True. Combine eventType == \"login-failed\" and failedFromInput >= 3 with &&."),
            ("Watched user", true, lines.Length > 25 && lines[25] == "Watched user: True", "Line 26 must be: Watched user: True. Combine user == \"bob\" and user == \"dave\" with ||."),
        ];
    }

    private static string[] RunProgram()
    {
        var original = Console.Out;
        var writer = new System.IO.StringWriter();
        Console.SetOut(writer);
        try
        {
            typeof(TaskTests).Assembly.EntryPoint!.Invoke(null, new object[] { new string[0] });
        }
        catch (Exception)
        {
            return [];
        }
        finally
        {
            Console.SetOut(original);
        }
        return writer.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
