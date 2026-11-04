// Test code for seclog-classes-task.cs (m06-l07). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 128 lines are printed", true, lines.Length == 128, "The output has 128 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 125 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[118] == "Incident bob: 5 failed logins (sample log)" && lines[122] == "Incident dave: 1 failed logins (sample log)" && lines[124] == "Source of bob: sample log" && lines[125] == "Source of dave: sample log", "All other earlier lines must stay as they are. Incident keeps its behavior when it gets a primary constructor."),
            ("User of bob", true, lines.Length > 126 && lines[126] == "User of bob: bob", "Add public string User => user; to Incident and print it for bobIncident in the new block."),
            ("User of dave", true, lines.Length > 127 && lines[127] == "User of dave: dave", "Print the User of daveIncident after the line for bob."),
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
