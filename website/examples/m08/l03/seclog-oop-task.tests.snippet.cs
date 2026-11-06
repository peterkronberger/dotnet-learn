// Test code for seclog-oop-task.cs (m08-l03). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 159 lines are printed", true, lines.Length == 159, "The output has 159 lines (an empty line does not count). The new block prints 4 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 157 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[121] == "Unknown status: unknown" && lines[145] == "Accepted lines: 1" && lines[148] == "Events from sample.log: 14" && lines[149] == "File not found: missing.log" && lines[150] == "Rule: failed logins (threshold 5)" && lines[153] == "Rule: failed logins (threshold 5)" && lines[154] == "As text: Rule: failed logins (threshold 5)", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("failed logins / bob", true, lines.Length > 155 && lines[155] == "failed logins / bob: True", "FailedLoginRule.IsMatch returns incident.FailureCount >= Threshold. Bob has 5 failed logins."),
            ("failed logins / dave", true, lines.Length > 156 && lines[156] == "failed logins / dave: False", "Dave has fewer than 5 failed logins."),
            ("watch list / bob", true, lines.Length > 157 && lines[157] == "watch list / bob: False", "WatchListRule.IsMatch compares incident.User with WatchedUser (dave) using StringComparison.Ordinal."),
            ("watch list / dave", true, lines.Length > 158 && lines[158] == "watch list / dave: True", "The rule list holds failedRule and new WatchListRule(\"dave\"). Print rule.Name / incident.User: rule.IsMatch(incident)."),
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
