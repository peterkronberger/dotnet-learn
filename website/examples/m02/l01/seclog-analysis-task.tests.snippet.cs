// Test code for seclog-analysis-task.cs (m02-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 24 lines are printed", true, lines.Length == 24, "The output has 24 lines (an empty line does not count). Add exactly one new line at the end."),
            ("Earlier lines stay unchanged", true, lines.Length > 22 && lines[0] == "SecLog project profile" && lines[9] == "First event:" && lines[11] == "User: bob" && lines[20] == "Failure rate from input (%): 35.7" && lines[21] == "Second text worked: False" && lines[22] == "Second text number: 0", "The first 23 lines must stay as they are."),
            ("Alert for 5 failed logins", true, lines.Length > 23 && lines[23] == "Alert: many failed logins", "The last line must be: Alert: many failed logins. Use if (failedFromInput >= 5)."),
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
