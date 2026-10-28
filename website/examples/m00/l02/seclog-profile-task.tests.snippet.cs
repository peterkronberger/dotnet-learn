// Test code for seclog-profile-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 4 lines are printed", true, lines.Length == 4, "The program should print exactly 4 lines."),
            ("Earlier lines stay unchanged: title and purpose", true, lines.Length > 0 && lines[0] == "SecLog project profile" && lines.Length > 1 && lines[1] == "Purpose: find suspicious logins in made-up login logs", "The first two lines must be: SecLog project profile / Purpose: find suspicious logins in made-up login logs"),
            ("New lines: language and platform", true, lines.Length > 2 && lines[2] == "Language: C#" && lines.Length > 3 && lines[3] == "Platform: .NET", "Lines 3 and 4 must be: Language: C# / Platform: .NET"),
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
