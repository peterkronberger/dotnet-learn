// Test code for seclog-linq-task.cs (m11-l09). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 318 lines are printed", true, lines.Length == 318, "The output has 318 lines (an empty line does not count). The block prints 13 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 305 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[290] == "08:00 6 failures" && lines[304] == "no logout of bob", "All earlier lines must stay as they are."),
            ("GroupJoin counts the log events per incident", true, lines.Length > 308 && lines[305] == "Log events per incident" && lines[306] == "- bob 5" && lines[307] == "- dave 3", "GroupJoin pairs every incident with all log events of its user: bob has 5 events, dave has 3."),
            ("Intersect finds users who failed and logged in", true, lines.Length > 310 && lines[308] == "Failed and logged in" && lines[309] == "- alice" && lines[310] == "- dave", "alice and dave are in both user sets."),
            ("Except finds users who never logged in", true, lines.Length > 312 && lines[311] == "Failed, never logged in" && lines[312] == "- bob", "Only bob is in the failed users but not in the successful users."),
            ("Union lists all users with a login attempt", true, lines.Length > 317 && lines[313] == "All users with a login" && lines[314] == "- bob" && lines[315] == "- alice" && lines[316] == "- dave" && lines[317] == "- carol", "Union keeps each user once, in the order of first appearance."),
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
