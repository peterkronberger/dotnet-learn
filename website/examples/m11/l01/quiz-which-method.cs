Counter counter = new();
Console.WriteLine(counter.Next());
Console.WriteLine(counter.Next(5));

class Counter
{
    private int value;

    public int Next()
    {
        value++;
        return value;
    }
}

static class CounterExtensions
{
    public static int Next(this Counter counter)
    {
        return -1;
    }

    public static int Next(this Counter counter, int step)
    {
        return step * 10;
    }
}
