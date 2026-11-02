// Test code for seclog-methods-task.cs (m03-l07). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 50 lines are printed", true, lines.Length == 50, "The output has 50 lines (an empty line does not count). Add exactly one new line: the call of your counting method."),
            ("Earlier lines stay unchanged", true, lines.Length > 49 && lines[0] == "SecLog project profile" && lines[42] == "Failure rate (%): 50.0" && lines[44] == "2026-03-02T08:42:11Z logout user=carol ip=unknown" && lines[49] == "dave |#", "All earlier lines must stay as they are."),
            ("Failures counted", true, lines.Length > 45 && lines[45] == "Failed in sample: 7", "Call CountFailed with the 14 event types of the sample log, one argument per event, and print 'Failed in sample: ' with the result right after the two FormatEvent lines."),
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
