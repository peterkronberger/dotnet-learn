// Test code for seclog-delegates-task.cs (m10-l02). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 229 lines are printed", true, lines.Length == 229, "The output has 229 lines (an empty line does not count). The new block prints 3 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 225 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[198] == "Failures of carol: 0" && lines[218] == "Interface count: 2" && lines[225] == "Under investigation: 2", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("ReportMatches reports through Console.WriteLine", true, lines.Length > 226 && lines[226] == "many failures: Incident #1 bob: 5 failed logins (sample log)", "ReportMatches(incidents, rule, Console.WriteLine) calls report with \"{rule.Name}: {incident.Summary()}\" for each matching incident."),
            ("FuncRule objects work in a Detector", true, lines.Length > 228 && lines[227] == "bob: 3 matches" && lines[228] == "dave: 3 matches", "Put two FuncRule objects and the rules WatchListRule(\"dave\") and StatusRule into one Detector and print CountMatches for each incident."),
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
