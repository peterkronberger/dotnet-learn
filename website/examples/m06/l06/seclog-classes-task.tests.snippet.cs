// Test code for seclog-classes-task.cs (m06-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 126 lines are printed", true, lines.Length == 126, "The output has 126 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Summary names the source", true, lines.Length > 123 && lines[118] == "Incident bob: 5 failed logins (sample log)" && lines[122] == "Incident dave: 1 failed logins (sample log)", "Summary() must end with the source in parentheses. Set Source = \"sample log\" in the object initializer of both incidents."),
            ("Earlier lines stay unchanged", true, lines.Length > 123 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[119] == "Rejected events: 9" && lines[121] == "Status: investigating" && lines[123] == "Status: investigating", "All other earlier lines must stay as they are. The status behaves as before."),
            ("Source of bob", true, lines.Length > 124 && lines[124] == "Source of bob: sample log", "Print the Source of bobIncident in the new block."),
            ("Source of dave", true, lines.Length > 125 && lines[125] == "Source of dave: sample log", "Print the Source of daveIncident after the line for bob."),
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
