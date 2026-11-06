// Task 1: write ThresholdEventArgs (derives from EventArgs, property Level)
// Task 2: declare the event Reached with EventHandler<ThresholdEventArgs>?
// Task 3: raise it in OnReached with ?.Invoke(this, e), call it in Check
Gauge gauge = new();
gauge.Check(7);

sealed class Gauge
{
    public void Check(int level)
    {
        Console.WriteLine($"checked {level}");
    }
}
