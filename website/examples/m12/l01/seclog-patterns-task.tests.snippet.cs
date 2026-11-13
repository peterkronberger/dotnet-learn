// Test code for seclog-patterns-task.cs (m12-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 328 lines are printed", true, lines.Length == 328, "The output has 328 lines (an empty line does not count). The block prints 7 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 320 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[320] == "Same rows as method syntax: True", "All earlier lines must stay as they are."),
            ("Log event and security event are described", true, lines.Length > 322 && lines[321] == "log event: 2026-03-02 08:00 login-success alice" && lines[322] == "security event of bob", "Use the patterns LogEvent e and SecurityEvent s. The text of a log event comes from e.Describe()."),
            ("String, null and other types are described", true, lines.Length > 325 && lines[323].StartsWith("raw line: 2026-03-02T08:02:05Z") && lines[324] == "missing" && lines[325] == "unknown item", "Use string line, the constant pattern null and the discard _."),
            ("is string is True for a string and False for null", true, lines.Length > 327 && lines[326] == "mixedItems[2] is string: True" && lines[327] == "mixedItems[3] is string: False", "A type pattern never matches null."),
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
