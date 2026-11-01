// Test code for seclog-analysis-task.cs (m02-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 28 lines are printed", true, lines.Length == 28, "The output has 28 lines (an empty line does not count). Add exactly one new line at the end."),
            ("Earlier lines stay unchanged", true, lines.Length > 26 && lines[0] == "SecLog project profile" && lines[11] == "User: bob" && lines[20] == "Failure rate from input (%): 35.7" && lines[24] == "Suspicious: True" && lines[25] == "5 failed attempts" && lines[26] == "Severity: high", "The first 27 lines must stay as they are."),
            ("Action for login-failed", true, lines.Length > 27 && lines[27] == "Action: count the failure", "The last line must be: Action: count the failure. Use a switch statement on eventType."),
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
