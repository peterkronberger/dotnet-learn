// Test code for seclog-analysis-task.cs. Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 24 lines are printed", true, lines.Length == 24, "The output has 24 lines (an empty line does not count). Keep the number of lines and add exactly one line."),
            ("User comes from the first input line", true, lines.Length > 16 && lines[10] == "User: bob" && lines[16] == "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7", "Keep the preset input (bob) in the input field. The lines User: bob and the log line must use it."),
            ("Earlier lines stay unchanged", true, lines.Length > 22 && lines[0] == "SecLog project profile" && lines[7] == "Execution model: Native AOT" && lines[13] == "Failure rate (%): 50" && lines[17] == "Failure rate: 50.0 %" && lines[20] == "Failure rate from input (%): 35.7" && lines[21] == "Second text worked: False" && lines[22] == "Second text number: 0", "The first 23 lines must stay as they are."),
            ("Alert for 5 failed logins", true, lines.Length > 23 && lines[23] == "Alert: many failed logins", "Line 24 must be: Alert: many failed logins. Use if (failedFromInput >= 5) and print No alert in the else part."),
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
