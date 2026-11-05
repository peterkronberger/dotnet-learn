// Test code for seclog-exceptions-task.cs (m07-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 141 lines are printed", true, lines.Length == 141, "The output has 141 lines (an empty line does not count). The status block now prints one more line, the new block prints 1 new line."),
            ("Earlier lines stay unchanged", true, lines.Length > 139 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[133] == "Not a number: seven" && lines[139] == "Processed: 2, rejected: 1", "All earlier lines must stay as they are. Change only the status block and the constructor, and add the new block after the earlier blocks."),
            ("Unknown status throws", true, lines.Length > 122 && lines[120] == "Status: investigating" && lines[121] == "Unknown status: unknown" && lines[122] == "Status: investigating", "Throw an ArgumentException with the message Unknown status: unknown in the Status setter, catch it in the status block and print its message. The status stays investigating."),
            ("Event without type is rejected", true, lines.Length > 140 && lines[140].StartsWith("Rejected event: "), "Call ArgumentException.ThrowIfNullOrEmpty(eventType) first in the LogEvent constructor. In the new block catch ArgumentException and print Rejected event: and the message."),
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
