// Test code for seclog-methods-task.cs (m03-l09). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 53 lines are printed", true, lines.Length == 53, "The output has 53 lines (an empty line does not count). Add exactly one new line: user and IP address from the tuple."),
            ("Earlier lines stay unchanged", true, lines.Length > 52 && lines[0] == "SecLog project profile" && lines[46] == "User from line: bob" && lines[47] == "Line ok: False" && lines[52] == "dave |#", "All earlier lines must stay as they are."),
            ("User and IP returned together", true, lines.Length > 48 && lines[48] == "User and IP: bob 203.0.113.7", "Call ReadUserAndIp with sample line 2, split the result with var (lineUser, lineIp) and print 'User and IP: ' with both values right after the 'Line ok:' line."),
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
