// Test code for seclog-patterns-task.cs (m12-l02). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 330 lines are printed", true, lines.Length == 330, "The output has 330 lines (an empty line does not count). The block prints 2 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 327 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[326] == "mixedItems[2] is string: True" && lines[327] == "mixedItems[3] is string: False", "All earlier lines must stay as they are."),
            ("Morning failures are counted", true, lines.Length > 328 && lines[328] == "Failures between 08:00 and 08:59: 6", "Count the lines for which IsMorningFailure is true. Use EventTypeOf(line) is \"login-failed\" and Hour is >= 8 and < 9."),
            ("Lines other than success and logout are counted", true, lines.Length > 329 && lines[329] == "Lines that are neither success nor logout: 7", "Use is not (\"login-success\" or \"logout\"). The parentheses make not apply to both texts."),
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
