// Test code for seclog-collections-task.cs (m05-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 94 lines are printed", true, lines.Length == 94, "The output has 94 lines (an empty line does not count). The new block prints 4 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 89 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[88] == "Failed lines of bob: 5" && lines[89] == "Failed lines in total: 7", "All earlier lines must stay as they are."),
            ("alice and bob", true, lines.Length > 91 && lines[90] == "alice: 1" && lines[91] == "bob: 5", "Count with TryGetValue and failuresPerUser[user] = count + 1."),
            ("carol has no entry", true, lines.Length > 92 && lines[92] == "carol: 0", "Read carol with TryGetValue: a missing key gives 0 in the out variable."),
            ("dave", true, lines.Length > 93 && lines[93] == "dave: 1", "Print one line per user of the users array, in this order."),
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
