// Test code for seclog-analysis-task.cs (m02-l08). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 37 lines are printed", true, lines.Length == 37, "The output has 37 lines (an empty line does not count). Add exactly two new lines at the end."),
            ("Earlier lines stay unchanged", true, lines.Length > 34 && lines[0] == "SecLog project profile" && lines[11] == "User: bob" && lines[28] == "Failure streak: 5" && lines[29] == "Attempt 1" && lines[33] == "Attempt 5" && lines[34] == "Dots in IP: 3", "The first 35 lines must stay as they are."),
            ("Date part of the timestamp", true, lines.Length > 35 && lines[35] == "Date: 2026-03-02", "Line 36 must be: Date: 2026-03-02. Use foreach over timestamp and break at the letter T."),
            ("Digits of the IP address", true, lines.Length > 36 && lines[36] == "Digits in IP: 8", "The last line must be: Digits in IP: 8. Use foreach over ip and continue at a dot."),
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
