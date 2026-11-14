// Test code for seclog-patterns-task.cs (m12-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 346 lines are printed", true, lines.Length == 346, "The output has 346 lines (an empty line does not count). The block prints 5 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 340 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[338] == "Lines parsed with list patterns: 14" && lines[339] == "invalid" && lines[340] == "invalid", "All earlier lines must stay as they are."),
            ("Bob's events get an action", true, lines.Length > 345 && lines[341] == "login-failed of bob: lock account" && lines[345] == "login-failed of bob: lock account", "Write ActionFor(securityEvent, failures) and print its result for every event of bob."),
            ("Five failed logins lock the account", true, lines.Length > 345 && lines[342] == "login-failed of bob: lock account" && lines[343] == "login-failed of bob: lock account" && lines[344] == "login-failed of bob: lock account", "The first arm is (EventType.LoginFailed, >= 5). It must come before the arm for fewer failures."),
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
