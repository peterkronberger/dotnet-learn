// Test code for seclog-collections-task.cs (m05-l10). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 113 lines are printed", true, lines.Length == 113, "The output has 113 lines (an empty line does not count). The new block prints 5 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 107 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[102] == "Watch dave: 1" && lines[107] == "Alert: 2026-03-02T08:06:48Z", "All earlier lines must stay as they are."),
            ("Events per type", true, lines.Length > 110 && lines[108] == "login-success: 5" && lines[109] == "login-failed: 7" && lines[110] == "logout: 2", "Split each line at the spaces: the event type is the part with index 1. Count it in a dictionary and print the types in the given order."),
            ("Watch list", true, lines.Length > 111 && lines[111] == "Watch list: bob, dave", "Join the names of extendedWatchList with string.Join(\", \", ...)."),
            ("Ignoring case", true, lines.Length > 112 && lines[112] == "Line 2 is LOGIN-FAILED, ignoring case: True", "Compare with string.Equals(a, b, StringComparison.OrdinalIgnoreCase)."),
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
