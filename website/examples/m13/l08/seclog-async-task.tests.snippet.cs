// Test code for seclog-async-task.cs (m13-l08). Runs the async learner program with the preset input and checks its output lines.
using System.Reflection;

public static class TaskTests
{
    public static async Task<(string Name, bool Visible, bool Passed, string Feedback)[]> Run()
    {
        var lines = await RunProgramAsync();
        return
        [
            ("Exactly 381 lines are printed", true, lines.Length == 381, "The output has 381 lines (an empty line does not count). The new block prints 1 line."),
            ("Earlier lines stay unchanged", true, lines.Length > 379 && lines[0] == "SecLog project profile" && lines[116] == "Events: 14" && lines[373] == "Failures in all sources: 7" && lines[374] == "Failures in web: 5" && lines[379] == "After the blocks", "All earlier lines must stay as they are."),
            ("NotifyAsync returns a Task", true, NotifyAsyncReturnsTask(), "Write the local function async Task NotifyAsync(Incident incident). It awaits Task.Delay(1) and prints nothing."),
            ("The handler stores the tasks and the program awaits them", true, lines.Length > 380 && lines[380] == "Notifications sent: 2", "Add a task to notifications in a normal lambda, await Task.WhenAll(notifications) and print: Notifications sent: 2"),
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

    private static bool NotifyAsyncReturnsTask()
    {
        var entryPoint = typeof(TaskTests).Assembly.EntryPoint!;
        const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        return entryPoint.DeclaringType!.GetMethods(flags)
            .Any(m => m.Name.Contains("NotifyAsync") && m.ReturnType == typeof(Task));
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
