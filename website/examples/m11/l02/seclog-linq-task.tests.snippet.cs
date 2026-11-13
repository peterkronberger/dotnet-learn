// Test code for seclog-linq-task.cs (m11-l02). Runs the learner program with the preset input and checks its output lines.
public static class TaskTests
{
    public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()
    {
        var lines = RunProgram();
        return
        [
            ("Exactly 257 lines are printed", true, lines.Length == 257, "The output has 257 lines (an empty line does not count). The block prints 3 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 253 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[246] == "Alerts counted: 2" && lines[253] == "Static form: True", "All earlier lines must stay as they are."),
            ("The header comes from the static extension member", true, lines.Length > 254 && lines[254] == "Number | User | Failures | Severe", "Print Incident.TableHeader. The static member is called on the type name."),
            ("bob is severe", true, lines.Length > 255 && lines[255] == "1 | bob | 5 | True", "Print Number, User, FailureCount and IsSevere. bob has 5 failures."),
            ("dave is not severe", true, lines.Length > 256 && lines[256] == "2 | dave | 1 | False", "IsSevere is true from 5 failed logins."),
            ("IncidentExtensions has no instance state", true, HasNoFields(), "An extension block cannot store data: IncidentExtensions must not declare a field."),
            ("Program still returns an int", true, EntryPointReturnsInt(), "Keep return 0; as the last top-level statement."),
        ];
    }

    private static bool HasNoFields()
    {
        var type = typeof(TaskTests).Assembly.GetType("SecLog.Model.IncidentExtensions");
        var fields = type?.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        return type != null && fields!.Length == 0;
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
