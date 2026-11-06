// Test code for seclog-oop-task.cs (m08-l02). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 155 lines are printed", true, lines.Length == 155, "The output has 155 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 150 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[121] == "Unknown status: unknown" && lines[145] == "Accepted lines: 1" && lines[148] == "Events from sample.log: 14" && lines[149] == "File not found: missing.log" && lines[151] == "Threshold: 5" && lines[152] == "failed logins", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("Describe() is overridden", true, lines.Length > 150 && lines[150] == "Rule: failed logins (threshold 5)", "Make Describe() virtual in DetectionRule and override it in FailedLoginRule: base.Describe() + the threshold. The older block prints the new text, too."),
            ("The base type variable runs the override", true, lines.Length > 153 && lines[153] == "Rule: failed logins (threshold 5)", "Print baseRule.Describe() in the new block."),
            ("ToString() is overridden", true, lines.Length > 154 && lines[154] == "As text: Rule: failed logins (threshold 5)", "Override ToString() in DetectionRule so that it returns Describe(), then print $\"As text: {baseRule}\"."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement. Add the new block before it."),
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
