// Test code for seclog-collections-task.cs (m05-l07). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        var addresses = lines.Length > 100 ? lines[97..101] : new string[0];
        return
        [
            ("Exactly 101 lines are printed", true, lines.Length == 101, "The output has 101 lines (an empty line does not count). The new block prints 7 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 93 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[90] == "alice: 1" && lines[93] == "dave: 1", "All earlier lines must stay as they are."),
            ("Distinct IP addresses", true, lines.Length > 94 && lines[94] == "Distinct IP addresses: 4", "Add every IP address of sampleLog to the HashSet. Count with distinctIps.Count."),
            ("Repeated IP addresses", true, lines.Length > 95 && lines[95] == "Repeated IP addresses: 10", "Count how often Add returned false (14 lines, 4 distinct addresses)."),
            ("Contains", true, lines.Length > 96 && lines[96] == "Contains 203.0.113.7: True", "Use distinctIps.Contains(\"203.0.113.7\")."),
            ("All four addresses are printed", true, addresses.Length == 4 && addresses.Contains("192.0.2.10") && addresses.Contains("203.0.113.7") && addresses.Contains("198.51.100.23") && addresses.Contains("198.51.100.8"), "Print each address of the set with foreach. The order is not fixed, so the check accepts any order."),
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
