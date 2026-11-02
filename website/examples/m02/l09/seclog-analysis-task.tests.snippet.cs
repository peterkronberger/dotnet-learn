// Test code for seclog-analysis-task.cs (m02-l09). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 41 lines are printed", true, lines.Length == 41, "The output has 41 lines (an empty line does not count). Add exactly four new lines at the end, one per user."),
            ("Earlier lines stay unchanged", true, lines.Length > 36 && lines[0] == "SecLog project profile" && lines[11] == "User: bob" && lines[28] == "Failure streak: 5" && lines[34] == "Dots in IP: 3" && lines[35] == "Date: 2026-03-02" && lines[36] == "Digits in IP: 8", "The first 37 lines must stay as they are."),
            ("Bars of alice and bob", true, lines.Length > 38 && lines[37] == "alice |#" && lines[38] == "bob |#####", "Print the name, then ' |', then one # per failed login. alice has 1, bob has 5."),
            ("Bars of carol and dave", true, lines.Length > 40 && lines[39] == "carol |" && lines[40] == "dave |#", "carol has 0 failures (no #), dave has 1. Use an outer for loop over the user numbers 1 to 4 and an inner loop for the #."),
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
