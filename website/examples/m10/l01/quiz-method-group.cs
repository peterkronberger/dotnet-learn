NumberCheck first = IsEven;
Console.WriteLine("A");
bool second = first(2);
Console.WriteLine("B");

static bool IsEven(int number)
{
    Console.WriteLine("C");
    return number % 2 == 0;
}

delegate bool NumberCheck(int number);
