// Test code for seclog-profile-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 7 lines are printed", true, lines.Length == 7, "The program should print exactly 7 lines."),
            ("Earlier lines stay unchanged: title and purpose", true, lines.Length > 0 && lines[0] == "SecLog project profile" && lines.Length > 1 && lines[1] == "Purpose: find suspicious logins in made-up login logs", "The first two lines must be: SecLog project profile / Purpose: find suspicious logins in made-up login logs"),
            ("Earlier lines stay unchanged: language and platform", true, lines.Length > 2 && lines[2] == "Language: C#" && lines.Length > 3 && lines[3] == "Platform: .NET", "Lines 3 and 4 must be: Language: C# / Platform: .NET"),
            ("Earlier lines stay unchanged: variant", true, lines.Length > 4 && lines[4] == "Variant: .NET (Linux server and Windows laptops)", "Line 5 must be: Variant: .NET (Linux server and Windows laptops)"),
            ("New lines: version and end of support", true, lines.Length > 5 && lines[5] == "Version: .NET 10 (LTS)" && lines.Length > 6 && lines[6] == "End of support: 2028-11-14", "Lines 6 and 7 must be: Version: .NET 10 (LTS) / End of support: 2028-11-14"),
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
