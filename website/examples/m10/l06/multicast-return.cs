Func<int> read = First;
read += Second;
read += Third;

int result = read();
Console.WriteLine($"result: {result}");

static int First()
{
    Console.WriteLine("First runs");
    return 1;
}

static int Second()
{
    Console.WriteLine("Second runs");
    return 2;
}

static int Third()
{
    Console.WriteLine("Third runs");
    return 3;
}
