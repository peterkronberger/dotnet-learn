// Test code for seclog-event-task.cs. Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 23 lines are printed", true, lines.Length == 23, "The output has 23 lines (an empty line does not count). Keep the number of lines."),
            ("User comes from the first input line", true, lines.Length > 15 && lines[10] == "User: bob" && lines[16] == "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7", "Read the user with Console.ReadLine() and keep the preset input (bob) in the input field. The lines User: bob and the log line must use it."),
            ("Earlier lines stay unchanged", true, lines.Length > 19 && lines[0] == "SecLog project profile" && lines[7] == "Execution model: Native AOT" && lines[8] == "First event:" && lines[13] == "Failure rate (%): 50" && lines[15] == "Share of bob (%): 71.43" && lines[17] == "Failure rate: 50.0 %" && lines[18] == "Success share, cut off (%): 35" && lines[19] == "Success share, rounded (%): 36", "The other lines of the first 20 must stay as they are."),
            ("Failure rate from the second input line", true, lines.Length > 20 && lines[20] == "Failure rate from input (%): 35.7", "Line 21 must be: Failure rate from input (%): 35.7. Read the second line (5), convert it with int.Parse, compute the rate and print it with one decimal place ({value:F1})."),
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
