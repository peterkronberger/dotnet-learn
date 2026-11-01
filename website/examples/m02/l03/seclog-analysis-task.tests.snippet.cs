// Test code for seclog-analysis-task.cs (m02-l03). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 26 lines are printed", true, lines.Length == 26, "The output has 26 lines (an empty line does not count). Add exactly one new line at the end."),
            ("Earlier lines stay unchanged", true, lines.Length > 24 && lines[0] == "SecLog project profile" && lines[11] == "User: bob" && lines[20] == "Failure rate from input (%): 35.7" && lines[23] == "Alert: many failed logins" && lines[24] == "Suspicious: True", "The first 25 lines must stay as they are."),
            ("Plural word for 5 failures", true, lines.Length > 25 && lines[25] == "5 failed attempts", "The last line must be: 5 failed attempts. Choose the word attempt or attempts with the conditional operator."),
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
