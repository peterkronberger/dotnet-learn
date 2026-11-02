// Test code for seclog-analysis-task.cs (m02-l07). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 35 lines are printed", true, lines.Length == 35, "The output has 35 lines (an empty line does not count). Add exactly six new lines at the end."),
            ("Earlier lines stay unchanged", true, lines.Length > 28 && lines[0] == "SecLog project profile" && lines[11] == "User: bob" && lines[26] == "Severity: high" && lines[28] == "Failure streak: 5", "The first 29 lines must stay as they are."),
            ("Five attempt lines", true, lines.Length > 33 && lines[29] == "Attempt 1" && lines[30] == "Attempt 2" && lines[31] == "Attempt 3" && lines[32] == "Attempt 4" && lines[33] == "Attempt 5", "After the failure streak print Attempt 1 to Attempt 5, one per failed login of bob. Use a for loop up to bobFailures."),
            ("Dots in the IP address", true, lines.Length > 34 && lines[34] == "Dots in IP: 3", "The last line must be: Dots in IP: 3. Count the dots with foreach over ip."),
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
