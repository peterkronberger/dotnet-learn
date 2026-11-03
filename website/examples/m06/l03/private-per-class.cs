Counter a = new Counter(5);
Counter b = new Counter(5);
Counter c = new Counter(7);

Console.WriteLine(a.SameValueAs(b));
Console.WriteLine(a.SameValueAs(c));

class Counter
{
    private readonly int value;

    public Counter(int value)
    {
        this.value = value;
    }

    public bool SameValueAs(Counter other)
    {
        return value == other.value;
    }
}
