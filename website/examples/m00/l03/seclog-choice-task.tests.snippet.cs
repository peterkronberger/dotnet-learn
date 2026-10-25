// Test code for seclog-choice-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Three lines are printed", true, lines.Length == 3, "The program should print exactly three lines."),
            ("Targets", true, lines.Length > 1 && lines[0] == "Target: Linux server" && lines[1] == "Target: Windows laptop", "The first two lines name the two targets and stay as they are."),
            ("Choice", true, lines.Length > 2 && lines[2] == "Choice: .NET 10", "The third line must be: Choice: .NET 10"),
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
