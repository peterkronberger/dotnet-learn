// Test code for seclog-oop-task.cs (m08-l08). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 174 lines are printed", true, lines.Length == 174, "The output has 174 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 171 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[121] == "Unknown status: unknown" && lines[145] == "Accepted lines: 1" && lines[148] == "Events from sample.log: 14" && lines[149] == "File not found: missing.log" && lines[154] == "As text: Rule: failed logins (threshold 5)" && lines[161] == "DetectionRule sealed: False" && lines[162] == "bob: 2 of 3 rules match" && lines[163] == "dave: 2 of 3 rules match" && lines[164] == "failed logins: threshold 5" && lines[165] == "Rules from DetectionRule: 2 of 3" && lines[166] == "Event type: LoginFailed" && lines[167] == "Severity of bob: High" && lines[168] == "Severity of dave: Low" && lines[169] == "Number of High: 3" && lines[170] == "Medium parsed: True (Medium)" && lines[171] == "Critical parsed: False (None)", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The window counts the failed logins", true, lines.Length > 172 && lines[172] == "In the window: 5 of 5", "Create TimeWindow bobWindow from bobTimes[0] and window, count the times for which Contains is true, and print the count."),
            ("The copy has the same start", true, lines.Length > 173 && lines[173] == "Copy has the same start: True", "Copy bobWindow into windowCopy and print whether the two Start values are equal."),
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
