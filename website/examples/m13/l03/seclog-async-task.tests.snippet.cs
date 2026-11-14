// Test code for seclog-async-task.cs (m13-l03). Runs the async learner program with the preset input and checks its output lines.
using System.Reflection;

public static class TaskTests
{
    public static async Task<(string Name, bool Visible, bool Passed, string Feedback)[]> Run()
    {
        var lines = await RunProgramAsync();
        return
        [
            ("Exactly 369 lines are printed", true, lines.Length == 369, "The output has 369 lines (an empty line does not count). The new block prints 2 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 366 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[357] == "resolved: false alarm" && lines[363] == "Loaded web: 7 lines" && lines[366] == "Failures in vpn: 2", "All earlier lines must stay as they are."),
            ("The error is caught at the await", true, lines.Length > 367 && lines[367] == "Source backup failed: KeyNotFoundException", "Await LoadSourceAsync(\"backup\") inside try and catch KeyNotFoundException. Print: Source backup failed: KeyNotFoundException"),
            ("The task object holds the error", true, lines.Length > 368 && lines[368] == "Faulted: True", "Keep the task in a variable, await it inside try, catch the exception and print: Faulted: True"),
            ("The program has an async entry point", true, AsyncEntryPointReturnsTaskOfInt(), "Keep return 0; as the last top-level statement."),
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
