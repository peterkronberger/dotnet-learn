// Test code for event-task.cs (w03-l02). Runs the learner program and checks its output and the declaration.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        var sensor = typeof(TaskTests).Assembly.GetType("Sensor");
        var flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
        var isEvent = sensor?.GetEvent("Alarm") is not null;
        var publicField = sensor?.GetField("Alarm", flags) is not null;
        return
        [
            ("The alarm is printed once", true, lines.Length == 1 && lines[0] == "alarm: 95 degrees", "The program still prints one line: alarm: 95 degrees."),
            ("Alarm is an event", true, isEvent, "Declare the member with the keyword event."),
            ("No public delegate field", true, !publicField, "Outside code must not be able to assign or invoke Alarm."),
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
