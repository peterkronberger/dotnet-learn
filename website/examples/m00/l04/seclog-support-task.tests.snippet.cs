// Test code for seclog-support-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Four lines are printed", true, lines.Length == 4, "The program should print exactly four lines."),
            ("Check date", true, lines.Length > 2 && lines[2] == "Check date: 2026-12-01", "The third line must be: Check date: 2026-12-01"),
            ("Result", true, lines.Length > 3 && lines[3] == "Supported: False", "With this check date, the last line must be: Supported: False"),
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
