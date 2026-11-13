// Test code for seclog-linq-task.cs (m11-l10). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 321 lines are printed", true, lines.Length == 321, "The output has 321 lines (an empty line does not count). The block prints 3 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 317 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[290] == "08:00 6 failures" && lines[304] == "no logout of bob" && lines[317] == "- carol", "All earlier lines must stay as they are."),
            ("The query expression prints the failures per hour", true, lines.Length > 319 && lines[318] == "08:00 6 failures" && lines[319] == "09:00 1 failures", "Group the failures by Timestamp.Hour, order by the key and print the lines in the format of l07."),
            ("Query syntax and method syntax give the same rows", true, lines.Length > 320 && lines[320] == "Same rows as method syntax: True", "SequenceEqual compares the rows of both queries one by one. Anonymous objects with the same values are equal."),
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
