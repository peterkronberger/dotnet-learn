// Test code for seclog-types-task.cs (m04-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 63 lines are printed", true, lines.Length == 63, "The output has 63 lines (an empty line does not count). The new block prints three lines after the lines about the failures."),
            ("Earlier lines stay unchanged", true, lines.Length > 59 && lines[0] == "SecLog project profile" && lines[54] == "Findings: bob: 5 failed attempts; alice: 1 failed attempt;" && lines[57] == "Failures from input: 5" && lines[59] == "Sum with a missing number is null: True", "All earlier lines must stay as they are."),
            ("A missing user gets a replacement", true, lines.Length > 60 && lines[60] == "Shown user: unknown", "Use ?? with FindUser(\"broken line\") and the text \"unknown\"."),
            ("A member access on a possible null", true, lines.Length > 61 && lines[61] == "Length of user: 3", "Use ?. on FindUser(sampleLine) to read Length. The type of the result is int?."),
            ("A null variable gets a value once", true, lines.Length > 62 && lines[62] == "Last IP: unknown", "Declare string? lastIp = null and assign \"unknown\" with ??=."),
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
