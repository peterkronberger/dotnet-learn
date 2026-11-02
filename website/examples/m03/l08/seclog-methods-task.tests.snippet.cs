// Test code for seclog-methods-task.cs (m03-l08). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 52 lines are printed", true, lines.Length == 52, "The output has 52 lines (an empty line does not count). Add exactly two new lines: the user from the log line and the check of the broken text."),
            ("Earlier lines stay unchanged", true, lines.Length > 51 && lines[0] == "SecLog project profile" && lines[45] == "Failed in sample: 7" && lines[51] == "dave |#", "All earlier lines must stay as they are."),
            ("User read from the line", true, lines.Length > 46 && lines[46] == "User from line: bob", "Call TryReadUser with sample line 2 and print 'User from line: ' with the user name right after the 'Failed in sample:' line."),
            ("Broken text rejected", true, lines.Length > 47 && lines[47] == "Line ok: False", "Call TryReadUser with the text \"broken line\" and print 'Line ok: ' with the bool result. The text has no user= part."),
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
