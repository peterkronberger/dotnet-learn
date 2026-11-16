using System.Runtime.CompilerServices;

Check(5 > 3);
Check(2 + 2 == 5);
Worker.Run();

static void Check(bool condition,
    [CallerArgumentExpression("condition")] string expression = "")
{
    Console.WriteLine($"{expression} -> {condition}");
}

static class Log
{
    public static void Write(string message,
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        Console.WriteLine($"[{member}, line {line}] {message}");
    }
}

static class Worker
{
    public static void Run()
    {
        Log.Write("working");
    }
}
