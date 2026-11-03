// Test code for seclog-types-task.cs (m04-l04). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 57 lines are printed", true, lines.Length == 57, "The output has 57 lines (an empty line does not count). The new block prints two lines after the findings line."),
            ("Earlier lines stay unchanged", true, lines.Length > 54 && lines[0] == "SecLog project profile" && lines[48] == "User and IP: bob 203.0.113.7" && lines[53] == "all |#######" && lines[54] == "Findings: bob: 5 failed attempts; alice: 1 failed attempt;", "All earlier lines must stay as they are, including the findings line."),
            ("The user of a line with a user is found", true, lines.Length > 55 && lines[55] == "User of sample line: bob", "Write string? FindUser(string line). It calls TryReadUser and returns the user name, or null. Print the user after a != null check."),
            ("A line without a user gives null", true, lines.Length > 56 && lines[56] == "User of broken line is missing: True", "FindUser(\"broken line\") must return null. Print the result of the comparison with null."),
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
