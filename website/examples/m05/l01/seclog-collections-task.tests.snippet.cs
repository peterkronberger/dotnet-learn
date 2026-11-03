// Test code for seclog-collections-task.cs (m05-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 72 lines are printed", true, lines.Length == 72, "The output has 72 lines (an empty line does not count). The new block prints 4 new lines after the lines about the user names."),
            ("Earlier lines stay unchanged", true, lines.Length > 67 && lines[0] == "SecLog project profile" && lines[65] == "Is bob: True" && lines[67] == "Same findings object: True", "All earlier lines must stay as they are."),
            ("Length gives the number of lines", true, lines.Length > 68 && lines[68] == "Lines in the sample log: 14", "Print sampleLog.Length."),
            ("First and last line", true, lines.Length > 70 && lines[69] == "First line: 2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10" && lines[70] == "Last line: 2026-03-02T09:30:00Z logout user=alice ip=192.0.2.10", "Use sampleLog[0] and sampleLog[sampleLog.Length - 1]."),
            ("User of line 2", true, lines.Length > 71 && lines[71] == "User of line 2: bob", "Call FindUser(sampleLog[1])."),
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
