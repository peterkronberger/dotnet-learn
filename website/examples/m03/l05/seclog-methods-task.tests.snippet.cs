// Test code for seclog-methods-task.cs (m03-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 47 lines are printed", true, lines.Length == 47, "The output has 47 lines (an empty line does not count). Add exactly two new lines: the failures and the failure rate."),
            ("Earlier lines stay unchanged", true, lines.Length > 46 && lines[0] == "SecLog project profile" && lines[38] == "203.0.113.7 has 3 dots" && lines[40] == "Digits in IP: 8" && lines[46] == "dave |#", "All earlier lines must stay as they are."),
            ("Failures reported", true, lines.Length > 41 && lines[41] == "Failures: 7", "Print Report.Line(\"Failures\", failures) right after the 'Digits in IP:' line."),
            ("Failure rate reported", true, lines.Length > 42 && lines[42] == "Failure rate (%): 50.0", "Print Report.Line(\"Failure rate (%)\", failureRate). The double overload must use one decimal place."),
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
