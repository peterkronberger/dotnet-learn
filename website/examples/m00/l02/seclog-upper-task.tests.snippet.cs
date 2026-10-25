// Test code for seclog-upper-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Two lines are printed", true, lines.Length == 2, "The program should print exactly two lines."),
            ("Upper case line", true, lines.Length > 0 && lines[0] == "LOGIN FAILED USER=ALICE", "The first line must be the log line in upper case."),
            ("Length line", true, lines.Length > 1 && lines[1] == "23", "The second line must be the number of characters of the log line: 23."),
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
