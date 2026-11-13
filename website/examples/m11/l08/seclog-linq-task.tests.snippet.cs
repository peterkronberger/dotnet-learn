// Test code for seclog-linq-task.cs (m11-l08). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 305 lines are printed", true, lines.Length == 305, "The output has 305 lines (an empty line does not count). The block prints 9 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 295 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[290] == "08:00 6 failures" && lines[295] == "Incident #1 bob: 5 failed logins (sample log)", "All earlier lines must stay as they are."),
            ("The first three messages are possible phishing", true, lines.Length > 298 && lines[296] == "Message 1: possible phishing" && lines[297] == "Message 2: possible phishing" && lines[298] == "Message 3: possible phishing", "Messages 1 to 3 contain http:// and a signal word."),
            ("The last three messages are harmless", true, lines.Length > 301 && lines[299] == "Message 4: harmless" && lines[300] == "Message 5: harmless" && lines[301] == "Message 6: harmless", "Messages 4 to 6 have no http:// link."),
            ("All messages have text", true, lines.Length > 302 && lines[302] == "All messages have text: True", "reportedMessages.All(m => m.Length > 0) is true because no message is empty."),
            ("The first harmless message is number 4", true, lines.Length > 303 && lines[303] == "First harmless message: 4", "First finds the first harmless text, Array.IndexOf gives its index 3, and the number is the index plus 1."),
            ("No logout of bob is reported", true, lines.Length > 304 && lines[304] == "no logout of bob", "bob has no logout event, so FirstOrDefault gives null and ?? supplies the text."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement."),
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
