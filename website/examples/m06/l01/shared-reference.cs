Counter a = new Counter();
Counter b = a;
Counter c = new Counter();

b.Increment();
c.Increment();
c.Increment();

Console.WriteLine($"a: {a.Value}");
Console.WriteLine($"b: {b.Value}");
Console.WriteLine($"c: {c.Value}");
Console.WriteLine($"a and b are one object: {object.ReferenceEquals(a, b)}");
Console.WriteLine($"a and c are one object: {object.ReferenceEquals(a, c)}");

class Counter
{
    public int Value;

    public void Increment()
    {
        Value++;
    }
}
