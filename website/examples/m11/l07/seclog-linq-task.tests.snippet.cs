// Test code for seclog-linq-task.cs (m11-l07). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 296 lines are printed", true, lines.Length == 296, "The output has 296 lines (an empty line does not count). The block prints 6 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 289 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[281] == "Failed logins in the query now: 4" && lines[289] == "Same rows: True", "All earlier lines must stay as they are."),
            ("Failures per hour are printed", true, lines.Length > 291 && lines[290] == "08:00 6 failures" && lines[291] == "09:00 1 failures", "Group the 7 failures by e.Timestamp.Hour and sort the groups by Key. 6 failures are in hour 8, 1 in hour 9."),
            ("The top 3 IP addresses are printed", true, lines.Length > 294 && lines[292] == "203.0.113.7 5" && lines[293] == "192.0.2.10 4" && lines[294] == "198.51.100.8 3", "Group the IP addresses of all 14 lines, sort by Count() descending, then by Key, and take 3."),
            ("The incident with most failures is printed", true, lines.Length > 295 && lines[295] == "Incident #1 bob: 5 failed logins (sample log)", "incidents.MaxBy(i => i.FailureCount)?.Summary() gives the incident of bob."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement."),
        ];
    }

    private static bool EntryPointReturnsInt()
    {
        return typeof(TaskTests).Assembly.EntryPoint!.ReturnType == typeof(int);
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
