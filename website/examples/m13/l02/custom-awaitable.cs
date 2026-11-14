// Any type with a suitable GetAwaiter method can be awaited
using System.Runtime.CompilerServices;

Console.WriteLine("Waiting");
await new Pause();
Console.WriteLine("Done");

class Pause
{
    public TaskAwaiter GetAwaiter() => Task.Delay(10).GetAwaiter();
}
