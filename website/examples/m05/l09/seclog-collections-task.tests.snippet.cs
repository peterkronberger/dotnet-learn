// Test code for seclog-collections-task.cs (m05-l09). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 108 lines are printed", true, lines.Length == 108, "The output has 108 lines (an empty line does not count). The new block prints 5 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 102 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[101] == "Watch bob: 5" && lines[102] == "Watch dave: 1", "All earlier lines must stay as they are."),
            ("First alert", true, lines.Length > 103 && lines[103] == "Alert: 2026-03-02T08:01:40Z", "Take the timestamp with line[..20] (20 characters)."),
            ("Order is kept", true, lines.Length > 107 && lines[104] == "Alert: 2026-03-02T08:02:05Z" && lines[105] == "Alert: 2026-03-02T08:04:02Z" && lines[106] == "Alert: 2026-03-02T08:05:15Z" && lines[107] == "Alert: 2026-03-02T08:06:48Z", "A queue gives the lines back in the order they were added: use Enqueue and Dequeue."),
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
