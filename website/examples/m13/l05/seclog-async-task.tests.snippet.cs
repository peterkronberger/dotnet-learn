// Test code for seclog-async-task.cs (m13-l05). Runs the async learner program with the preset input and checks its output lines.
using System.Reflection;

public static class TaskTests
{
    public static async Task<(string Name, bool Visible, bool Passed, string Feedback)[]> Run()
    {
        var lines = await RunProgramAsync();
        return
        [
            ("Exactly 373 lines are printed", true, lines.Length == 373, "The output has 373 lines (an empty line does not count). The new blocks print 2 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 370 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[366] == "Failures in vpn: 2" && lines[369] == "Sources loaded: 2, lines: 14" && lines[370] == "Slow source: timeout", "All earlier lines must stay as they are."),
            ("A cancelled token stops the loading", true, lines.Length > 371 && lines[371] == "Loading web: canceled (TaskCanceledException)", "Add the token parameter to LoadSourceAsync, pass it to Task.Delay, cancel the source before the call and print: Loading web: canceled (TaskCanceledException)"),
            ("CancelAfter stops the slow source", true, lines.Length > 372 && lines[372] == "Slow source: canceled", "Pass the token to LoadSlowSourceAsync and its Task.Delay, use CancelAfter(TimeSpan.FromMilliseconds(50)) and print: Slow source: canceled"),
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
