// Test code for async-task.cs. Runs the async program and checks its output lines.
using System.Reflection;

public static class TaskTests
{
    public static async Task<(string Name, bool Visible, bool Passed, string Feedback)[]> Run()
    {
        var lines = await RunProgramAsync();
        return
        [
            ("Task.WhenAll results are printed", true, lines.Length > 0 && lines[0] == "Counts: 5, 4", "The first line must be: Counts: 5, 4"),
            ("The cancelled delay is caught", true, lines.Contains("Delay was cancelled"), "Catch the OperationCanceledException and print: Delay was cancelled"),
            ("The async iterator yields 3 items", true, lines.Count(l => l.StartsWith("Item ")) == 3, "await foreach prints Item 1 to Item 3."),
            ("The resource is disposed before the last line", false, lines.Length >= 2 && lines[^2] == "Resource disposed" && lines[^1] == "Done", "await using disposes the resource before Done is printed."),
        ];
    }

    private static async Task<string[]> RunProgramAsync()
    {
        var original = Console.Out;
        var writer = new System.IO.StringWriter();
        Console.SetOut(writer);
        try
        {
            var entryPoint = typeof(TaskTests).Assembly.EntryPoint!;
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            var asyncMethod = entryPoint.DeclaringType!.GetMethods(flags)
                .FirstOrDefault(m => (m.Name == "<Main>$" || m.Name == "Main") && typeof(Task).IsAssignableFrom(m.ReturnType));
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
