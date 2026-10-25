// Test code for seclog-banner-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Two lines are printed", true, lines.Length == 2, "The program should print exactly two lines."),
            ("Banner comes first", true, lines.Length > 0 && lines[0] == "SecLog 0.1", "The first line must be the banner: SecLog 0.1"),
            ("Log line comes second", true, lines.Length > 1 && lines[1] == "2026-01-01 08:00 login failed user=alice", "The second line must be the log line."),
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
