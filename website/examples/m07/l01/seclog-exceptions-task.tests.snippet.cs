// Test code for seclog-exceptions-task.cs (m07-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 134 lines are printed", true, lines.Length == 134, "The output has 134 lines (an empty line does not count). The new block prints 3 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 130 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[118] == "Incident #1 bob: 5 failed logins (sample log)" && lines[128] == "Incidents: 2" && lines[129] == "SecLog.Model.LogEvent" && lines[130] == "Analyzed log: sample log", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("Good text is parsed", true, lines.Length > 131 && lines[131] == "Failure count: 7", "Parse \"7\" first and print Failure count: 7."),
            ("Bad text is caught", true, lines.Length > 133 && lines[132] == "Not a number: seven" && lines[133].Length > 0, "Parse badText in the try block and catch FormatException. Print Not a number: seven and then ex.Message. The line after the failing int.Parse must not be printed."),
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
