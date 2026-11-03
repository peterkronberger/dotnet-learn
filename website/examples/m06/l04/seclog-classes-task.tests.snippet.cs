// Test code for seclog-classes-task.cs (m06-l04). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 122 lines are printed", true, lines.Length == 122, "The output has 122 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 119 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[118] == "Incident bob: 5 failed logins" && lines[119] == "Rejected events: 9", "All earlier lines must stay as they are. Summary() must still print the number of failed logins."),
            ("Status accepts a known value", true, lines.Length > 120 && lines[120] == "Status: investigating", "Set Status to \"investigating\" and print it. The set accessor accepts open, investigating and closed."),
            ("Status keeps the old value", true, lines.Length > 121 && lines[121] == "Status: investigating", "Set Status to \"unknown\" and print it. The set accessor ignores values that are not allowed, so the status stays investigating."),
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
