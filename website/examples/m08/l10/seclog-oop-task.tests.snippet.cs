// Test code for seclog-oop-task.cs (m08-l10). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 180 lines are printed", true, lines.Length == 180, "The output has 180 lines (an empty line does not count). The new block prints 3 new lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 176 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[154] == "As text: Rule: failed logins (threshold 5)" && lines[173] == "Copy has the same start: True" && lines[174] == "Same reference with ==: False" && lines[175] == "Same values with Equals: True" && lines[176] == "Distinct events: 2", "All earlier lines must stay as they are. Add the new block after the earlier blocks."),
            ("The record prints its values", true, lines.Length > 177 && lines[177] == "As record: SecurityEvent { Timestamp = 03/02/2026 08:01:40 +00:00, Type = LoginFailed, User = bob }", "Convert logEvents[1] with ToSecurityEvent and print the record. The compiler generates its ToString."),
            ("Two records with the same values are equal", true, lines.Length > 178 && lines[178] == "Records equal with ==: True", "Convert logEvents[1] twice and compare the two records with ==."),
            ("The record is deconstructed", true, lines.Length > 179 && lines[179] == "bob: LoginFailed", "Deconstruct the record with var (recordTime, recordType, who) = ... and print who and the type."),
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
