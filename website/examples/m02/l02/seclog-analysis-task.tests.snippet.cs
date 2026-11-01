// Test code for seclog-analysis-task.cs (m02-l02). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 25 lines are printed", true, lines.Length == 25, "The output has 25 lines (an empty line does not count). Add exactly one new line at the end."),
            ("Earlier lines stay unchanged", true, lines.Length > 23 && lines[0] == "SecLog project profile" && lines[11] == "User: bob" && lines[20] == "Failure rate from input (%): 35.7" && lines[22] == "Second text number: 0" && lines[23] == "Alert: many failed logins", "The first 24 lines must stay as they are."),
            ("Suspicious for login-failed and 5 failures", true, lines.Length > 24 && lines[24] == "Suspicious: True", "The last line must be: Suspicious: True. Combine eventType == \"login-failed\" and failedFromInput >= 3 with &&."),
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
