// Test code for seclog-collections-task.cs (m05-l08). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 103 lines are printed", true, lines.Length == 103, "The output has 103 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 100 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[94] == "Distinct IP addresses: 4" && lines[96] == "Contains 203.0.113.7: True", "All earlier lines must stay as they are."),
            ("Watch bob", true, lines.Length > 101 && lines[101] == "Watch bob: 5", "Read the count of bob from failuresPerUser with TryGetValue."),
            ("Watch dave", true, lines.Length > 102 && lines[102] == "Watch dave: 1", "extendedWatchList is [.. watchList, \"dave\"]: bob first, then dave."),
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
