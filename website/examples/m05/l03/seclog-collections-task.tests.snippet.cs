// Test code for seclog-collections-task.cs (m05-l03). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 86 lines are printed", true, lines.Length == 86, "The output has 86 lines (an empty line does not count). The new block prints 10 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 75 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[75] == "dave | hour 8: 0 | hour 9: 1", "All earlier lines must stay as they are."),
            ("Last line with ^1", true, lines.Length > 76 && lines[76] == "Last line again: 2026-03-02T09:30:00Z logout user=alice ip=192.0.2.10", "Print sampleLog[^1]."),
            ("Lines 2 to 7", true, lines.Length > 83 && lines[77] == "Lines 2 to 7: 6" && lines[78] == "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7" && lines[83] == "2026-03-02T08:06:48Z login-failed user=bob ip=203.0.113.7", "Use sampleLog[1..7]: the start is included, the end is not."),
            ("Last three lines", true, lines.Length > 84 && lines[84] == "Last three lines: 3", "Use sampleLog[^3..]."),
            ("Date part of line 1", true, lines.Length > 85 && lines[85] == "Date of line 1: 2026-03-02", "Use the range [..10] on sampleLog[0]."),
        ];
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
