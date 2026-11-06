// Test code for seclog-delegates-task.cs (m10-l03). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 233 lines are printed", true, lines.Length == 233, "The output has 233 lines (an empty line does not count). The new block prints 4 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 228 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[198] == "Failures of carol: 0" && lines[225] == "Under investigation: 2" && lines[228] == "dave: 3 matches", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("Lambda rules work in a Detector", true, lines.Length > 230 && lines[229] == "bob: 2 lambda matches" && lines[230] == "dave: 2 lambda matches", "Create the rules with lambdas, put them with new StatusRule() into one Detector and print \"{User}: {CountMatches} lambda matches\" for each incident."),
            ("Incidents are sorted with Comparer<Incident>.Create", true, lines.Length > 232 && lines[231] == "Incident #1 bob: 5 failed logins (sample log)" && lines[232] == "Incident #2 dave: 1 failed logins (sample log)", "Sort a copy of incidents (most failures first, then User ordinal) and print the Summary() of each incident."),
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
