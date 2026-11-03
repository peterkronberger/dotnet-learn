Counter x = new Counter();
Counter y = new Counter();
x.Value = 5;
y.Value = x.Value;
x.Value = 7;
Console.WriteLine(x.Value);
Console.WriteLine(y.Value);

class Counter
{
    public int Value;
}
