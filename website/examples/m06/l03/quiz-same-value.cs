Counter first = new Counter(3);
Counter second = new Counter(3);
Console.WriteLine(first.SameValueAs(second));

class Counter
{
    private int value;

    public Counter(int value)
    {
        this.value = value;
    }

    public bool SameValueAs(Counter other)
    {
        return value == other.value;
    }
}
