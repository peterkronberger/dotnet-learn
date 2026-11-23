// Test code for switch-order-task.cs (w04-l02). Runs the learner program and checks its output.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Three lines are printed", true, lines.Length == 3, "The program prints one line per name."),
            ("bob is exact", true, lines.Length == 3 && lines[0] == "exactly bob", "The first line for \"bob\" must be: exactly bob."),
            ("bobby and alice unchanged", true, lines.Length == 3 && lines[1] == "starts with bo" && lines[2] == "other", "bobby still starts with bo, alice is other."),
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
