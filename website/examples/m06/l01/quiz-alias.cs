Counter p = new Counter();
Counter q = p;
q.Value = 4;
p.Value++;
Console.WriteLine(q.Value);

class Counter
{
    public int Value;
}
