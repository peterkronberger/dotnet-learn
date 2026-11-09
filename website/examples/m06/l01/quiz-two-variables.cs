Counter x = new Counter();
Counter y = x;
Console.WriteLine(ReferenceEquals(x, y));

class Counter
{
    public int Value = 0;
}
