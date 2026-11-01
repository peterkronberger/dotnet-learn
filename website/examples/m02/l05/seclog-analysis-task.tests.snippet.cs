// Test code for seclog-analysis-task.cs. Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 30 lines are printed", true, lines.Length == 30, "The output has 30 lines (an empty line does not count). Add exactly one line after the old output."),
            ("Earlier lines stay unchanged", true, lines.Length > 28 && lines[0] == "SecLog project profile" && lines[23] == "Alert: many failed logins" && lines[27] == "5 failed attempts" && lines[28] == "Severity: high", "The first 29 lines must stay as they are."),
            ("Action for login-failed", true, lines.Length > 29 && lines[29] == "Action: count the failure", "Line 30 must be: Action: count the failure. Use switch (eventType) with a section for \"login-failed\", one for \"login-success\" and \"logout\", and default. End every section with break."),
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
