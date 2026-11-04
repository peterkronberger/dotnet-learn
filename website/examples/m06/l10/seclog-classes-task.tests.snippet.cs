// Test code for seclog-classes-task.cs (m06-l10). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 131 lines are printed", true, lines.Length == 131, "The output has 131 lines (an empty line does not count). The new block prints 1 new line."),
            ("Earlier lines stay unchanged", true, lines.Length > 129 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[118] == "Incident #1 bob: 5 failed logins (sample log)" && lines[128] == "Incidents: 2" && lines[129] == "SecLog.Model.LogEvent", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("Name of the log", true, lines.Length > 130 && lines[130] == "Analyzed log: sample log", "Without arguments, args is empty. Then logName must be \"sample log\". Print it as Analyzed log: <name>."),
            ("Entry point returns an int", true, EntryPointReturnsInt(), "End the top-level statements with return 0; so the generated entry point returns an int."),
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
