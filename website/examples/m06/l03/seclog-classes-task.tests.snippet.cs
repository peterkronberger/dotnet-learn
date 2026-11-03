// Test code for seclog-classes-task.cs (m06-l03). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 120 lines are printed", true, lines.Length == 120, "The output has 120 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 117 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[116] == "Events: 14" && lines[117] == "2026-03-02 09:30 logout alice", "All earlier lines must stay as they are."),
            ("Summary of the incident", true, lines.Length > 118 && lines[118] == "Incident bob: 5 failed logins", "Add every event to the incident of bob. Add accepts only failed logins of the same user. Print Summary()."),
            ("Rejected events", true, lines.Length > 119 && lines[119] == "Rejected events: 9", "Count the events for which Add returns false. 14 events minus 5 failed logins of bob is 9."),
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
