NumberCheck? check = null;

try
{
    Console.WriteLine(check!(4));
}
catch (NullReferenceException ex)
{
    Console.WriteLine(ex.GetType().Name);
}

Console.WriteLine(check?.Invoke(4) is null);

check = IsEven;
Console.WriteLine(check?.Invoke(4));

static bool IsEven(int number)
{
    return number % 2 == 0;
}

delegate bool NumberCheck(int number);
