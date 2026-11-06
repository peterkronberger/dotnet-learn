// Test code for seclog-generics-task.cs (m09-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 186 lines are printed", true, lines.Length == 186, "The output has 186 lines (an empty line does not count). The new block prints 3 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 182 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[176] == "Distinct events: 2" && lines[179] == "bob: LoginFailed" && lines[182] == "Severity of the first: High", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The event type texts are counted", true, lines.Length > 183 && lines[183] == "login-failed texts: 7", "Call CountOf<string>(typeNames, \"login-failed\") with the list of event type strings of the sample log."),
            ("The event type values are counted", true, lines.Length > 184 && lines[184] == "LoginFailed values: 7", "Call CountOf with a list of EventType values and EventType.LoginFailed. The compiler can infer the type argument."),
            ("The LogEvent objects are counted", true, lines.Length > 185 && lines[185] == "Same events: 1", "Call CountOf with logEvents and one of its items. Equals(item, value) compares the values of LogEvent."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement. Add the new block before it."),
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
