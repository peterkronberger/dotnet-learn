// Test code for seclog-delegates-task.cs (m10-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 226 lines are printed", true, lines.Length == 226, "The output has 226 lines (an empty line does not count). The new block prints 4 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 221 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[183] == "login-failed texts: 7" && lines[198] == "Failures of carol: 0" && lines[203] == "Watch list" && lines[218] == "Interface count: 2", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The delegate is invoked for both incidents", true, lines.Length > 223 && lines[222] == "bob has many failures: True" && lines[223] == "dave has many failures: False", "Declare delegate bool IncidentCheck(Incident incident);, assign HasManyFailures to an IncidentCheck variable and call it for bobIncident and daveIncident."),
            ("The methods are passed as method groups", true, lines.Length > 225 && lines[224] == "Many failures: 1" && lines[225] == "Under investigation: 2", "Write CountMatching(incidents, HasManyFailures) and CountMatching(incidents, IsUnderInvestigation) without parentheses after the method names."),
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
