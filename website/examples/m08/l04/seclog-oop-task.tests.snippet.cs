// Test code for seclog-oop-task.cs (m08-l04). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 162 lines are printed", true, lines.Length == 162, "The output has 162 lines (an empty line does not count). The new block prints 3 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 160 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[121] == "Unknown status: unknown" && lines[145] == "Accepted lines: 1" && lines[148] == "Events from sample.log: 14" && lines[149] == "File not found: missing.log" && lines[150] == "Rule: failed logins (threshold 5)" && lines[153] == "Rule: failed logins (threshold 5)" && lines[154] == "As text: Rule: failed logins (threshold 5)" && lines[155] == "failed logins / bob: True" && lines[158] == "watch list / dave: True", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("FailedLoginRule is sealed", true, lines.Length > 159 && lines[159] == "FailedLoginRule sealed: True", "Mark FailedLoginRule with sealed and print typeof(FailedLoginRule).IsSealed."),
            ("WatchListRule is sealed", true, lines.Length > 160 && lines[160] == "WatchListRule sealed: True", "Mark WatchListRule with sealed and print typeof(WatchListRule).IsSealed."),
            ("DetectionRule is not sealed", true, lines.Length > 161 && lines[161] == "DetectionRule sealed: False", "DetectionRule is the base class, so it stays unsealed."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement. Add the new block before it."),
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
