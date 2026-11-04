// Test code for seclog-classes-task.cs (m06-l09). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 130 lines are printed", true, lines.Length == 130, "The output has 130 lines (an empty line does not count). The new block prints 1 new line."),
            ("Earlier lines stay unchanged", true, lines.Length > 128 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[118] == "Incident #1 bob: 5 failed logins (sample log)" && lines[126] == "User of bob: bob" && lines[128] == "Incidents: 2", "All earlier lines must stay as they are. Put LogEvent and Incident into the namespace SecLog.Model and add using SecLog.Model; at the top."),
            ("Full name of the type", true, lines.Length > 129 && lines[129] == "SecLog.Model.LogEvent", "Print typeof(LogEvent).FullName in the new block. It is SecLog.Model.LogEvent only if LogEvent is declared in namespace SecLog.Model."),
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
