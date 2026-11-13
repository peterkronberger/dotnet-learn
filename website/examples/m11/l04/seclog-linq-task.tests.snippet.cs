// Test code for seclog-linq-task.cs (m11-l04). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 278 lines are printed", true, lines.Length == 278, "The output has 278 lines (an empty line does not count). The block prints 8 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 269 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[257] == "Failed logins, newest first" && lines[269] == "Events in the log: 14", "All earlier lines must stay as they are."),
            ("bob's first 3 lines are printed", true, lines.Length > 272 && lines[270] == "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7" && lines[271] == "2026-03-02T08:02:05Z login-failed user=bob ip=203.0.113.7" && lines[272] == "2026-03-02T08:04:02Z login-failed user=bob ip=203.0.113.7", "LinesOfUser(sampleLog, \"bob\", 3) gives sample log lines 2, 3 and 5. bob has 5 lines, so yield break must stop the iterator after 3."),
            ("The title for alice is printed", true, lines.Length > 273 && lines[273] == "Lines of alice", "PrintEach prints the title first."),
            ("All 4 lines of alice are printed", true, lines.Length > 277 && lines[274].StartsWith("- 2026-03-02T08:00:12Z login-success user=alice") && lines[277].StartsWith("- 2026-03-02T09:30:00Z logout user=alice"), "alice has 4 lines in the sample log; maxCount 10 is not reached."),
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
