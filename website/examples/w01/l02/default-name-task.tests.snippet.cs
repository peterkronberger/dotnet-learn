// Test code for default-name-task.cs (w01-l02). Runs the learner program and checks its output.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("One line is printed", true, lines.Length == 1, "The program prints exactly one line."),
            ("Fallback is used without input", true, lines.Length == 1 && lines[0] == "Name: unknown", "The input is empty, so line is null. Use line ?? \"unknown\"."),
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
