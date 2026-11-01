// Test code for seclog-analysis-task.cs. Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 27 lines are printed", true, lines.Length == 27, "The output has 27 lines (an empty line does not count). Add exactly one line after the old output."),
            ("Earlier lines stay unchanged", true, lines.Length > 25 && lines[0] == "SecLog project profile" && lines[23] == "Alert: many failed logins" && lines[24] == "Suspicious: True" && lines[25] == "Watched user: True", "The first 26 lines must stay as they are."),
            ("Word matches the number", true, lines.Length > 26 && lines[26] == "5 failed attempts", "Line 27 must be: 5 failed attempts. Choose \"attempt\" or \"attempts\" with the conditional operator: failedFromInput == 1 ? \"attempt\" : \"attempts\"."),
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
