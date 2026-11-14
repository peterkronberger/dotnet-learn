// Test code for seclog-patterns-task.cs (m12-l10). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 363 lines are printed", true, lines.Length == 363, "The output has 363 lines (an empty line does not count). The block prints 5 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 357 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[338] == "Lines parsed with list patterns: 14" && lines[351] == "Same after an empty with: True" && lines[357] == "resolved: false alarm", "All earlier lines must stay as they are."),
            ("An open status needs attention", true, lines.Length > 358 && lines[358] == "Needs attention (OpenStatus): True", "NeedsAttention returns true for OpenStatus."),
            ("The other statuses do not", true, lines.Length > 360 && lines[359] == "Needs attention (InvestigatingStatus): False" && lines[360] == "Needs attention (ResolvedStatus): False", "NeedsAttention returns false for InvestigatingStatus and ResolvedStatus."),
            ("The new rule is part of the detector", true, lines.Length > 362 && lines[361] == "bob: 3 of 4 rules match" && lines[362] == "dave: 2 of 4 rules match", "Build a new Detector with the rules of detector plus the FuncRule FailuresAtLeast(4). bob has 5 failures, so the new rule matches him."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement."),
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
