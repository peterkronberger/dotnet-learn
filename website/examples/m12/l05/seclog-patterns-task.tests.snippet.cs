// Test code for seclog-patterns-task.cs (m12-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 341 lines are printed", true, lines.Length == 341, "The output has 341 lines (an empty line does not count). The block prints 3 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 337 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[330] == "Failed logins of bob: 5" && lines[336] == "Lines from 203.0.113.7: 5" && lines[337] == "Failed logins with a user: 7", "All earlier lines must stay as they are."),
            ("All sample lines are parsed", true, lines.Length > 338 && lines[338] == "Lines parsed with list patterns: 14", "Write ParseWithPatterns(line) with line.Split(' ') switch { ... } and count the results that are not null."),
            ("The line with two parts is invalid", true, lines.Length > 339 && lines[339] == "invalid", "A list pattern with four elements does not match a line with two parts. The arm _ => null catches it."),
            ("The line with an unknown event type is invalid", true, lines.Length > 340 && lines[340] == "invalid", "Allow only the three event texts with (\"login-success\" or \"login-failed\" or \"logout\") in the second position."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement."),
        ];
    }

    private static bool EntryPointReturnsInt()
    {
        return typeof(TaskTests).Assembly.EntryPoint!.ReturnType == typeof(int);
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
