// Test code for seclog-linq-task.cs (m11-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 283 lines are printed", true, lines.Length == 283, "The output has 283 lines (an empty line does not count). The block prints 5 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 277 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[257] == "Failed logins, newest first" && lines[269] == "Events in the log: 14" && lines[277].StartsWith("- 2026-03-02T09:30:00Z logout user=alice"), "All earlier lines must stay as they are."),
            ("The query counts 2 failures at first", true, lines.Length > 278 && lines[278] == "Failed logins in the query: 2", "liveLog holds the first 4 lines of the sample log. 2 of them are login-failed."),
            ("The query sees the added line", true, lines.Length > 279 && lines[279] == "Failed logins after adding a line: 3", "The query runs again on every Count(), so it sees sampleLog[4] (a failure of bob)."),
            ("The snapshot keeps the old result", true, lines.Length > 280 && lines[280] == "Failed logins in the snapshot: 3", "ToList() runs the query at once. The list does not change when liveLog changes later."),
            ("The query sees the second added line", true, lines.Length > 281 && lines[281] == "Failed logins in the query now: 4", "sampleLog[5] is another failure of bob, so the query counts 4."),
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
