NumberCheck check = IsEven;
Console.WriteLine("Assigned the method group");

bool result = check(6);
Console.WriteLine($"Invoked: {result}");

static bool IsEven(int number)
{
    Console.WriteLine($"IsEven runs with {number}");
    return number % 2 == 0;
}

delegate bool NumberCheck(int number);
