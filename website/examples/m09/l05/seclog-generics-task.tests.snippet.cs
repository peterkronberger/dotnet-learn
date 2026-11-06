// Test code for seclog-generics-task.cs (m09-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 216 lines are printed", true, lines.Length == 218, "The output has 218 lines (an empty line does not count). The new block prints 15 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 202 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[183] == "login-failed texts: 7" && lines[198] == "Failures of carol: 0" && lines[202] == "TryGet 99: False, null: True", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The watch list is printed", true, lines.Length > 205 && lines[203] == "Watch list" && lines[204] == "- bob" && lines[205] == "- dave", "Call PrintEach(\"Watch list\", extendedWatchList). It prints the title and one line per item: - bob and - dave."),
            ("The array of first lines is printed", true, lines.Length > 208 && lines[206] == "First lines" && lines[207].StartsWith("- 2026-03-02T08:00:12Z login-success") && lines[208].StartsWith("- 2026-03-02T08:01:40Z login-failed"), "Take the first two lines of the sample log with a range (sampleLog[..2]) into a string[] and print it with PrintEach."),
            ("The sorted IP addresses are printed", true, lines.Length > 213 && lines[209] == "Distinct IPs" && lines[210] == "- 192.0.2.10" && lines[211] == "- 198.51.100.23" && lines[212] == "- 198.51.100.8" && lines[213] == "- 203.0.113.7", "Copy distinctIps into a List<string>, sort it and print it with PrintEach."),
            ("The failure counts are printed", true, lines.Length > 217 && lines[214] == "Failure counts" && lines[215] == "- 1" && lines[216] == "- 5" && lines[217] == "- 1", "Call PrintEach(\"Failure counts\", failureCounts.Items). It is the Store<int> of the previous block."),
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
