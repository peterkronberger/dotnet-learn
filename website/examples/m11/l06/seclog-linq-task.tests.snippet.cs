// Test code for seclog-linq-task.cs (m11-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 291 lines are printed", true, lines.Length == 291, "The output has 291 lines (an empty line does not count). The block prints 8 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 282 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[257] == "Failed logins, newest first" && lines[269] == "Events in the log: 14" && lines[282] == "Failed logins in the query now: 4", "All earlier lines must stay as they are."),
            ("7 failure rows are printed", true, lines.Length > 289 && lines[283] == "{ User = bob, Hour = 8 }" && lines[287] == "{ User = bob, Hour = 8 }" && lines[288] == "{ User = alice, Hour = 8 }" && lines[289] == "{ User = dave, Hour = 9 }", "Select(e => new { e.User, e.Timestamp.Hour }) gives the member names User and Hour. The 7 failures are 5 of bob, 1 of alice and 1 of dave."),
            ("The two hand-built rows are equal", true, lines.Length > 290 && lines[290] == "Same rows: True", "Two anonymous objects with the same names, types and order get the same type, and Equals compares all properties."),
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
