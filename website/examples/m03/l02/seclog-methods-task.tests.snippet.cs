// Test code for seclog-methods-task.cs (m03-l02). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 43 lines are printed", true, lines.Length == 43, "The output has 43 lines (an empty line does not count). Add exactly one new line: bob's failures with the word."),
            ("Earlier lines stay unchanged", true, lines.Length > 42 && lines[0] == "SecLog project profile" && lines[25] == "5 failed attempts" && lines[27] == "Severity: high" && lines[42] == "dave |#", "All earlier lines must stay as they are."),
            ("Failures of bob with the word", true, lines.Length > 26 && lines[26] == "5 failed attempts", "Print bobFailures and the word from your method right after the existing '5 failed attempts' line."),
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
