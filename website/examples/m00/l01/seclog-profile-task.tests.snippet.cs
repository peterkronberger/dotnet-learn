// Test code for seclog-profile-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 2 lines are printed", true, lines.Length == 2, "The program should print exactly 2 lines."),
            ("New lines: title and purpose", true, lines.Length > 0 && lines[0] == "SecLog project profile" && lines.Length > 1 && lines[1] == "Purpose: find suspicious logins in made-up login logs", "The first two lines must be: SecLog project profile / Purpose: find suspicious logins in made-up login logs"),
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
