// Test code for skip-multiples-task.cs (w01-l02). Runs the learner program and checks its output.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("One line is printed", true, lines.Length == 1, "The program prints exactly one line."),
            ("Multiples of 3 are skipped", true, lines.Length == 1 && lines[0] == "Sum: 12", "The sum must be 12: 1 + 2 + 4 + 5. Use continue when i % 3 == 0, before the line that adds i."),
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
