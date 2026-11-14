// Test code for seclog-patterns-task.cs (m12-l04). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 338 lines are printed", true, lines.Length == 338, "The output has 338 lines (an empty line does not count). The block prints 5 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 332 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[330] == "Failed logins of bob: 5" && lines[332] == "Incident #1 bob: 5 failed logins (sample log)", "All earlier lines must stay as they are."),
            ("The three parts of event 1 are printed", true, lines.Length > 335 && lines[333] == "Time: 2026-03-02 08:01" && lines[334] == "Type: login-failed" && lines[335] == "User: bob", "Add Deconstruct to LogEvent and use var (time, type, user) = logEvents[1]; Print the three parts as Time:, Type: and User:."),
            ("Lines from 203.0.113.7 are counted", true, lines.Length > 336 && lines[336] == "Lines from 203.0.113.7: 5", "Use ReadUserAndIp(line) is (_, \"203.0.113.7\"). The first part is ignored with _."),
            ("Failed logins with a user are counted", true, lines.Length > 337 && lines[337] == "Failed logins with a user: 7", "Use ToSecurityEvent(e) is (_, EventType.LoginFailed, var who) and check who is not null."),
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
