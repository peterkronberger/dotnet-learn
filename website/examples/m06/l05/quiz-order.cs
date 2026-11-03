Counter counter = new Counter(10) { Step = 2 };
Console.WriteLine(counter.Total);

class Counter
{
    private int start;
    private int step = 1;

    public Counter(int start)
    {
        this.start = start;
        Console.WriteLine("constructor");
    }

    public int Step
    {
        get { return step; }
        set
        {
            step = value;
            Console.WriteLine("setter");
        }
    }

    public int Total => start + step;
}
