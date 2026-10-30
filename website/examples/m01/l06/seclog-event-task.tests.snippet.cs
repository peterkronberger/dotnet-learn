// Test code for seclog-event-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 18 lines are printed", true, lines.Length == 18, "The output has 18 lines (an empty line does not count): the 16 old lines and 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 15 && lines[0] == "SecLog project profile" && lines[7] == "Execution model: Native AOT" && lines[8] == "First event:" && lines[9] == "2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10" && lines[10] == "User: bob" && lines[11] == "IP: 203.0.113.7" && lines[12] == "Event type: login-failed" && lines[13] == "Failure rate (%): 50" && lines[14] == "Failed logins of bob: 5" && lines[15] == "Share of bob (%): 71.43", "The first 16 lines must stay as they are. Add your new lines at the end."),
            ("Event as one log line", true, lines.Length > 16 && lines[16] == "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7", "Line 17 must be: 2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7. Build it with an interpolated string from the variables eventType, user and ip."),
            ("Failure rate with one decimal place", true, lines.Length > 17 && lines[17] == "Failure rate: 50.0 %", "Line 18 must be: Failure rate: 50.0 %. Use the format {failureRate:F1} inside the interpolated string."),
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
