// Test code for seclog-oop-task.cs (m08-l09). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 177 lines are printed", true, lines.Length == 177, "The output has 177 lines (an empty line does not count). The new block prints 3 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 173 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[154] == "As text: Rule: failed logins (threshold 5)" && lines[166] == "Event type: LoginFailed" && lines[171] == "Critical parsed: False (None)" && lines[172] == "In the window: 5 of 5" && lines[173] == "Copy has the same start: True", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("== still compares references", true, lines.Length > 174 && lines[174] == "Same reference with ==: False", "Print eventA == eventB. LogEvent has no operator overload, so it stays False."),
            ("Equals compares the values", true, lines.Length > 175 && lines[175] == "Same values with Equals: True", "Override Equals so that events with the same Timestamp, EventType and User are equal."),
            ("The set holds two distinct events", true, lines.Length > 176 && lines[176] == "Distinct events: 2", "Override GetHashCode with HashCode.Combine so that HashSet<LogEvent> recognizes equal events."),
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
