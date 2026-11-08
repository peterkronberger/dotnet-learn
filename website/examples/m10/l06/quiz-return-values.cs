Func<int> read = First;
read += Second;
read += Third;

int result = read();
Console.WriteLine(result);

static int First()
{
    return 1;
}

static int Second()
{
    return 2;
}

static int Third()
{
    return 3;
}
