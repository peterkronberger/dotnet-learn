// Test code for seclog-generics-task.cs (m09-l03). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 199 lines are printed", true, lines.Length == 199, "The output has 199 lines (an empty line does not count). The new block prints 7 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 191 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[183] == "login-failed texts: 7" && lines[185] == "Same events: 1" && lines[191] == "First rule: failed logins", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The users are counted and sorted", true, lines.Length > 194 && lines[192] == "alice: 1" && lines[193] == "bob: 5" && lines[194] == "dave: 1", "Add the user of every failed login to a Counter<string> and print every key of SortedKeys() as key: count."),
            ("The IP addresses are counted and sorted", true, lines.Length > 197 && lines[195] == "192.0.2.10: 1" && lines[196] == "198.51.100.8: 1" && lines[197] == "203.0.113.7: 5", "Use a second Counter<string> for the IP addresses of the failed logins."),
            ("A missing key counts 0", true, lines.Length > 198 && lines[198] == "Failures of carol: 0", "Print CountOf(\"carol\") of the user counter. A missing key gives 0."),
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
