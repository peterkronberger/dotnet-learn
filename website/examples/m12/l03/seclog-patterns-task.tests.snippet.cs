// Test code for seclog-patterns-task.cs (m12-l03). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 333 lines are printed", true, lines.Length == 333, "The output has 333 lines (an empty line does not count). The block prints 3 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 329 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[328] == "Failures between 08:00 and 08:59: 6" && lines[329] == "Lines that are neither success nor logout: 7", "All earlier lines must stay as they are."),
            ("Failed logins of bob are counted", true, lines.Length > 330 && lines[330] == "Failed logins of bob: 5", "Count the events that match { EventType: \"login-failed\", User: \"bob\" }."),
            ("Events in the hour 9 are counted", true, lines.Length > 331 && lines[331] == "Events in the hour 09:00 to 09:59: 3", "Use the extended property pattern { Timestamp.Hour: 9 }."),
            ("Severe incidents are printed", true, lines.Length > 332 && lines[332] == "Incident #1 bob: 5 failed logins (sample log)", "Print the Summary() of every incident that matches { FailureCount: >= 5 }."),
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
