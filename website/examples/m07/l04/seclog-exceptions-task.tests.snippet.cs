// Test code for seclog-exceptions-task.cs (m07-l04). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 139 lines are printed", true, lines.Length == 139, "The output has 139 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 136 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[132] == "Not a number: seven" && lines[134] == "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7" && lines[136] == "Not a line number: two", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("Rejected line is printed", true, lines.Length > 137 && lines[137] == "Rejected: broken line", "Catch FormatException around TimestampOf(line) and print Rejected: broken line."),
            ("Both lines are counted", true, lines.Length > 138 && lines[138] == "Processed: 2, rejected: 1", "Count every processed line in the finally block and every rejected line in the catch clause. Print Processed: 2, rejected: 1."),
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
