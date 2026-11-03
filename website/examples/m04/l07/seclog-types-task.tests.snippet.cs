// Test code for seclog-types-task.cs (m04-l07). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 65 lines are printed", true, lines.Length == 65, "The output has 65 lines (an empty line does not count). The new block prints 2 new lines after the lines about the missing values."),
            ("Earlier lines stay unchanged", true, lines.Length > 62 && lines[0] == "SecLog project profile" && lines[54] == "Findings: bob: 5 failed attempts; alice: 1 failed attempt;" && lines[59] == "Sum with a missing number is null: True" && lines[62] == "Last IP: unknown", "All earlier lines must stay as they are."),
            ("The raw user keeps its spaces", true, lines.Length > 63 && lines[63] == "Raw user: [  Bob ]", "Print rawUser between square brackets. Trim() does not change it."),
            ("The clean user is trimmed and lower case", true, lines.Length > 64 && lines[64] == "Clean user: [bob]", "Store rawUser.Trim().ToLower() in cleanUser and print it between square brackets."),
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
