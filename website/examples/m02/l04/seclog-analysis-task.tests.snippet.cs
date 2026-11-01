// Test code for seclog-analysis-task.cs. Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 29 lines are printed", true, lines.Length == 29, "The output has 29 lines (an empty line does not count). Add exactly one line after the old output."),
            ("Earlier lines stay unchanged", true, lines.Length > 27 && lines[0] == "SecLog project profile" && lines[23] == "Alert: many failed logins" && lines[26] == "Watched user: True" && lines[27] == "5 failed attempts", "The first 28 lines must stay as they are."),
            ("Severity for 5 failures", true, lines.Length > 28 && lines[28] == "Severity: high", "Line 29 must be: Severity: high. Use a switch expression on failedFromInput with the arms 0 => none, < 3 => low, < 5 => medium and _ => high."),
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
