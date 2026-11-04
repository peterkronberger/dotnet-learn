// Test code for seclog-exceptions-task.cs (m07-l02). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 137 lines are printed", true, lines.Length == 137, "The output has 137 lines (an empty line does not count). The new block prints 3 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 133 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[118] == "Incident #1 bob: 5 failed logins (sample log)" && lines[128] == "Incidents: 2" && lines[129] == "SecLog.Model.LogEvent" && lines[130] == "Analyzed log: sample log" && lines[131] == "Failure count: 7" && lines[132] == "Not a number: seven", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("Line 2 is printed", true, lines.Length > 134 && lines[134] == "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7", "The text \"2\" is number 2. Print sampleLog[number - 1]."),
            ("Wrong line number", true, lines.Length > 135 && lines[135] == "No line 15 in the sample log", "Catch IndexOutOfRangeException and print No line 15 in the sample log."),
            ("Text that is not a number", true, lines.Length > 136 && lines[136] == "Not a line number: two", "Catch FormatException in a second clause and print Not a line number: two."),
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
