// Test code for seclog-linq-task.cs (m11-l03). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 270 lines are printed", true, lines.Length == 270, "The output has 270 lines (an empty line does not count). The block prints 13 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 256 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[254] == "Number | User | Failures | Severe" && lines[256] == "2 | dave | 1 | False", "All earlier lines must stay as they are."),
            ("The failures are listed newest first", true, lines.Length > 258 && lines[257] == "Failed logins, newest first" && lines[258] == "- 2026-03-02 09:10 login-failed dave" && lines[259] == "- 2026-03-02 08:15 login-failed alice", "Where keeps the failures, OrderByDescending puts the newest first, Select calls Describe()."),
            ("All 7 failures are listed", true, lines.Length > 264 && lines[264] == "- 2026-03-02 08:01 login-failed bob", "The chain must end with ToList(); bob has the oldest failure."),
            ("The first 3 events by user are printed", true, lines.Length > 268 && lines[265] == "First 3 events by user" && lines[266] == "- 2026-03-02 08:00 login-success alice" && lines[267] == "- 2026-03-02 08:15 login-failed alice" && lines[268] == "- 2026-03-02 08:15 login-success alice", "Sort by user with StringComparer.Ordinal, then by Timestamp, and Take(3)."),
            ("The list is not changed", true, lines.Length > 269 && lines[269] == "Events in the log: 14", "Where and Take do not remove events from logEvents."),
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
