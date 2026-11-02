// Test code for seclog-methods-task.cs (m03-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 49 lines are printed", true, lines.Length == 49, "The output has 49 lines (an empty line does not count). Add exactly two new lines: the two calls of your formatting method."),
            ("Earlier lines stay unchanged", true, lines.Length > 48 && lines[0] == "SecLog project profile" && lines[42] == "Failure rate (%): 50.0" && lines[48] == "dave |#", "All earlier lines must stay as they are."),
            ("Log line formatted", true, lines.Length > 43 && lines[43] == "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7", "Call FormatEvent with timestamp, eventType, the literal \"bob\" and ip, and print the result right after the 'Failure rate (%):' line."),
            ("Default IP used", true, lines.Length > 44 && lines[44] == "2026-03-02T08:42:11Z logout user=carol ip=unknown", "Call FormatEvent with named arguments user, eventType and timestamp, and no ip. The default 'unknown' is used."),
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
