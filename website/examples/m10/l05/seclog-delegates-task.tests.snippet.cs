// Test code for seclog-delegates-task.cs (m10-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 241 lines are printed", true, lines.Length == 241, "The output has 241 lines (an empty line does not count). The new block prints 6 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 234 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[198] == "Failures of carol: 0" && lines[228] == "dave: 3 matches" && lines[232] == "Incident #2 dave: 1 failed logins (sample log)" && lines[234] == "[INFO] Incident #2 dave: 1 failed logins (sample log)", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("First rule is false for dave", true, lines.Length > 235 && lines[235] == "at least 2 / dave: False", "Build the rule with FailuresAtLeast(2) and print IsMatch for daveIncident: the incident has 1 failure."),
            ("First rule is true for bob", true, lines.Length > 236 && lines[236] == "at least 2 / bob: True", "Print IsMatch of the same rule for bobIncident (5 failures)."),
            ("Second rule is false for dave", true, lines.Length > 237 && lines[237] == "at least 5 / dave: False", "The second rule is FailuresAtLeast(5); print it for daveIncident."),
            ("Second rule is true for bob", true, lines.Length > 238 && lines[238] == "at least 5 / bob: True", "Print the second rule for bobIncident."),
            ("The static lambda rule is used", true, lines.Length > 239 && lines[239] == "no failures / dave: False", "Add the static lambda rule static incident => incident.FailureCount == 0 and print it for daveIncident."),
            ("The captured counter counts the checks", true, lines.Length > 240 && lines[240] == "Checked incidents: 2", "Increment checkedCount inside the lambda, run the rule over incidents and print checkedCount."),
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
