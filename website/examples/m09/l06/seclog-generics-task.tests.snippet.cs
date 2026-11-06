// Test code for seclog-generics-task.cs (m09-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 222 lines are printed", true, lines.Length == 222, "The output has 222 lines (an empty line does not count). The new block prints 4 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 217 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[183] == "login-failed texts: 7" && lines[198] == "Failures of carol: 0" && lines[203] == "Watch list" && lines[217] == "- 1", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The store is used through its interface", true, lines.Length > 218 && lines[218] == "Interface count: 2", "Declare IStore<T>, let Store<T> implement it and fill an IStore<SecurityEvent> with two records. Print its Count."),
            ("The records are printed as objects", true, lines.Length > 221 && lines[219] == "Events as objects" && lines[220].Contains("LoginSuccess") && lines[221].Contains("LoginFailed"), "Call PrintEach<object>(\"Events as objects\", securityEvents.Items). An IReadOnlyList<SecurityEvent> converts to IEnumerable<object>."),
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
