// Test code for seclog-exceptions-task.cs (m07-l06). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 146 lines are printed", true, lines.Length == 146, "The output has 146 lines (an empty line does not count). The new block prints 5 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 140 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[121] == "Unknown status: unknown" && lines[139] == "Processed: 2, rejected: 1" && lines[140] == "Rejected event: The value cannot be an empty string. (Parameter 'eventType')", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("Good line is accepted", true, lines.Length > 141 && lines[141] == "2026-03-02 08:01 login-failed bob", "ParseLine returns a LogEvent for sampleLog[1]. Print its Describe() text."),
            ("Line with a wrong part count is rejected", true, lines.Length > 142 && lines[142] == "Line 2 rejected: expected 4 parts, found 2", "Throw InvalidLogLineException when the line does not have 4 parts, and print Line 2 rejected: and the message."),
            ("Invalid timestamp keeps the inner exception", true, lines.Length > 144 && lines[143] == "Line 3 rejected: invalid timestamp" && lines[144] == "Cause: FormatException", "Catch the FormatException of TimestampOf, pass it as the inner exception, and print the type name of ex.InnerException."),
            ("Accepted lines are counted", true, lines.Length > 145 && lines[145] == "Accepted lines: 1", "Print Accepted lines: 1 at the end of the block."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement. Add the new block before it."),
        ];
    }

    private static bool EntryPointReturnsInt()
    {
        return typeof(TaskTests).Assembly.EntryPoint!.ReturnType == typeof(int);
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
