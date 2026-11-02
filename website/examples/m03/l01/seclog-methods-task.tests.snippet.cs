// Test code for seclog-methods-task.cs (m03-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 42 lines are printed", true, lines.Length == 42, "The output has 42 lines (an empty line does not count). Add exactly one new line: the severity of bob."),
            ("Earlier lines stay unchanged", true, lines.Length > 41 && lines[0] == "SecLog project profile" && lines[10] == "User: bob" && lines[26] == "Severity: high" && lines[41] == "dave |#", "All earlier lines must stay as they are. The severity rating only moves into a method."),
            ("Severity of bob", true, lines.Length > 27 && lines[27] == "Severity of bob: high", "Call your method a second time with bobFailures and print 'Severity of bob: ' and the result right after the 'Severity:' line."),
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
