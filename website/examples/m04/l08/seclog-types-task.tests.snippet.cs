// Test code for seclog-types-task.cs (m04-l08). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 68 lines are printed", true, lines.Length == 68, "The output has 68 lines (an empty line does not count). The new block prints 3 new lines after the lines about the missing values."),
            ("Earlier lines stay unchanged", true, lines.Length > 64 && lines[0] == "SecLog project profile" && lines[54] == "Findings: bob: 5 failed attempts; alice: 1 failed attempt;" && lines[62] == "Last IP: unknown" && lines[63] == "Raw user: [  Bob ]" && lines[64] == "Clean user: [bob]", "All earlier lines must stay as they are."),
            ("== compares the text of two strings", true, lines.Length > 65 && lines[65] == "Is bob: True", "Compare cleanUser == \"bob\" inside the interpolated string."),
            ("Equals compares the text of two strings", true, lines.Length > 66 && lines[66] == "Equals bob: True", "Call cleanUser.Equals(\"bob\")."),
            ("ReferenceEquals tells whether it is the same object", true, lines.Length > 67 && lines[67] == "Same findings object: True", "Call object.ReferenceEquals(findings, sameFindings)."),
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
