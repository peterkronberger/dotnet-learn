// Test code for seclog-analysis-task.cs (m02-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 29 lines are printed", true, lines.Length == 29, "The output has 29 lines (an empty line does not count). Add exactly one new line at the end."),
            ("Earlier lines stay unchanged", true, lines.Length > 27 && lines[0] == "SecLog project profile" && lines[11] == "User: bob" && lines[20] == "Failure rate from input (%): 35.7" && lines[25] == "5 failed attempts" && lines[26] == "Severity: high" && lines[27] == "Action: count the failure", "The first 28 lines must stay as they are."),
            ("Failure streak of 5", true, lines.Length > 28 && lines[28] == "Failure streak: 5", "The last line must be: Failure streak: 5. Read lines with a while loop as long as the line is login-failed."),
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
