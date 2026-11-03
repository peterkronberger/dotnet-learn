// Test code for seclog-collections-task.cs (m05-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 90 lines are printed", true, lines.Length == 90, "The output has 90 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 87 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[86] == "Failed lines: 7" && lines[87].StartsWith("First failed line: "), "All earlier lines must stay as they are."),
            ("Failed lines of bob", true, lines.Length > 88 && lines[88] == "Failed lines of bob: 5", "Go backwards with for and call RemoveAt(i) for every line whose user is not bob."),
            ("The original list is unchanged", true, lines.Length > 89 && lines[89] == "Failed lines in total: 7", "bobLines is a copy. failedLines.Count must still be 7."),
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
