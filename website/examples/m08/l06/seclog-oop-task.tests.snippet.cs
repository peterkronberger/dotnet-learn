// Test code for seclog-oop-task.cs (m08-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 166 lines are printed", true, lines.Length == 166, "The output has 166 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 163 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[121] == "Unknown status: unknown" && lines[145] == "Accepted lines: 1" && lines[148] == "Events from sample.log: 14" && lines[149] == "File not found: missing.log" && lines[154] == "As text: Rule: failed logins (threshold 5)" && lines[161] == "DetectionRule sealed: False" && lines[162] == "bob: 2 of 3 rules match" && lines[163] == "dave: 2 of 3 rules match", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The threshold rule is found with is", true, lines.Length > 164 && lines[164] == "failed logins: threshold 5", "Use rule is FailedLoginRule thresholdRule and print the name and the threshold of the rule."),
            ("The rules from the base class are counted with as", true, lines.Length > 165 && lines[165] == "Rules from DetectionRule: 2 of 3", "Use rule as DetectionRule, count the results that are not null, and print the count of all rules."),
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
