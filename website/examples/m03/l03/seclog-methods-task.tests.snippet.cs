// Test code for seclog-methods-task.cs (m03-l03). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 44 lines are printed", true, lines.Length == 44, "The output has 44 lines (an empty line does not count). Add exactly one new line: the check for login-success."),
            ("Earlier lines stay unchanged", true, lines.Length > 43 && lines[0] == "SecLog project profile" && lines[24] == "Suspicious: True" && lines[26] == "5 failed attempts" && lines[43] == "dave |#", "All earlier lines must stay as they are."),
            ("Second case checked", true, lines.Length > 25 && lines[25] == "Suspicious for login-success: False", "Call your method with \"login-success\" and failedFromInput and print 'Suspicious for login-success: ' and the result right after the 'Suspicious:' line."),
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
