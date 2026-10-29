// Test code for seclog-event-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 10 lines are printed", true, lines.Length == 10, "The program should print the 8 profile lines, the heading and the event line (an empty line does not count)."),
            ("Profile lines stay unchanged", true, lines.Length > 7 && lines[0] == "SecLog project profile" && lines[1] == "Purpose: find suspicious logins in made-up login logs" && lines[2] == "Language: C#" && lines[3] == "Platform: .NET" && lines[4] == "Variant: .NET (Linux server and Windows laptops)" && lines[5] == "Version: .NET 10 (LTS)" && lines[6] == "End of support: 2028-11-14" && lines[7] == "Execution model: Native AOT", "The first 8 lines must stay the profile lines."),
            ("New line: heading", true, lines.Length > 8 && lines[8] == "First event:", "Line 9 (after the empty line) must be: First event:"),
            ("New line: first event of the sample log", true, lines.Length > 9 && lines[9] == "2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10", "Line 10 must be: 2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10"),
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
