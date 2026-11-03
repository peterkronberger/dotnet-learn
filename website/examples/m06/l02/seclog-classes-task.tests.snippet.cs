// Test code for seclog-classes-task.cs (m06-l02). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 118 lines are printed", true, lines.Length == 118, "The output has 118 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 115 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[114] == "Time span: 00:05:08" && lines[115] == "2026-03-02 08:01 login-failed bob", "All earlier lines must stay as they are. The first block now creates its LogEvent with the constructor."),
            ("Number of events", true, lines.Length > 116 && lines[116] == "Events: 14", "Add one LogEvent per line of sampleLog to the list and print its Count."),
            ("Last event", true, lines.Length > 117 && lines[117] == "2026-03-02 09:30 logout alice", "Print Describe() of the last element of the list."),
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
