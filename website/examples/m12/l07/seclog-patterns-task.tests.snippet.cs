// Test code for seclog-patterns-task.cs (m12-l07). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 352 lines are printed", true, lines.Length == 352, "The output has 352 lines (an empty line does not count). The block prints 6 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 345 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[338] == "Lines parsed with list patterns: 14" && lines[345] == "login-failed of bob: lock account", "All earlier lines must stay as they are."),
            ("The three states are printed", true, lines.Length > 348 && lines[346].EndsWith("Status = open }") && lines[347].EndsWith("Status = investigating }") && lines[348].EndsWith("Status = closed }"), "Print opened, investigating and done with Console.WriteLine. A record prints all its properties."),
            ("The first state is unchanged", true, lines.Length > 349 && lines[349] == "Status of the first state: open", "with creates a copy. The status of opened stays open."),
            ("Equality of the states", true, lines.Length > 351 && lines[350] == "Same state: False" && lines[351] == "Same after an empty with: True", "opened == investigating is False. opened == opened with { } is True, because a record compares its values."),
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
