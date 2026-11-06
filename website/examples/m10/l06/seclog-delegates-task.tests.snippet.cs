// Test code for seclog-delegates-task.cs (m10-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 247 lines are printed", true, lines.Length == 247, "The output has 247 lines (an empty line does not count). The new block prints 6 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 240 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[198] == "Failures of carol: 0" && lines[228] == "dave: 3 matches" && lines[232] == "Incident #2 dave: 1 failed logins (sample log)" && lines[240] == "Checked incidents: 2", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("PrintAlert runs first", true, lines.Length > 241 && lines[241] == "ALERT: Incident #1 bob: 5 failed logins (sample log)", "Invoke alerts for bobIncident: PrintAlert runs first."),
            ("PrintSeverity runs second", true, lines.Length > 242 && lines[242] == "Severity: High", "PrintSeverity was added second and prints the severity of bob."),
            ("Three targets before the removal", true, lines.Length > 243 && lines[243] == "Targets: 3", "Print alerts.GetInvocationList().Length: three targets (two methods and the lambda)."),
            ("Only the remaining targets run for dave", true, lines.Length > 244 && lines[244] == "ALERT: Incident #2 dave: 1 failed logins (sample log)", "After alerts -= PrintSeverity only PrintAlert and the lambda run for daveIncident."),
            ("Two targets after the removal", true, lines.Length > 245 && lines[245] == "Targets: 2", "Print the number of targets again after the removal."),
            ("The lambda counted both invocations", true, lines.Length > 246 && lines[246] == "Alerts counted: 2", "The lambda increments the captured alertCount once per invocation."),
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
