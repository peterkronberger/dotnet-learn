// Test code for seclog-async-task.cs (m13-l07). Runs the async learner program with the preset input and checks its output lines.
using System.Reflection;

public static class TaskTests
{
    public static async Task<(string Name, bool Visible, bool Passed, string Feedback)[]> Run()
    {
        var lines = await RunProgramAsync();
        return
        [
            ("Exactly 380 lines are printed", true, lines.Length == 380, "The output has 380 lines (an empty line does not count). The new block prints 6 lines."),
            ("Earlier lines stay unchanged", true, lines.Length > 373 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[366] == "Failures in vpn: 2" && lines[372] == "Slow source: canceled" && lines[373] == "Failures in all sources: 7", "All earlier lines must stay as they are."),
            ("LogSourceReader implements IAsyncDisposable", true, ReaderIsAsyncDisposable(), "Write sealed class LogSourceReader : IAsyncDisposable in namespace SecLog.Model."),
            ("The web reader is released after its block", true, lines.Length > 376 && lines[374] == "Failures in web: 5" && lines[375] == "Reader closed: web", "Print Failures in web: 5 inside the await using block. DisposeAsync prints Reader closed: web after it."),
            ("The vpn reader is released at the end of its block", true, lines.Length > 379 && lines[376] == "Failures in vpn: 2" && lines[377] == "End of the block" && lines[378] == "Reader closed: vpn" && lines[379] == "After the blocks", "Use an await using declaration inside a small block. The reader closes when the block ends."),
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

    private static bool ReaderIsAsyncDisposable()
    {
        var type = typeof(TaskTests).Assembly.GetType("SecLog.Model.LogSourceReader");
        return type is not null && type.IsSealed && typeof(IAsyncDisposable).IsAssignableFrom(type) && !typeof(IDisposable).IsAssignableFrom(type);
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
