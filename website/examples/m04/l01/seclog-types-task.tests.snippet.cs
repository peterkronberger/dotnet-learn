// Test code for seclog-types-task.cs (m04-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 55 lines are printed", true, lines.Length == 55, "The output has 55 lines (an empty line does not count). Add exactly one new line: the findings."),
            ("Earlier lines stay unchanged", true, lines.Length > 53 && lines[0] == "SecLog project profile" && lines[48] == "User and IP: bob 203.0.113.7" && lines[52] == "dave |#" && lines[53] == "all |#######", "All earlier lines must stay as they are, including the bar of all failures."),
            ("Both variables show the same text", true, lines.Length > 54 && lines[54] == "Findings: bob: 5 failed attempts", "Add 'using System.Text;' as the first line. Create one StringBuilder, copy the variable, append through the copy and print the original variable after 'Findings: '."),
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
