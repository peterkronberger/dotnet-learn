// Test code for seclog-oop-task.cs (m08-l11). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 183 lines are printed", true, lines.Length == 183, "The output has 183 lines (an empty line does not count). The new block prints 3 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 179 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[154] == "As text: Rule: failed logins (threshold 5)" && lines[176] == "Distinct events: 2" && lines[177] == "As record: SecurityEvent { Timestamp = 03/02/2026 08:01:40 +00:00, Type = LoginFailed, User = bob }" && lines[179] == "bob: LoginFailed", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The incident with more failures comes first", true, lines.Length > 180 && lines[180] == "Incident #1 bob: 5 failed logins (sample log)", "Sort the list with incidents.Sort(new IncidentComparer()) and print Summary() for each incident in list order."),
            ("The incident with fewer failures comes second", true, lines.Length > 181 && lines[181] == "Incident #2 dave: 1 failed logins (sample log)", "Compare the failure counts so that more failures come first."),
            ("The first incident is rated by its severity", true, lines.Length > 182 && lines[182] == "Severity of the first: High", "Print SeverityOf(...) of the FailureCount of the first incident in the sorted list."),
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
