// Test code for seclog-oop-task.cs (m08-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 153 lines are printed", true, lines.Length == 153, "The output has 153 lines (an empty line does not count). The new block prints 3 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 150 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[121] == "Unknown status: unknown" && lines[145] == "Accepted lines: 1" && lines[148] == "Events from sample.log: 14" && lines[149] == "File not found: missing.log", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The inherited method is called", true, lines.Length > 150 && lines[150] == "Rule: failed logins", "Create FailedLoginRule failedRule = new(5); and print failedRule.Describe(). Describe() comes from DetectionRule."),
            ("The own property is printed", true, lines.Length > 151 && lines[151] == "Threshold: 5", "Print Threshold: followed by failedRule.Threshold."),
            ("The base type variable reaches Name", true, lines.Length > 152 && lines[152] == "failed logins", "Assign failedRule to DetectionRule baseRule and print baseRule.Name."),
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
