// Test code for seclog-classes-task.cs (m06-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 124 lines are printed", true, lines.Length == 124, "The output has 124 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 121 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[118] == "Incident bob: 5 failed logins" && lines[121] == "Status: investigating", "All earlier lines must stay as they are. The events are still created for all 14 lines of the sample log."),
            ("Summary of the incident of dave", true, lines.Length > 122 && lines[122] == "Incident dave: 1 failed logins", "Create the incident with new(\"dave\") and the object initializer, then add every event. Only the failed login of dave is accepted."),
            ("Status from the initializer", true, lines.Length > 123 && lines[123] == "Status: investigating", "The object initializer sets Status to \"investigating\" right after the constructor. Print Status after the summary."),
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
