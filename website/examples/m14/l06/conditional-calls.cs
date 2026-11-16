#define TRACE_STEPS
using System.Diagnostics;

int evaluated = 0;
Tracer.Trace(Next());
Tracer.Verbose(Next());
Console.WriteLine($"Next() ran {evaluated} time(s)");

int Next()
{
    evaluated++;
    return evaluated;
}

static class Tracer
{
    [Conditional("TRACE_STEPS")]
    public static void Trace(int value)
    {
        Console.WriteLine($"Trace: {value}");
    }

    [Conditional("VERBOSE_STEPS")]
    public static void Verbose(int value)
    {
        Console.WriteLine($"Verbose: {value}");
    }
}
