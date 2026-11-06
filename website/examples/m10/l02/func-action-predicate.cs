// Read the type arguments: Func has a return value, Action has none
Func<int, string> describe = Describe;
Func<int, int, int> add = Add;
Action<string> log = Log;
Predicate<int> isEven = IsEven;

Console.WriteLine(describe(7));
Console.WriteLine(add(3, 4));
log("finished");
Console.WriteLine(isEven(7));

static string Describe(int number)
{
    return $"number {number}";
}

static int Add(int left, int right)
{
    return left + right;
}

static void Log(string text)
{
    Console.WriteLine($"Log: {text}");
}

static bool IsEven(int number)
{
    return number % 2 == 0;
}
