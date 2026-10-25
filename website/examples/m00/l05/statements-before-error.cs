// The compiler accepted this program, so it starts. The error appears while it runs.
Console.WriteLine("Step 1: program started");
Console.WriteLine("Step 2: about to divide");

var divisor = 0;
try
{
    var result = 10 / divisor;
    Console.WriteLine($"Result: {result}");
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Run-time error: {ex.Message}");
}

Console.WriteLine("Step 3: program finished");
