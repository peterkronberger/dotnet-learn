// Test code for seclog-async-task.cs (m13-l06). Runs the async learner program with the preset input and checks its output lines.
using System.Reflection;

public static class TaskTests
{
    public static async Task<(string Name, bool Visible, bool Passed, string Feedback)[]> Run()
    {
        var lines = await RunProgramAsync();
        return
        [
            ("Exactly 374 lines are printed", true, lines.Length == 374, "The output has 374 lines (an empty line does not count). The new block prints 1 line."),
            ("Earlier lines stay unchanged", true, lines.Length > 372 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[366] == "Failures in vpn: 2" && lines[371] == "Loading web: canceled (TaskCanceledException)" && lines[372] == "Slow source: canceled", "All earlier lines must stay as they are."),
            ("ReadSourceLinesAsync is an async stream", true, ReadSourceLinesAsyncReturnsAsyncStream(), "Write the local function async IAsyncEnumerable<string> ReadSourceLinesAsync(string name, CancellationToken token = default) with yield return."),
            ("The failures of all sources are counted", true, lines.Length > 373 && lines[373] == "Failures in all sources: 7", "Use await foreach over the sources \"web\" and \"vpn\", count the lines with login-failed and print: Failures in all sources: 7"),
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

    private static bool ReadSourceLinesAsyncReturnsAsyncStream()
    {
        var entryPoint = typeof(TaskTests).Assembly.EntryPoint!;
        const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        return entryPoint.DeclaringType!.GetMethods(flags)
            .Any(m => m.Name.Contains("ReadSourceLinesAsync") && m.ReturnType == typeof(IAsyncEnumerable<string>));
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
