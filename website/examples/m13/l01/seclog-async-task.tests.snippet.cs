// Test code for seclog-async-task.cs (m13-l01). Runs the async learner program with the preset input and checks its output lines.
using System.Reflection;

public static class TaskTests
{
    public static async Task<(string Name, bool Visible, bool Passed, string Feedback)[]> Run()
    {
        var lines = await RunProgramAsync();
        return
        [
            ("Exactly 364 lines are printed", true, lines.Length == 364, "The output has 364 lines (an empty line does not count). The new block prints 1 line."),
            ("Earlier lines stay unchanged", true, lines.Length > 362 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[338] == "Lines parsed with list patterns: 14" && lines[351] == "Same after an empty with: True" && lines[357] == "resolved: false alarm" && lines[362] == "dave: 2 of 4 rules match", "All earlier lines must stay as they are."),
            ("The web source is loaded asynchronously", true, lines.Length > 363 && lines[363] == "Loaded web: 7 lines", "Load the source with await LoadSourceAsync(\"web\") and print: Loaded web: 7 lines"),
            ("The program has an async entry point", true, AsyncEntryPointReturnsTaskOfInt(), "Keep return 0; as the last top-level statement. Using await makes the generated entry point return Task<int>."),
        ];
    }

    private static MethodInfo? FindAsyncEntryPoint()
    {
        var entryPoint = typeof(TaskTests).Assembly.EntryPoint!;
        const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        return entryPoint.DeclaringType!.GetMethods(flags)
            .FirstOrDefault(m => (m.Name == "<Main>$" || m.Name == "Main") && typeof(Task).IsAssignableFrom(m.ReturnType));
    }

    private static bool AsyncEntryPointReturnsTaskOfInt()
    {
        return FindAsyncEntryPoint()?.ReturnType == typeof(Task<int>);
    }

    private static async Task<string[]> RunProgramAsync()
    {
        var original = Console.Out;
        var writer = new System.IO.StringWriter();
        Console.SetOut(writer);
        try
        {
            var entryPoint = typeof(TaskTests).Assembly.EntryPoint!;
            var asyncMethod = FindAsyncEntryPoint();
            if (asyncMethod is not null)
            {
                var args = asyncMethod.GetParameters().Length == 0 ? null : new object[] { new string[0] };
                await (Task)asyncMethod.Invoke(null, args)!;
            }
            else
            {
                entryPoint.Invoke(null, new object[] { new string[0] });
            }
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
