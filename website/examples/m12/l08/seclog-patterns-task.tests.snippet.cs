// Test code for seclog-patterns-task.cs (m12-l08). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 355 lines are printed", true, lines.Length == 355, "The output has 355 lines (an empty line does not count). The block prints 3 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 351 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[338] == "Lines parsed with list patterns: 14" && lines[351] == "Same after an empty with: True", "All earlier lines must stay as they are."),
            ("A valid line is an event", true, lines.Length > 352 && lines[352] == "event: LoginFailed of bob", "Parse(line) returns the SecurityEvent of ParseWithPatterns. Describe has an arm SecurityEvent securityEvent."),
            ("An invalid line is an error", true, lines.Length > 354 && lines[353] == "error: not a valid log line" && lines[354] == "error: not a valid log line", "If ParseWithPatterns returns null, return new LineError(line, \"not a valid log line\"). Describe has an arm LineError error."),
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
