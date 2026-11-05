// Test code for seclog-exceptions-task.cs (m07-l08). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 148 lines are printed", true, lines.Length == 148, "The output has 148 lines (an empty line does not count). The new block prints 1 new line."),
            ("Earlier lines stay unchanged", true, lines.Length > 140 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[121] == "Unknown status: unknown" && lines[139] == "Processed: 2, rejected: 1" && lines[145] == "Accepted lines: 1", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("Good and bad lines are counted", true, lines.Length > 146 && lines[146] == "Good lines: 2, bad lines: 2", "Keep the block that counts good and bad lines."),
            ("The sample log is read line by line", true, lines.Length > 147 && lines[147] == "Lines read: 14, events: 14", "Read the lines with ReadLine() until it returns null, parse each with TryParseLine, and print Lines read: 14, events: 14."),
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
