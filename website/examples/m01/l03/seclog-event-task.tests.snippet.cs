// Test code for seclog-event-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 14 lines are printed", true, lines.Length == 14, "The program should print the 8 profile lines, the heading, the event line, the 3 labeled lines and the failure rate (an empty line does not count)."),
            ("Earlier lines stay unchanged", true, lines.Length > 12 && lines[0] == "SecLog project profile" && lines[7] == "Execution model: Native AOT" && lines[8] == "First event:" && lines[9] == "2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10" && lines[10] == "User: bob" && lines[11] == "IP: 203.0.113.7" && lines[12] == "Event type: login-failed", "The profile, the first event and the three labeled lines must stay as they are."),
            ("Failure rate in percent", true, lines.Length > 13 && lines[13] == "Failure rate (%): 50", "Line 14 must be: Failure rate (%): 50. Use a double operand, because int / int drops the fraction."),
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
