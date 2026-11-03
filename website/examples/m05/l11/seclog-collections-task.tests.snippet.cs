// Test code for seclog-collections-task.cs (m05-l11). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 115 lines are printed", true, lines.Length == 115, "The output has 115 lines (an empty line does not count). The new block prints 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 112 && lines[0] == "SecLog project profile" && lines[68] == "Lines in the sample log: 14" && lines[111] == "Watch list: bob, dave" && lines[112] == "Line 2 is LOGIN-FAILED, ignoring case: True", "All earlier lines must stay as they are."),
            ("Five failures within the window", true, lines.Length > 113 && lines[113] == "bob: 5 failed logins within 10 minutes (08:01:40 to 08:06:48)", "Compare the timestamp at index i + 4 with the one at index i. Print the times with the format HH:mm:ss."),
            ("Time span", true, lines.Length > 114 && lines[114] == "Time span: 00:05:08", "Subtract the two points in time. The result is a TimeSpan."),
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
