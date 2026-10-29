// Test code for seclog-event-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 13 lines are printed", true, lines.Length == 13, "The program should print the 8 profile lines, the heading, the event line and the 3 labeled lines (an empty line does not count)."),
            ("Earlier lines stay unchanged", true, lines.Length > 9 && lines[0] == "SecLog project profile" && lines[7] == "Execution model: Native AOT" && lines[8] == "First event:" && lines[9] == "2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10", "The profile, the heading and the first event must stay as they are."),
            ("New line: user", true, lines.Length > 10 && lines[10] == "User: bob", "Line 11 must be: User: bob"),
            ("New line: IP address", true, lines.Length > 11 && lines[11] == "IP: 203.0.113.7", "Line 12 must be: IP: 203.0.113.7"),
            ("New line: event type", true, lines.Length > 12 && lines[12] == "Event type: login-failed", "Line 13 must be: Event type: login-failed"),
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
