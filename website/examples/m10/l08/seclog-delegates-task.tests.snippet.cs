// Test code for seclog-delegates-task.cs (m10-l08). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 251 lines are printed", true, lines.Length == 251, "The output has 251 lines (an empty line does not count). The block still prints 4 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 246 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[228] == "dave: 3 matches" && lines[240] == "Checked incidents: 2" && lines[245] == "Targets: 2" && lines[246] == "Alerts counted: 2", "All earlier lines must stay as they are."),
            ("The handler reports bob with the number of rules", true, lines.Length > 247 && lines[247] == "Incident #1 bob: 5 failed logins (sample log) (2 rules)", "The handler prints {e.Incident.Summary()} ({e.Matches} rules). The event data carries the incident and the match count."),
            ("The handler reports dave with the number of rules", true, lines.Length > 248 && lines[248] == "Incident #2 dave: 1 failed logins (sample log) (2 rules)", "Check raises the event once per matching incident, through OnIncidentFound."),
            ("Two incidents were counted", true, lines.Length > 249 && lines[249] == "Found: 2", "The second handler counts in a captured local variable."),
            ("No report after the unsubscribe, the counter still counts", true, lines.Length > 250 && lines[250] == "Found: 4", "After -= report nothing is printed, but the counting handler is still subscribed."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement."),
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
