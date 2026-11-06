// Test code for seclog-generics-task.cs (m09-l02). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 192 lines are printed", true, lines.Length == 192, "The output has 192 lines (an empty line does not count). The new block prints 6 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 185 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[183] == "login-failed texts: 7" && lines[185] == "Same events: 1", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The three stores count their items", true, lines.Length > 188 && lines[186] == "Stored events: 14" && lines[187] == "Stored incidents: 2" && lines[188] == "Stored rules: 3", "Create Store<LogEvent>, Store<Incident> and Store<IDetectionRule>. Fill each one with Add and print its Count."),
            ("The last event is read from its store", true, lines.Length > 189 && lines[189] == "Last event: 2026-03-02 09:30 logout alice", "Print Get(Count - 1) of the event store with Describe()."),
            ("The first incident is read through Items", true, lines.Length > 190 && lines[190] == "First incident: Incident #1 bob: 5 failed logins (sample log)", "Print Summary() of the first item of Items of the incident store."),
            ("The first rule is read from its store", true, lines.Length > 191 && lines[191] == "First rule: failed logins", "Print the Name of Get(0) of the rule store."),
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
