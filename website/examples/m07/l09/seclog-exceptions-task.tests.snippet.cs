// Test code for seclog-exceptions-task.cs (m07-l09). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 150 lines are printed", true, lines.Length == 150, "The output has 150 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 140 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[121] == "Unknown status: unknown" && lines[139] == "Processed: 2, rejected: 1" && lines[145] == "Accepted lines: 1" && lines[147] == "Lines read: 14, events: 14", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The file is read and all events are counted", true, lines.Length > 148 && lines[148] == "Events from sample.log: 14", "Write sampleLog with File.WriteAllLines, read it with a StreamReader in using, parse each line with ParseLine, and print Events from sample.log: 14."),
            ("A missing file is caught", true, lines.Length > 149 && lines[149] == "File not found: missing.log", "Read missing.log with File.ReadAllLines and catch FileNotFoundException. Print only the file name."),
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
