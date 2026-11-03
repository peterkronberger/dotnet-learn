// Test code for seclog-types-task.cs (m04-l05). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 60 lines are printed", true, lines.Length == 60, "The output has 60 lines (an empty line does not count). The new block prints three lines after the lines about the user."),
            ("Earlier lines stay unchanged", true, lines.Length > 56 && lines[0] == "SecLog project profile" && lines[48] == "User and IP: bob 203.0.113.7" && lines[54] == "Findings: bob: 5 failed attempts; alice: 1 failed attempt;" && lines[56] == "User of broken line is missing: True", "All earlier lines must stay as they are, including the lines about the user."),
            ("The number from the input is read", true, lines.Length > 57 && lines[57] == "Failures from input: 5", "Write int? FailuresOrNull(string? text). It returns the number if int.TryParse works, otherwise null. Print the result for failedText."),
            ("A text that is not a number gives null", true, lines.Length > 58 && lines[58] == "Number in \"many\": False", "FailuresOrNull(\"many\") must return null. Print its HasValue."),
            ("An operator with null gives null", true, lines.Length > 59 && lines[59] == "Sum with a missing number is null: True", "Add FailuresOrNull(failedText) and FailuresOrNull(\"many\") and compare the sum with null."),
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
