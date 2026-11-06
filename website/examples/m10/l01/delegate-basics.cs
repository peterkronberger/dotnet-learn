// Create delegate values from method names and invoke them
NumberCheck check = IsEven;
Console.WriteLine(check(4));
Console.WriteLine(check.Invoke(7));

check = IsPositive;
Console.WriteLine(check(-3));

// A delegate to an instance method keeps its target object
Limit limit = new Limit(10);
NumberCheck belowLimit = limit.IsBelow;
Console.WriteLine(belowLimit(8));
limit.Value = 5;
Console.WriteLine(belowLimit(8));

static bool IsEven(int number)
{
    return number % 2 == 0;
}

static bool IsPositive(int number)
{
    return number > 0;
}

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
