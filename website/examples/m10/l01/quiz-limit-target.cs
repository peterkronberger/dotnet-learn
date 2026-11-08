Limit limit = new Limit(10);
NumberCheck below = limit.IsBelow;
limit.Value = 5;
Console.WriteLine(below(8));

delegate bool NumberCheck(int number);

class Limit
{
    public Limit(int value)
    {
        Value = value;
    }

    public int Value { get; set; }

    public bool IsBelow(int number)
    {
        return number < Value;
    }
}
