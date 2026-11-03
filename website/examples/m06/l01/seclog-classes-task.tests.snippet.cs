// Test code for seclog-classes-task.cs (m06-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 116 lines are printed", true, lines.Length == 116, "The output has 116 lines (an empty line does not count). The new block prints 1 new line."),
            ("Earlier lines stay unchanged", true, lines.Length > 114 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[113] == "bob: 5 failed logins within 10 minutes (08:01:40 to 08:06:48)" && lines[114] == "Time span: 00:05:08", "All earlier lines must stay as they are."),
            ("The event is described", true, lines.Length > 115 && lines[115] == "2026-03-02 08:01 login-failed bob", "Create the object with new LogEvent(), set the three fields from sampleLog[1] and print Describe(). The format is yyyy-MM-dd HH:mm."),
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
