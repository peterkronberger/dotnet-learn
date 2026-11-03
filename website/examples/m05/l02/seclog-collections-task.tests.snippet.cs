// Test code for seclog-collections-task.cs (m05-l02). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 76 lines are printed", true, lines.Length == 76, "The output has 76 lines (an empty line does not count). The new block prints 4 new lines, one per user."),
            ("Earlier lines stay unchanged", true, lines.Length > 71 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[71] == "User of line 2: bob", "All earlier lines must stay as they are."),
            ("alice and bob", true, lines.Length > 73 && lines[72] == "alice | hour 8: 1 | hour 9: 0" && lines[73] == "bob | hour 8: 5 | hour 9: 0", "One line per user: the name, then the counts of hour 8 and hour 9, joined with ' | hour N: '."),
            ("carol and dave", true, lines.Length > 75 && lines[74] == "carol | hour 8: 0 | hour 9: 0" && lines[75] == "dave | hour 8: 0 | hour 9: 1", "Use two nested for loops with GetLength(0) and GetLength(1)."),
        ];
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
