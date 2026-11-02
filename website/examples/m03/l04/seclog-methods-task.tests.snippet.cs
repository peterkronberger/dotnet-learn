// Test code for seclog-methods-task.cs (m03-l04). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 45 lines are printed", true, lines.Length == 45, "The output has 45 lines (an empty line does not count). Add exactly one new line: the description of the IP address."),
            ("Earlier lines stay unchanged", true, lines.Length > 44 && lines[0] == "SecLog project profile" && lines[24] == "Suspicious: True" && lines[38] == "Dots in IP: 3" && lines[44] == "dave |#", "All earlier lines must stay as they are."),
            ("IP address described", true, lines.Length > 39 && lines[39] == "203.0.113.7 has 3 dots", "Write a method DescribeIp with a static local function that counts a character. Print its result right after the 'Dots in IP:' line."),
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
