// Test code for seclog-event-task.cs. Runs the learner program and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 23 lines are printed", true, lines.Length == 23, "The output has 23 lines (an empty line does not count): the 20 old lines and 3 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 19 && lines[0] == "SecLog project profile" && lines[7] == "Execution model: Native AOT" && lines[8] == "First event:" && lines[13] == "Failure rate (%): 50" && lines[15] == "Share of bob (%): 71.43" && lines[16] == "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7" && lines[17] == "Failure rate: 50.0 %" && lines[18] == "Success share, cut off (%): 35" && lines[19] == "Success share, rounded (%): 36", "The first 20 lines must stay as they are. Add your new lines at the end."),
            ("Failure rate from the text \"7\"", true, lines.Length > 20 && lines[20] == "Failure rate from text (%): 50", "Line 21 must be: Failure rate from text (%): 50. Convert \"7\" with int.Parse, then compute the rate."),
            ("TryParse result of \"seven\"", true, lines.Length > 22 && lines[21] == "Second text worked: False" && lines[22] == "Second text number: 0", "Lines 22 and 23 must be: Second text worked: False and Second text number: 0."),
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
