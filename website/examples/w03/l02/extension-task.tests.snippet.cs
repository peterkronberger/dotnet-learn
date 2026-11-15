// Test code for extension-task.cs (w03-l02). Runs the learner program and checks its output.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Two lines are printed", true, lines.Length == 2, "The program prints the length and then the shouted text."),
            ("Shout works", true, lines.Length == 2 && lines[1] == "ANA!", "Write public static string Shout(this string text) in TextExtensions and print \"ana\".Shout()."),
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
