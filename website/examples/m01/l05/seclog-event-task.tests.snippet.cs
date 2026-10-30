// Test code for seclog-event-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 16 lines are printed", true, lines.Length == 16, "The output must stay the same: 16 lines (an empty line does not count). Changing const and var must not add or remove output."),
            ("Earlier lines stay unchanged", true, lines.Length > 13 && lines[0] == "SecLog project profile" && lines[7] == "Execution model: Native AOT" && lines[8] == "First event:" && lines[9] == "2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10" && lines[10] == "User: bob" && lines[11] == "IP: 203.0.113.7" && lines[12] == "Event type: login-failed" && lines[13] == "Failure rate (%): 50", "The profile, the first event, the three labeled lines and the failure rate must stay as they are. Check that 14 and 7 still give 50."),
            ("Failed logins of bob", true, lines.Length > 14 && lines[14] == "Failed logins of bob: 5", "Line 15 must be: Failed logins of bob: 5. If you declared the counter with var, start it with the int literal 0."),
            ("Share of bob in percent", true, lines.Length > 15 && lines[15] == "Share of bob (%): 71.43", "Line 16 must be: Share of bob (%): 71.43. A var for this value must get a double from its initializer."),
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
