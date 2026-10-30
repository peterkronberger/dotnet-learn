// Test code for seclog-event-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 20 lines are printed", true, lines.Length == 20, "The output has 20 lines (an empty line does not count): the 18 old lines and 2 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 17 && lines[0] == "SecLog project profile" && lines[7] == "Execution model: Native AOT" && lines[8] == "First event:" && lines[13] == "Failure rate (%): 50" && lines[15] == "Share of bob (%): 71.43" && lines[16] == "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7" && lines[17] == "Failure rate: 50.0 %", "The first 18 lines must stay as they are. Add your new lines at the end."),
            ("Share cut off with a cast", true, lines.Length > 18 && lines[18] == "Success share, cut off (%): 35", "Line 19 must be: Success share, cut off (%): 35. Cast the share with (int)."),
            ("Share rounded with Math.Round", true, lines.Length > 19 && lines[19] == "Success share, rounded (%): 36", "Line 20 must be: Success share, rounded (%): 36. Round with Math.Round first, then cast with (int)."),
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
