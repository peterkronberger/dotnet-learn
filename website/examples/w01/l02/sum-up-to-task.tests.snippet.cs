// Test code for sum-up-to-task.cs (w01-l02). Runs the learner program and checks its output.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("One line is printed", true, lines.Length == 1, "The program prints exactly one line."),
            ("Recursion adds 4 + 3 + 2 + 1", true, lines.Length == 1 && lines[0] == "Sum: 10", "SumUpTo(4) must give 10. Return 0 when n is 0, otherwise n + SumUpTo(n - 1)."),
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
