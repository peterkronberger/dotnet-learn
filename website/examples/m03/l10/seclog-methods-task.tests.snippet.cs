// Test code for seclog-methods-task.cs (m03-l10). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 54 lines are printed", true, lines.Length == 54, "The output has 54 lines (an empty line does not count). Add exactly one new line: the bar of all failures."),
            ("Earlier lines stay unchanged", true, lines.Length > 52 && lines[0] == "SecLog project profile" && lines[47] == "Line ok: False" && lines[48] == "User and IP: bob 203.0.113.7" && lines[52] == "dave |#", "All earlier lines must stay as they are, including the bar chart of the four users."),
            ("Bar of all failures", true, lines.Length > 53 && lines[53] == "all |#######", "Write the recursive method string Bar(int count) and print 'all |' followed by Bar(failures) after the bar chart."),
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
