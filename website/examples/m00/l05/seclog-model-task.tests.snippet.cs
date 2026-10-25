// Test code for seclog-model-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Three lines are printed", true, lines.Length == 3, "The program should print exactly three lines."),
            ("Needs", true, lines.Length > 1 && lines[0] == "Need: fast start" && lines[1] == "Need: no installed runtime", "The first two lines name the needs and stay as they are."),
            ("Choice", true, lines.Length > 2 && lines[2] == "Choice: Native AOT", "The third line must be: Choice: Native AOT"),
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
