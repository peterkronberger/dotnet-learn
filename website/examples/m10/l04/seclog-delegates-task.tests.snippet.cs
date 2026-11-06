// Test code for seclog-delegates-task.cs (m10-l04). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 235 lines are printed", true, lines.Length == 235, "The output has 235 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 232 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[198] == "Failures of carol: 0" && lines[228] == "dave: 3 matches" && lines[232] == "Incident #2 dave: 1 failed logins (sample log)", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The default level is used for bob", true, lines.Length > 233 && lines[233] == "[ALERT] Incident #1 bob: 5 failed logins (sample log)", "Call the lambda with only the incident. The optional parameter level defaults to \"ALERT\"."),
            ("The level is given for dave", true, lines.Length > 234 && lines[234] == "[INFO] Incident #2 dave: 1 failed logins (sample log)", "Call the lambda with daveIncident and the level \"INFO\"."),
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
