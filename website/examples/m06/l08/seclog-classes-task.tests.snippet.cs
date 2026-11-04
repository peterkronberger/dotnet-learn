// Test code for seclog-classes-task.cs (m06-l08). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 129 lines are printed", true, lines.Length == 129, "The output has 129 lines (an empty line does not count). The new block prints 1 new line."),
            ("Summaries start with the number", true, lines.Length > 122 && lines[118] == "Incident #1 bob: 5 failed logins (sample log)" && lines[122] == "Incident #2 dave: 1 failed logins (sample log)", "Summary() must start with Incident #<Number>. The first incident gets 1, the second 2."),
            ("Earlier lines stay unchanged", true, lines.Length > 127 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[124] == "Source of bob: sample log" && lines[125] == "Source of dave: sample log" && lines[126] == "User of bob: bob" && lines[127] == "User of dave: dave", "All other earlier lines must stay as they are."),
            ("Count of incidents", true, lines.Length > 128 && lines[128] == "Incidents: 2", "Add a static field nextNumber and a static property Count to Incident. Print Incident.Count in the new block."),
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
