// Test code for seclog-patterns-task.cs (m12-l09). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 358 lines are printed", true, lines.Length == 358, "The output has 358 lines (an empty line does not count). The block prints 3 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 354 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[338] == "Lines parsed with list patterns: 14" && lines[351] == "Same after an empty with: True" && lines[354] == "error: not a valid log line", "All earlier lines must stay as they are."),
            ("An open status", true, lines.Length > 355 && lines[355] == "open: waiting for an analyst", "StatusLine has an arm OpenStatus => \"open: waiting for an analyst\"."),
            ("An investigating status", true, lines.Length > 356 && lines[356] == "investigating: analyst-1", "The arm InvestigatingStatus investigating uses investigating.Analyst."),
            ("A resolved status", true, lines.Length > 357 && lines[357] == "resolved: false alarm", "The arm ResolvedStatus resolved uses resolved.Note."),
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
