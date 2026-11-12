// Test code for seclog-linq-task.cs (m11-l01). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 254 lines are printed", true, lines.Length == 254, "The output has 254 lines (an empty line does not count). The block prints 3 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 250 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[228] == "dave: 3 matches" && lines[246] == "Alerts counted: 2" && lines[250] == "Found: 4", "All earlier lines must stay as they are."),
            ("The failures are counted with IsFailure()", true, lines.Length > 251 && lines[251] == "Failed events: 7", "Count the events for which logEvent.IsFailure() is true. The sample log has 7 failed logins."),
            ("IsFrom works as an extension method", true, lines.Length > 252 && lines[252] == "Event 2 is from bob: True", "Call logEvents[1].IsFrom(\"bob\") like an instance method."),
            ("The static form gives the same result", true, lines.Length > 253 && lines[253] == "Static form: True", "LogEventExtensions.IsFrom(logEvents[1], \"bob\") is the same call in the static form."),
            ("LogEventExtensions has extension methods", true, HasExtensionMethods(), "Declare IsFailure and IsFrom as public static methods with this on the first parameter, in the static class LogEventExtensions."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement."),
        ];
    }

    private static bool HasExtensionMethods()
    {
        var type = typeof(TaskTests).Assembly.GetType("SecLog.Model.LogEventExtensions");
        var extension = typeof(System.Runtime.CompilerServices.ExtensionAttribute);
        return type != null
            && type.GetMethod("IsFailure")?.IsDefined(extension, false) == true
            && type.GetMethod("IsFrom")?.IsDefined(extension, false) == true;
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
