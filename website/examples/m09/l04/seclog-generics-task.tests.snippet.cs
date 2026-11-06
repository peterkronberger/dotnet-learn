// Test code for seclog-generics-task.cs (m09-l04). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 203 lines are printed", true, lines.Length == 203, "The output has 203 lines (an empty line does not count). The new block prints 4 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 198 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[183] == "login-failed texts: 7" && lines[185] == "Same events: 1" && lines[191] == "First rule: failed logins" && lines[198] == "Failures of carol: 0", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("A missing event is null", true, lines.Length > 199 && lines[199] == "Event 99: (none)", "Add GetOrDefault to Store<T>. Print the Describe() of the item at index 99 of the event store, or (none) if it is null."),
            ("A missing number is 0", true, lines.Length > 200 && lines[200] == "Count 99: 0", "Fill a Store<int> with the failure counts and print GetOrDefault(99). For int the default value is 0."),
            ("TryGet finds the second count", true, lines.Length > 201 && lines[201] == "TryGet 1: True, 5", "Call TryGet(1, out int secondCount) on the Store<int> and print the result and the item."),
            ("TryGet reports a missing event", true, lines.Length > 202 && lines[202] == "TryGet 99: False, null: True", "Call TryGet(99, out LogEvent? missing) on the event store and print the result and whether the item is null."),
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
