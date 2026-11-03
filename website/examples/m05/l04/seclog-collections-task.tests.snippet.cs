// Test code for seclog-collections-task.cs (m05-l04). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 88 lines are printed", true, lines.Length == 88, "The output has 88 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 85 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[85] == "Date of line 1: 2026-03-02", "All earlier lines must stay as they are."),
            ("Number of failed lines", true, lines.Length > 86 && lines[86] == "Failed lines: 7", "Add every line that contains login-failed to the list and print failedLines.Count."),
            ("First failed line", true, lines.Length > 87 && lines[87] == "First failed line: 2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7", "Print failedLines[0]."),
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
