// Test code for seclog-oop-task.cs (m08-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 164 lines are printed", true, lines.Length == 164, "The output has 164 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 161 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[121] == "Unknown status: unknown" && lines[145] == "Accepted lines: 1" && lines[148] == "Events from sample.log: 14" && lines[149] == "File not found: missing.log" && lines[150] == "Rule: failed logins (threshold 5)" && lines[154] == "As text: Rule: failed logins (threshold 5)" && lines[155] == "failed logins / bob: True" && lines[158] == "watch list / dave: True" && lines[161] == "DetectionRule sealed: False", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("Detector counts the rules of bob", true, lines.Length > 162 && lines[162] == "bob: 2 of 3 rules match", "Create the Detector with the three rules and print the count for bobIncident."),
            ("Detector counts the rules of dave", true, lines.Length > 163 && lines[163] == "dave: 2 of 3 rules match", "Print the count for daveIncident, too. StatusRule matches both incidents."),
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
